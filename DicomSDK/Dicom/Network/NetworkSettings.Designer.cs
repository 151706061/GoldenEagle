﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.261
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearCanvas.Dicom.Network {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class NetworkSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static NetworkSettings defaultInstance = ((NetworkSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new NetworkSettings())));
        
        public static NetworkSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        /// <summary>
        /// The timeout, in milliseconds, for read operations on an association.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("The timeout, in milliseconds, for read operations on an association.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30000")]
        public int ReadTimeout {
            get {
                return ((int)(this["ReadTimeout"]));
            }
        }
        
        /// <summary>
        /// The timeout, in milliseconds, for write operations on an association.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("The timeout, in milliseconds, for write operations on an association.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30000")]
        public int WriteTimeout {
            get {
                return ((int)(this["WriteTimeout"]));
            }
        }
        
        /// <summary>
        /// The length of time, in milliseconds, to wait for a connection to be established.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("The length of time, in milliseconds, to wait for a connection to be established.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10000")]
        public int ConnectTimeout {
            get {
                return ((int)(this["ConnectTimeout"]));
            }
        }
        
        /// <summary>
        /// The size of buffers used to receive incoming data.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("The size of buffers used to receive incoming data.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("118341")]
        public int ReceiveBufferSize {
            get {
                return ((int)(this["ReceiveBufferSize"]));
            }
        }
        
        /// <summary>
        /// The size of buffers used for sending data.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("The size of buffers used for sending data.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("118341")]
        public int SendBufferSize {
            get {
                return ((int)(this["SendBufferSize"]));
            }
        }
        
        /// <summary>
        /// Maximum length of a PDU.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Maximum length of a PDU.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("116794")]
        public uint LocalMaxPduLength {
            get {
                return ((uint)(this["LocalMaxPduLength"]));
            }
        }
        
        /// <summary>
        /// Maximum length of a PDU (remote).
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Maximum length of a PDU (remote).")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("116794")]
        public uint RemoteMaxPduLength {
            get {
                return ((uint)(this["RemoteMaxPduLength"]));
            }
        }
        
        /// <summary>
        /// Specifies whether or not to disable use of the Nagle algorithm on socket connections.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Specifies whether or not to disable use of the Nagle algorithm on socket connecti" +
            "ons.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DisableNagle {
            get {
                return ((bool)(this["DisableNagle"]));
            }
        }
        
        /// <summary>
        /// Specifies whether or not to combine the command and data parts of a PDU together when writing to the network.
        /// </summary>
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Configuration.SettingsDescriptionAttribute("Specifies whether or not to combine the command and data parts of a PDU together " +
            "when writing to the network.")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CombineCommandDataPdu {
            get {
                return ((bool)(this["CombineCommandDataPdu"]));
            }
        }
    }
}
