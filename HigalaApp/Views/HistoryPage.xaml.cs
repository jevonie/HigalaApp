using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        DataTemplate historyListTemplate;
        public HistoryPage()
        {
            InitializeComponent();

            _restService = new RestService();

            historyListTemplate = new DataTemplate(() =>
            {

                var lblestablishments = new Label();
                lblestablishments.SetBinding(Label.TextProperty, "establishment_name");

                var lbldate = new Label();
                lbldate.SetBinding(Label.TextProperty, "answer_date");
                var lbltime = new Label();
                lbltime.SetBinding(Label.TextProperty, "answer_time");

                var timestacklayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { lbldate, lbltime }
                };
                var finalstackLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(10,10),
                    Children = { lblestablishments, timestacklayout }
                };
                return new ViewCell { View = finalstackLayout };
            });
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
    public class MasterDateConverter : IValueConverter
    {
        public object Convert(Object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            DateTime enteredDate = (DateTime)value;
            enteredDate.ToLongDateString();
            return enteredDate;
        }

        public object ConvertBack(Object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}