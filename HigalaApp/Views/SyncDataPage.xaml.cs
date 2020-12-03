using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HigalaApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncDataPage : ContentPage
    {
        DataServices _dataService;
        public SyncDataPage()
        {
            InitializeComponent();
            _dataService = new DataServices();
        }

        public async void OnUpdateClick(object sender, EventArgs e)
        {
            ai.IsRunning = true;
            aiLayout.IsVisible = true;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                _dataService = new DataServices();

                try
                {
                    await _dataService.SyncQuestions();
                    await _dataService.DowloadQuestionHistory();
                    await _dataService.UploadQuestionHistory();
                    await _dataService.UploadAnswers();
                    await _dataService.DowloadEstablishments();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("\t"+ ex, "ERROR ON SYNCHING DATA!!!");
                }
                App.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await DisplayAlert("Failed", "Check youre internet connection!", "ok");
            }

            aiLayout.IsVisible = false;
            ai.IsRunning = false;
        }
    }
}