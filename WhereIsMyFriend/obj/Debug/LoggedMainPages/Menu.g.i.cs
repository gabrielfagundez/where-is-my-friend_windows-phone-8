﻿#pragma checksum "C:\wmf_wp\WhereIsMyFriend\LoggedMainPages\Menu.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C0B4EBC0643BD4386D47EAEF27457367"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18331
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WhereIsMyFriend.LoggedMainPages {
    
    
    public partial class Menu : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid TitlePanel;
        
        internal System.Windows.Controls.Grid Tiles;
        
        internal System.Windows.Controls.Image mapImage;
        
        internal System.Windows.Controls.TextBlock map;
        
        internal System.Windows.Controls.Image friendsImage;
        
        internal System.Windows.Controls.TextBlock friends;
        
        internal System.Windows.Controls.TextBlock requestsImage;
        
        internal System.Windows.Controls.TextBlock requests;
        
        internal System.Windows.Controls.Grid ProgressBar;
        
        internal System.Windows.Controls.ProgressBar ProgressB;
        
        internal System.Windows.Controls.TextBlock Connecting;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WhereIsMyFriend;component/LoggedMainPages/Menu.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.Grid)(this.FindName("TitlePanel")));
            this.Tiles = ((System.Windows.Controls.Grid)(this.FindName("Tiles")));
            this.mapImage = ((System.Windows.Controls.Image)(this.FindName("mapImage")));
            this.map = ((System.Windows.Controls.TextBlock)(this.FindName("map")));
            this.friendsImage = ((System.Windows.Controls.Image)(this.FindName("friendsImage")));
            this.friends = ((System.Windows.Controls.TextBlock)(this.FindName("friends")));
            this.requestsImage = ((System.Windows.Controls.TextBlock)(this.FindName("requestsImage")));
            this.requests = ((System.Windows.Controls.TextBlock)(this.FindName("requests")));
            this.ProgressBar = ((System.Windows.Controls.Grid)(this.FindName("ProgressBar")));
            this.ProgressB = ((System.Windows.Controls.ProgressBar)(this.FindName("ProgressB")));
            this.Connecting = ((System.Windows.Controls.TextBlock)(this.FindName("Connecting")));
        }
    }
}

