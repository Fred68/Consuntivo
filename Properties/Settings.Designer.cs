﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPF02.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SecondSaveActive {
            get {
                return ((bool)(this["SecondSaveActive"]));
            }
            set {
                this["SecondSaveActive"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\Dropbox")]
        public string SecondSavePath {
            get {
                return ((string)(this["SecondSavePath"]));
            }
            set {
                this["SecondSavePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("consuntivoBak.txt")]
        public string SecondSaveFilename {
            get {
                return ((string)(this["SecondSaveFilename"]));
            }
            set {
                this["SecondSaveFilename"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool FirstSaveEncryptActive {
            get {
                return ((bool)(this["FirstSaveEncryptActive"]));
            }
            set {
                this["FirstSaveEncryptActive"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SecondSaveEncryptActive {
            get {
                return ((bool)(this["SecondSaveEncryptActive"]));
            }
            set {
                this["SecondSaveEncryptActive"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("antani")]
        public string FirstSavePassphrase {
            get {
                return ((string)(this["FirstSavePassphrase"]));
            }
            set {
                this["FirstSavePassphrase"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("blinda")]
        public string SecondSavePassphrase {
            get {
                return ((string)(this["SecondSavePassphrase"]));
            }
            set {
                this["SecondSavePassphrase"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool FirstSaveStorePassphrase {
            get {
                return ((bool)(this["FirstSaveStorePassphrase"]));
            }
            set {
                this["FirstSaveStorePassphrase"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SecondSaveStorePassphrase {
            get {
                return ((bool)(this["SecondSaveStorePassphrase"]));
            }
            set {
                this["SecondSaveStorePassphrase"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("12345678")]
        public string Salt {
            get {
                return ((string)(this["Salt"]));
            }
            set {
                this["Salt"] = value;
            }
        }
    }
}
