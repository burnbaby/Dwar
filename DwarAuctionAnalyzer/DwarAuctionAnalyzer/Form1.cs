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
        private bool finishedLoading = false;
        public Form1()
        {
            InitializeComponent();
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
            API.scanItem(webControl1);
        }

    }
}
