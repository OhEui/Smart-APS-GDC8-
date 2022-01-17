﻿using APSUtil.Http;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APSWinForm
{
    public partial class frmWebView : Form
    {
        public string UrlAddress { get; set; }

        public frmWebView(string title, string urlAddress)
        {
            InitializeComponent();
            Text = title;
            UrlAddress = urlAddress;
           
        }
        private void frmWebView_Load(object sender, EventArgs e)
        {
            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async();
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(UrlAddress);
            }
        }


        public void UpdateWebView(string title, string urlAddress) 
        {
            Text = title;
            UrlAddress = urlAddress;

            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(UrlAddress);
            }
            Activate();
        }

        private void webView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            //Following line is MUST if you want to use WebResourceRequested Event
            webView.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            webView.CoreWebView2.Settings.UserAgent = "MY-AGENT";
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            e.Request.Headers.SetHeader("Authorization", $"Bearer {TokenStorage.AccessToken}");
        }
    }
}
