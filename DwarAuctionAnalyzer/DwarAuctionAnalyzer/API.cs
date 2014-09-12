using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Awesomium.Core;
using Awesomium.Windows.Forms;
using System.Threading;
using System.Windows.Forms;

namespace DwarAuctionAnalyzer
{
    class API
    {
        private static bool finishedLoading = false;
        public static void login (WebControl webControl1)
        {
            try
            {
                dynamic document = (JSObject)webControl1.ExecuteJavascriptWithResult("document");
                if (document == null)
                    throw new Exception("document has null value");
                dynamic element = document.getElementById("userEmail");
                if (element == null)
                    throw new Exception("email has null value");
                element.value = "ganton217@gmail.com";
                element = document.getElementById("userPassword");
                element.value = "ee34nf3o";

                element = document.getElementsByTagName("input");
                for (int i = 0; i < element.length; i++)
                {
                    if (element[i].getAttribute("src") == "images/go_btn.png") ;
                    {
                        element[i].click();
                        break;
                    }
                }
                Thread.Sleep(2000);
                webControl1.Source = new Uri("http://w1.dwar.ru/area_auction.php");
            }
            catch (Exception exception)
            {
               MessageBox.Show(exception.Message);
            }   
        }
        private static dynamic getCategories(WebControl webControl1)
        {
            try
            {
                dynamic document = (JSObject)webControl1.ExecuteJavascriptWithResult("document");
                dynamic filter = document.getElementsByName("_filter[kind]");
                dynamic categories = filter[0].getElementsByTagName("*");
                return categories;
            }
            catch (Exception exception)
            {
                return null;
            }
        }
        private static void addCategories(dynamic categories, MySqlCommand command)
        {
            for (var i = 0; i < categories.length; i++)
            {
                command.CommandText = "REPLACE INTO categories (browserValue, categoryName) VALUES(@browserValue,@categoryName)";//" + (string)categories[i].firstChild.nodeValue + "
                command.Parameters.AddWithValue("@browserValue", categories[i].getAttribute("value"));
                if (command.Parameters[0].Value.ToString().Length < 10 && command.Parameters[0].Value != "")
                {
                    command.Parameters.AddWithValue("@categoryName", categories[i].firstChild.nodeValue);
                    command.ExecuteNonQuery();
                }
                command.Parameters.Clear();
            }
        }
        public static void copyAuctionCategories(WebControl webControl1)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(@"server=localhost;userid=root;password=1547;Database=fordwar;charset=utf8");
                string dbCommand = "CREATE TABLE IF NOT EXISTS categories (browserValue NVARCHAR(10) PRIMARY KEY, categoryName NVARCHAR(50));";
                MySqlCommand command = new MySqlCommand(dbCommand, connection);
                connection.Open();
                command.ExecuteNonQuery();
                dynamic categories = getCategories(webControl1);
                addCategories(categories, command);
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private static void loadPage()
        {
            while (!finishedLoading)
            {
                WebCore.Update();
            }
            finishedLoading = false;
        }
        private static void addItems(JSValue itemData, MySqlCommand command)
        {
            if (!itemData.IsUndefined)
            {
                JSValue[] records = (JSValue[])itemData;
                foreach (JSValue[] record in records)
                {
                    command.CommandText = "REPLACE INTO items (itemID, itemName, itemCategory, itemStrength, itemTime, itemCount, itemBid, itemBuyOut) VALUES(@itemID, @itemName, @itemCategory, @itemStrength, @itemTime, @itemCount, @itemBid, @itemBuyOut)";
                    command.Parameters.AddWithValue("@itemID", (string)record[0]);
                    command.Parameters.AddWithValue("@itemName", (string)record[1]);
                    command.Parameters.AddWithValue("@itemCategory", (string)record[2]);
                    command.Parameters.AddWithValue("@itemStrength", (string)record[3]);
                    command.Parameters.AddWithValue("@itemTime", (string)record[4]);
                    command.Parameters.AddWithValue("@itemCount", (string)record[5]);
                    command.Parameters.AddWithValue("@itemBid", (string)record[6]);
                    command.Parameters.AddWithValue("@itemBuyOut", (string)record[7]);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }
        public static void scanItem(WebControl webControl1)
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(@"server=localhost;userid=root;password=1547;Database=fordwar;charset=utf8");
                MySqlConnection connection2 = new MySqlConnection(@"server=localhost;userid=root;password=1547;Database=fordwar;charset=utf8");
                MySqlCommand command = new MySqlCommand();

                string itemsForDb = JavaScripts.itemsForDb;
                string pagesNumber = JavaScripts.pagesNumber;

                connection.Open();
                connection2.Open();
                command.Connection = connection;
                command.CommandText = "CREATE TABLE IF NOT EXISTS items (itemID NVARCHAR(30) PRIMARY KEY, itemName NVARCHAR(50), itemCategory NVARCHAR(50), itemStrength NVARCHAR(10), itemTime NVARCHAR(10), itemCount NVARCHAR(10), itemBid NVARCHAR(50), itemBuyOut NVARCHAR(50));";
                command.ExecuteNonQuery();
                command.CommandText = "SELECT browserValue FROM categories";
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int i = 0;
                    command.Connection = connection2;
                    webControl1.LoadingFrameComplete += Awesomium_Windows_Forms_WebControl_LoadingFrameComplete;
                    webControl1.Source = new Uri("http://w1.dwar.ru/area_auction.php?&_filter%5Btitle%5D=&_filter%5Bcount_min%5D=&_filter%5Bcount_max%5D=&_filter%5Blevel_min%5D=&_filter%5Blevel_max%5D=&_filter%5Bkind%5D=" + reader[0] + "&_filter%5Bquality%5D=-1&_filterapply=%D0%9E%D0%BA&page=" + i);
                    loadPage();

                    JSValue itemData = webControl1.ExecuteJavascriptWithResult(itemsForDb);
                    addItems(itemData, command);

                    JSValue pages = webControl1.ExecuteJavascriptWithResult(pagesNumber);
                    int number = Convert.ToInt32((int)pages);
                    for (; i < number; i++)
                    {
                        webControl1.LoadingFrameComplete += Awesomium_Windows_Forms_WebControl_LoadingFrameComplete;
                        webControl1.Source = new Uri("http://w1.dwar.ru/area_auction.php?&_filter%5Btitle%5D=&_filter%5Bcount_min%5D=&_filter%5Bcount_max%5D=&_filter%5Blevel_min%5D=&_filter%5Blevel_max%5D=&_filter%5Bkind%5D=" + reader[0] + "&_filter%5Bquality%5D=-1&_filterapply=%D0%9E%D0%BA&page=" + i);
                        loadPage();

                        itemData = webControl1.ExecuteJavascriptWithResult(itemsForDb);
                        addItems(itemData, command);
                    }
                    command.Connection = connection;
                }
                connection.Close();
                connection2.Close();
                reader.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }   
        private static void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, FrameEventArgs e)
        {
            if (e.IsMainFrame)
            {
                finishedLoading = true;
                ((WebControl)sender).LoadingFrameComplete -= Awesomium_Windows_Forms_WebControl_LoadingFrameComplete;
            }

        }
    }
}
