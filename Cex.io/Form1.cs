using CefSharp;
using CefSharp.WinForms;
using Kanpinar.Cex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.IO;
using System.Diagnostics;
using System.Media;
using Quartz;
using Quartz.Impl;
using Newtonsoft.Json;

namespace KanpinarCexWinform
{

    public partial class Form1 : Form
    {
        public static ChromiumWebBrowser browser;



        public static string CexUsername = "";   // Your username on Cex.io
        public static string CexApiKey = "";     // Your API key
        public static string CexApiSecret = "";  // Your API secret

        private CexApi CexClient { get; set; }

        private GhashApi GhashClient { get; set; }
        private SynchronizationContext m_SynchronizationContext;

        String page;
        public Form1()
        {
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            InitializeComponent();
            
            // Register an object in javascript named "cefCustomObject" with function of the CefCustomObject class :3
            //chromeBrowser.RegisterJsObject("cefCustomObject", new CefCustomObject(chromeBrowser, this));
            
            WindowState = FormWindowState.Maximized;

            CefSettings settings = new CefSettings();

            //Editing html enabled while running ..
#if DEBUG
            page = string.Format(@"{0}\..\..\html-resources\html", Application.StartupPath);
#endif
#if RELESE
            page=string.Format(@"{0}\html-resources\html", Application.StartupPath);
#endif

            settings.RegisterScheme(new CefCustomScheme
            {
                IsSecure = true, //treated with the same security rules as those applied to "https" URLs
                SchemeName = "app",

                SchemeHandlerFactory = new FileResourceHandlerFactory("app", "local", page, "index.html")
            });
            // Note that if you get an error or a white screen, you may be doing something wrong !
            // Try to load a local file that you're sure that exists and give the complete path instead to test
            // for example, replace page with a direct path instead :
            // String page = @"C:\Users\SDkCarlos\Desktop\afolder\index.html";

            //page = string.Format(@"{0}\html-resources\html\index.html", Application.StartupPath);
            //String page = @"C:\Users\SDkCarlos\Desktop\artyom-HOMEPAGE\index.html";

            if (!File.Exists(page + "\\index.html"))
            {
                MessageBox.Show("Error The html file doesn't exists : " + page);
            }

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
            // Create a browser component
            browser = new ChromiumWebBrowser(page + "\\index.html");


            // Allow the use of local resources in the browser
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browser.BrowserSettings = browserSettings;

            browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;


            //browser = new ChromiumWebBrowser("www.google.com")
            //{
            //    Dock = DockStyle.Fill,
            //};
            panel1.Controls.Add(browser);

            m_SynchronizationContext = SynchronizationContext.Current;
        }
        bool timerStarted = false;
        private void Browser_IsBrowserInitializedChanged(object sender, IsBrowserInitializedChangedEventArgs e)
        {
            if (e.IsBrowserInitialized)
            {
                browser.ShowDevTools();
                if (!timerStarted)
                {
                    // Grab the Scheduler instance from the Factory 
                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                    scheduler.Start();

                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<Fiyatlar>().WithIdentity("job1", "group1").Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever()).Build();

                    // Tell quartz to schedule the job using our trigger
                    scheduler.ScheduleJob(job, trigger);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string configFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\..\\..\\config.json";
            if (File.Exists(configFilePath))
            {
                var jsonfile = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(configFilePath));
                CexUsername = jsonfile.CexUsername.ToString();
                CexApiKey = jsonfile.CexApiKey.ToString();
                CexApiSecret = jsonfile.CexApiSecret.ToString();
            }
            else
            {
                using (FileStream fs = File.Create(configFilePath))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(@"
                        {
                            CexUsername : '',
                            CexApiKey: '',
                            CexApiSecret: ''
                        }");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            browser.Load("app://local/");
            browser.LoadingStateChanged += Browser_LoadingStateChanged;

        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            // ...
        }      
  

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileResourceHandlerFactory : ISchemeHandlerFactory
    {
        private string scheme, host, folder, default_filename;

        public string Scheme => scheme;

        public FileResourceHandlerFactory(string scheme, string host, string folder, string default_filename = "index.html")
        {
            this.scheme = scheme;
            this.host = host;
            this.folder = folder;
            this.default_filename = default_filename;
        }

        private string get_content(Uri uri, out string extension)
        {
            extension = "";
            // Api Calls
            if (uri.ToString().Contains("/api"))
            {
                string command = uri.ToString().Split(new string[] { "api" }, StringSplitOptions.None)[1].Substring(1);
                CexApi CexClient = new CexApi(Form1.CexUsername, Form1.CexApiKey, Form1.CexApiSecret);
                
                switch (command)
                {
                    case "ticker":
                        return Newtonsoft.Json.JsonConvert.SerializeObject(CexClient.Ticker(SymbolPair.XRP_USD).Result);
                        break;
                    default:
                        break;
                }

                var orderbook = CexClient.OrderBook(SymbolPair.XRP_USD).Result;
                
                return Newtonsoft.Json.JsonConvert.SerializeObject(
                    new
                    {
                        // -------------------------------
                        // Cex.Io Api Functions
                        // -------------------------------

                        // -------------------------------
                        // ---- Tag: Public API calls ----
                        // -------------------------------
                        // Currency_limits
                        Ticker = CexClient.Ticker(SymbolPair.XRP_USD).Result,

                        // Tickers_for_all_pairs_by_markets
                        // Last_price
                        // Last_prices_for_given_markets
                        // Converter 
                        Chart = CexClient.Chart(SymbolPair.XRP_USD).Result,
                        
                        // Historical_1m_OHLCV_Chart = CexClient.Historical_1m_OHLCV_Chart(SymbolPair.XRP_USD, DateTime.Now.AddDays(-1), "m"),
                        Orderbook =
                        new
                        {
                            Asks = orderbook.Asks.Take(15).Union(orderbook.Asks.Skip(orderbook.Asks.Count() - 5).Take(5)),
                            Bids = orderbook.Bids.Take(15).Union(orderbook.Asks.Skip(orderbook.Bids.Count() - 5).Take(5)),
                            Timestamp = orderbook.Timestamp,
                            buy_total = orderbook.buy_total,
                            sell_total = orderbook.sell_total

                        }
                        ,
                        // Trade_history = CexClient.TradeHistory(SymbolPair.XRP_USD).Result,

                        // --------------------------------
                        // ---- Tag: Private API calls ----
                        // --------------------------------
                        Account_balance = CexClient.AccountBalance().Result,
                        Open_orders = CexClient.OpenOrders(SymbolPair.XRP_USD).Result,
                        // Open_orders_by_pair
                        // Active_order_status
                        // Archived_orders

                        // Cancel_order= CexClient.CancelOrder(new TradeId(0)).Result,
                        // Cancel_all_orders_for_given_pair
                        // Place_order = CexClient.PlaceOrder(SymbolPair.XRP_USD,new Order() { Amount=1, Price=1, Type= OrderType.Buy },null).Result,
                        // Get_order_details                         
                        // Get_order_transactions
                        // Get_crypto_address
                        // Get_my_fee
                        // Cancel_replace_order
                        // Open_position
                        // Get_position
                        // Open_positions
                        // Close_position
                        // Archived_positions
                        // Get_marginal_fee                       


                    });
            }
            else // Orher Resource calls
            {                
                var path = uri.LocalPath.Substring(1);
                path = string.IsNullOrWhiteSpace(path) ? this.default_filename : path;
                extension = Path.GetExtension(path);
                return File.ReadAllText(Path.Combine(this.folder, path));
            }
        }

        IResourceHandler ISchemeHandlerFactory.Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            var uri = new Uri(request.Url);
            return ResourceHandler.FromString(get_content(uri, out var extension), extension);
        }
    }

    public class Fiyatlar : IJob
    {
        // Get Angular Scope
        string angularScopeString = "angular.element(document.getElementsByTagName('body')[0]).scope().";
        void IJob.Execute(IJobExecutionContext context)
        {
            try
            {
                // Execute javascript refresh function from Quartz ...
                if (!Form1.browser.GetMainFrame().Browser.IsLoading)
                    Form1.browser.ExecuteScriptAsync(angularScopeString + "getall();");
            }
            catch (Exception)
            {

            }
        }
    }
}
