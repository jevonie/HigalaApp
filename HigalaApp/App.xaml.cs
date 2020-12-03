using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HigalaApp.Data;
using System.IO;
using HigalaApp.Views;
namespace HigalaApp
{
    public partial class App : Application
    {
        static Database database;

        public static string UserID { get; set; }
        public static string CustomerName { get; set; }
        public static string QrCode { get; set; }
        public static int FormID { get; set; }
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HigalaAppv1.db3"));

                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
