using HigalaApp.Data;
using HigalaApp.Models;
using HigalaApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        DataServices _dataService;
        public HomePage()
        {
            InitializeComponent();
            CheckLocalData();
            BackgroundImageSource = ImageSource.FromResource("HigalaApp.Views.Image.background.jpg");
            HeaderHigalaimg.Source = ImageSource.FromResource("HigalaApp.Views.Image.higala.png");
            HeaderCdoimg.Source = ImageSource.FromResource("HigalaApp.Views.Image.cdo.png");
            HeaderGoldenimg.Source = ImageSource.FromResource("HigalaApp.Views.Image.goldencdo.png");
            Title = "Higala App";
            _dataService = new DataServices();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            lblTitle.Text = App.CustomerName.ToUpper();
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await _dataService.SyncQuestions();

            }
        }

        public async void CheckLocalData()
        {
            List<QuestionFormOnline> questionsHistory = await App.Database.GetQuestionsNotSyncAsync();
           if(questionsHistory.Count > 0)
            {   
                await Navigation.PushAsync(new SyncDataPage());
            }
            
        }
        public void OnLogoutClick(object sender, EventArgs e)
        {
            App.UserID = "";
            App.CustomerName = "";
            App.Current.MainPage = new LoginPage();
        }
        async void OnScanQRcodeClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QRcodeScanPage());

        }
        async void OnViewProfileClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage());
        }
        async void OnMyQrCodeClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyQRCodePage());

        }
        async void OnHistpryClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());

        }





    }
}