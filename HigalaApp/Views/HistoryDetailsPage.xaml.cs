using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryDetailsPage : ContentPage
    {
        public HistoryDetailsPage()
        {
            InitializeComponent();
            CustomerName.Text = App.CustomerName;
            checkedImg.Source = ImageSource.FromResource("HigalaApp.Views.Image.checked.png");
        }
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
      
    }
}