using HigalaApp.Models;
using System;
using System.Drawing;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Common;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using Color = System.Drawing.Color;
using sd = System.Drawing;


namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyQRCodePage : ContentPage
    {
        public MyQRCodePage()
        {
            InitializeComponent();
            QRCodeView.BarcodeValue = App.QrCode;
            QRCodeViewImg.Source = ImageSource.FromResource("HigalaApp.Views.Image.cdo.png");
            QRCodeViewHigala.Source = ImageSource.FromResource("HigalaApp.Views.Image.higala.png");
            lblName.Text = App.CustomerName;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

          
        }


    }


}