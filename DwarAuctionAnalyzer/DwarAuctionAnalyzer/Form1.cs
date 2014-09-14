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
       // public Thread myThread;
        delegate void myDelegate(string category, int i);
        private static bool finishedLoading = false;
        public Form1()
        {
            InitializeComponent();
            API.onUpdateWebControl += updateWebControl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            API.login(webControl1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            API.copyAuctionCategories(webControl1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread myThread = new Thread(API.scanItem);
            myThread.Start();
        }

        public void updateWebControl(string category, int i)
        {
            if (!InvokeRequired)
            {
                webControl1.LoadingFrameComplete += Awesomium_Windows_Forms_WebControl_LoadingFrameComplete;
                webControl1.Source = new Uri("http://w1.dwar.ru/area_auction.php?&_filter%5Btitle%5D=&_filter%5Bcount_min%5D=&_filter%5Bcount_max%5D=&_filter%5Blevel_min%5D=&_filter%5Blevel_max%5D=&_filter%5Bkind%5D=" + category + "&_filter%5Bquality%5D=-1&_filterapply=%D0%9E%D0%BA&page=" + i);
                loadPage();
            }
            else
                Invoke(new myDelegate(updateWebControl), new object[] { category, i }); 
        }
        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
            if (e.IsMainFrame)
            {
                finishedLoading = true;
                ((WebControl)sender).LoadingFrameComplete -= Awesomium_Windows_Forms_WebControl_LoadingFrameComplete;
            }

        }

        public static void loadPage()
        {
            while (!finishedLoading)
            {
                WebCore.Update();
            }
            finishedLoading = false;
        }

    }
}
