﻿#pragma checksum "..\..\OpEditWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8EB6BDADF01479A30D1D78A37B4EB35E"
//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPF02;


namespace WPF02 {
    
    
    /// <summary>
    /// OpEditWindow
    /// </summary>
    public partial class OpEditWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btCancel;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btOk;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbTesto;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbNota;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpData;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbDescrizione;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbImporto;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkConsuntivo;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox chkVerificato;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbTipo;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbOperazioneStandard;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\OpEditWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbConti;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPF02;component/opeditwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\OpEditWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.btCancel = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\OpEditWindow.xaml"
            this.btCancel.Click += new System.Windows.RoutedEventHandler(this.btCancel_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btOk = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\OpEditWindow.xaml"
            this.btOk.Click += new System.Windows.RoutedEventHandler(this.btOk_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbTesto = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.tbNota = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.dpData = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 6:
            this.tbDescrizione = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.tbImporto = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.chkConsuntivo = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.chkVerificato = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 10:
            this.cbTipo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 11:
            this.tbOperazioneStandard = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.tbConti = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
