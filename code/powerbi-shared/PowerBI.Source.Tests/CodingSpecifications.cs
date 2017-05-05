using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Onyx.CodeAnalysis;
using Onyx.CodeAnalysis.Analyzers;
using Onyx.CodeAnalysis.Analyzers.Resx;

namespace PowerBI.Source.Tests
{
    [TestClass]
    public class CodingSpecifications
    {
        [TestMethod]
        public void RunOnyxCodeAnalyzers()
        {
            ResxAnalyzer.ValidHtmlTags.AddRange(new []{ "u", "pre" });

            For.Solution("powerbi.sln")
                .RunAnalyzers(OnyxCodeAnalyzers.All)
                .ExcludeGeneratedFiles()
                .KnownOffenders<NoDollarsInResxAnalyzer>(x => x.AnalyzerId == NoDollarsInResxAnalyzer.Id)
                .KnownOffendersMax<UsingsStatementsMustNotBeNestedConvention>(52)
                .OnFailures(Assert.Fail);
        }

        [TestMethod]
        public void RunOnyxCodeAnalyzersJobs()
        {
            For.Solution("powerbi-jobs.sln")
                .RunAnalyzers(OnyxCodeAnalyzers.CSharp)
                .ExcludeGeneratedFiles()
                .KnownOffendersMax<UsingsStatementsMustNotBeNestedConvention>(66)
                .OnFailures(Assert.Fail);
        }
    }
}
