// Updated by XamlIntelliSenseFileGenerator 8/21/2024 8:48:35 PM
#pragma checksum "..\..\..\PlayWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "56A2EF88CA7D32CE628DA3A131C5C59DEF692075"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MusicPlayer;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace MusicPlayer
{


    /// <summary>
    /// PlayWindow
    /// </summary>
    public partial class PlayWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.7.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MusicPlayer;V1.0.0.0;component/playwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\PlayWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.7.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            this._contentLoaded = true;
        }

        internal System.Windows.Controls.Button ImportButton;
        internal System.Windows.Controls.Button NextButton;
        internal System.Windows.Controls.Button StopButton;
        internal System.Windows.Controls.Button PlayButton;
        internal System.Windows.Controls.Button PreviousButton;
        internal System.Windows.Controls.Button FavouriteButton;
        internal System.Windows.Controls.ProgressBar p_bar;
        internal System.Windows.Controls.Label CurrentTimeLabel;
        internal System.Windows.Controls.Label TotalTimeLabel;
        internal System.Windows.Controls.DataGrid SongDataGrid;
        internal System.Windows.Controls.Image song_img;
        internal System.Windows.Controls.Button PauseButton;
    }
}

