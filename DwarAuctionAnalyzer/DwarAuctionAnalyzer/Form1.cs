using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Awesomium.Core;
using Awesomium.Windows.Forms;

using System.IO;

namespace DwarAuctionAnalyzer
{
    public partial class Form1 : Form
    {
        public WebView browser;
        public Form1()
        {
            InitializeComponent();
            browser = WebCore.CreateWebView(800, 600, WebViewType.Offscreen);
            browser.Source = new Uri("http://w1.dwar.ru/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            API.login(browser);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            API.copyAuctionCategories(browser);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            API.scanItem(browser);
        }

    }
}
