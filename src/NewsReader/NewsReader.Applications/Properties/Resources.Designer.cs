﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Waf.NewsReader.Applications.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Waf.NewsReader.Applications.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Add Feed.
        /// </summary>
        internal static string AddFeed {
            get {
                return ResourceManager.GetString("AddFeed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load the RSS Feed from the provided URL..
        /// </summary>
        internal static string ErrorLoadRssFeed {
            get {
                return ResourceManager.GetString("ErrorLoadRssFeed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to reduce the feed item lifetime? 
        ///This might remove older feed items..
        /// </summary>
        internal static string ReduceFeedItemLifetimeQuestion {
            get {
                return ResourceManager.GetString("ReduceFeedItemLifetimeQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to reduce the maximum number of feed items per feed? 
        ///This might remove older feed items..
        /// </summary>
        internal static string ReduceMaxItemsLimitQuestion {
            get {
                return ResourceManager.GetString("ReduceMaxItemsLimitQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to remove the Feeds?
        ///{0}.
        /// </summary>
        internal static string RemoveFeedQuestion {
            get {
                return ResourceManager.GetString("RemoveFeedQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Settings.
        /// </summary>
        internal static string Settings {
            get {
                return ResourceManager.GetString("Settings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred during the sign-in process. 
        ///Details: {0}.
        /// </summary>
        internal static string SignInError {
            get {
                return ResourceManager.GetString("SignInError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Synchronization failed. Error: {0}.
        /// </summary>
        internal static string SynchronizationDownloadError {
            get {
                return ResourceManager.GetString("SynchronizationDownloadError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The URL must begin with http:// or https://.
        /// </summary>
        internal static string UrlMustBeginWithHttp {
            get {
                return ResourceManager.GetString("UrlMustBeginWithHttp", resourceCulture);
            }
        }
    }
}
