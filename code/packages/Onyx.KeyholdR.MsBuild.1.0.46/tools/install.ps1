param($installPath, $toolsPath, $package, $project)

$TargetsFile = 'onyx.keyholdr.msbuild.targets'
$TasksFile = 'Onyx.KeyholdR.MsBuild.dll'
$TargetsPath = $ToolsPath | Join-Path -ChildPath $TargetsFile
$TasksFilePath = $ToolsPath | Join-Path -ChildPath $TasksFile

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($Project.FullName) |
    Select-Object -First 1

$ProjectUri = New-Object -TypeName Uri -ArgumentList "file://$($Project.FullName)"
$TargetUri = New-Object -TypeName Uri -ArgumentList "file://$TargetsPath"
$RelativePath = "`$(ProjectDir)\" + $TargetsFile
$RelativeTasksFilePath = $ProjectUri.MakeRelativeUri($TasksFilePath) -replace '/','\'

$ExistingImports = $MSBProject.Xml.Imports | Where-Object { $_.Project -like "*\$TargetsFile" }
if ($ExistingImports) {
    $ExistingImports | 
        ForEach-Object {
            $MSBProject.Xml.RemoveChild($_) | Out-Null
        }
}

$existsCondition = "Exists('" + $RelativeTasksFilePath + "')"
$importElement = $MSBProject.Xml.AddImport($RelativePath)
$importElement.Condition = $existsCondition

# Update path to Task.dll

$path = [System.IO.Path]
$deployedAcomTargetsPath = $path::Combine($path::GetDirectoryName($project.FileName), $TargetsFile)

Write-Host "Found targets file: ${deployedAcomTargetsPath}"

[xml]$taskXml = Get-Content $deployedAcomTargetsPath 
$ns = New-Object System.Xml.XmlNamespaceManager -ArgumentList $taskXml.NameTable
$ns.AddNamespace('x','http://schemas.microsoft.com/developer/msbuild/2003')

$node = $taskXml.SelectSingleNode("/x:Project/x:UsingTask", $ns)
$node.SetAttribute("AssemblyFile", $RelativeTasksFilePath)

Write-Host "Setting Task Path to: $RelativeTasksFilePath"

$taskXml.Save($deployedAcomTargetsPath)

Write-Host "Saved: ${deployedAcomTargetsPath}"

$returnCode = $Project.Save()

Write-Host "Project File Updated."