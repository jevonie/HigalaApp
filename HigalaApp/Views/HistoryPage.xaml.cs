using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HigalaApp.Data;
using HigalaApp.Models;
using HigalaApp.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        RestService _restService;
        public HistoryPage()
        {
            InitializeComponent();

            _restService = new RestService();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.Database.GetQuestionsAsync(App.UserID);
        }
        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var detailPage = new HistoryDetailsPage();
                detailPage.BindingContext = e.SelectedItem as QuestionFormOnline;
                await Navigation.PushModalAsync(detailPage);
            }
        }
      
    }
}