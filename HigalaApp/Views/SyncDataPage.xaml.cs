using System;
using System.Collections.Generic;
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
                await _dataService.SyncQuestions();
                await _dataService.DowloadEstablishments();
                await _dataService.DowloadQuestionHistory();
                await _dataService.UploadQuestionHistory();
                await _dataService.UploadAnswers();
                await Navigation.PopAsync();
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