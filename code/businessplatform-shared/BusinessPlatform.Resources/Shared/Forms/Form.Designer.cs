﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BusinessPlatform.Resources.Shared.Forms {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Form {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Form() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BusinessPlatform.Resources.Shared.Forms.Form", typeof(Form).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You entered a personal email address: Please enter your work email address so we can securely store your company&apos;s data..
        /// </summary>
        public static string Email_ConsumerMessage {
            get {
                return ResourceManager.GetString("Email_ConsumerMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid email address to continue..
        /// </summary>
        public static string Email_ErrorMessage {
            get {
                return ResourceManager.GetString("Email_ErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter your work email address.
        /// </summary>
        public static string Email_Placeholder {
            get {
                return ResourceManager.GetString("Email_Placeholder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Submit.
        /// </summary>
        public static string Submit {
            get {
                return ResourceManager.GetString("Submit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thank you for completing your registration!.
        /// </summary>
        public static string ThanksForSignUp {
            get {
                return ResourceManager.GetString("ThanksForSignUp", resourceCulture);
            }
        }
    }
}
