﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.296
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.ImageViewer.Annotations.Dicom {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class DicomFilteredAnnotationLayoutStoreSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static DicomFilteredAnnotationLayoutStoreSettings defaultInstance = ((DicomFilteredAnnotationLayoutStoreSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DicomFilteredAnnotationLayoutStoreSettings())));
        
        public static DicomFilteredAnnotationLayoutStoreSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// XML document stores the identifier of the text overlay configuration for each modality.  The actual configurations are stored in the AnnotationLayoutSettings.
        /// </summary>
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("XML document stores the identifier of the text overlay configuration for each mod" +
            "ality.  The actual configurations are stored in the AnnotationLayoutSettings.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DicomFilteredAnnotationLayoutStoreDefaults.xml")]
        public string FilteredLayoutSettingsXml {
            get {
                return ((string)(this["FilteredLayoutSettingsXml"]));
            }
            set {
                this["FilteredLayoutSettingsXml"] = value;
            }
        }
    }
}
