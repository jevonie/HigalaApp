using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HigalaApp.Models;
using HigalaApp.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HigalaApp.Services;
using System.Diagnostics;
using Xamarin.Essentials;
using ZXing.Net.Mobile.Forms;

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRcodeScanPage : ContentPage
    {
        RestService _restService;
        private bool _isScanning = true;
        DataServices _dataService;
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        public QRcodeScanPage()
        {
            InitializeComponent();
            _restService = new RestService();
            _dataService = new DataServices();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

          
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Granted)
            {
                InitiateScanner();
            }
            else 
            { 
                status = await Permissions.RequestAsync<Permissions.Camera>();
                await Task.Delay(1000);
                Debug.WriteLine("\tCAMERA {0}", status);
                if (status == PermissionStatus.Granted)
                {
                    InitiateScanner();
                }
            }
            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                Debug.WriteLine("\tCAMERA DENIED {0}", status);
            }
          

        }

        private void InitiateScanner()
        {
            zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingScannerView",
            };
            zxing.OnScanResult += OnScanResult;
            overlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the qrcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = zxing.HasTorch,
                AutomationId = "zxingDefaultOverlay",
            };
            overlay.FlashButtonClicked += (sender, e) => {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };
            
            zxing.IsScanning = true;
            zxing.IsAnalyzing = true;
            _isScanning = true;

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            // The root page of your application
            Content = grid;

           
        }
        private void OnScanResult(ZXing.Result result)
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (_isScanning)
                {
                    _isScanning = false;
                    ai.IsRunning = true;
                    aiLayout.IsVisible = true;

                    Debug.WriteLine("\tTIBONG {0}", "Start Scanning");

                    EstablishmentOnline establishment = await App.Database.GetEstablismentAsync(result.Text);

                    if (establishment != null)
                    {
                        bool answer = await DisplayAlert(establishment.establishment_name, "Higala QR Code detected, Continue?", "Yes", "No");
                        Debug.WriteLine("Answer: " + answer);
                        if (answer)
                        {
                            Debug.WriteLine("\tTIBONG {0}", "Using Pure Equals Establishment search");
                            OpenScanDetails(establishment);
                        }
                        else
                        {
                            _isScanning = true;
                            aiLayout.IsVisible = false;
                            ai.IsRunning = false;
                        }

                    }
                    else
                    {

                        List<EstablishmentOnline> establishmentlist = await App.Database.GetEstablismentSearchAsync(result.Text);

                        if (establishmentlist != null)
                        {

                            foreach (EstablishmentOnline establishmentitem in establishmentlist)
                            {
                                bool answer = await DisplayAlert(establishmentitem.establishment_name, "Higala QR Code detected, Continue?", "Yes", "No");
                                Debug.WriteLine("Answer: " + answer);
                                if (answer)
                                {
                                    Debug.WriteLine("\tTIBONG {0}", "Using Simple Like Equals Establishment search");
                                    OpenScanDetails(establishmentitem);
                                }
                                else
                                {
                                    _isScanning = true;
                                    aiLayout.IsVisible = false;
                                    ai.IsRunning = false;
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert("Establishment not found!", "Try open youre internet connection and Logout then login.", "OK");
                            _isScanning = true;
                        }

                    }


                }
            });
        }
        private async void OpenScanDetails(EstablishmentOnline establishment)
        {
            Debug.WriteLine("\tTIBONG {0}", "Enter Establishment");
            var question = new QuestionFormOnline();

            //create random string
            question.question_form_id = DateTime.Now.ToString("MMddyyyymmhhss") + Guid.NewGuid().ToString().Replace("-", "").ToUpper();
            question.establishment_id = establishment.establishment_id;
            question.establishment_name = establishment.establishment_name;
            question.customer_id = App.UserID;
            question.customer_name = App.CustomerName;
            question.answer_date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            question.answer_time = DateTime.Now.ToString("HH:mm:ss");
            question.created_at = DateTime.Now;
            question.history_date = DateTime.Now;
            question.entity = establishment.has_questions;
            question.is_sync = 0;

            Debug.WriteLine("\tTIBONG {0}", "Done Setting");
            await App.Database.SaveScannedItemsAsync(question);
            App.FormID = question.question_form_id;

            if (establishment.has_questions != "1")
            {
                var detailPage = new HistoryDetailsPage();
                detailPage.Disappearing += (sender2, e2) =>
                {
                    _isScanning = true;
                };
                detailPage.BindingContext = question;
                aiLayout.IsVisible = false;
                ai.IsRunning = false;
                await Navigation.PushModalAsync(detailPage);
            }
            else
            {
                List<QuestionsReferenceOnline> questionsitems = await App.Database.GetQuestionsAllReferenceAsync();
                foreach (QuestionsReferenceOnline reference in questionsitems)
                {
                    var answermodel = new QuestionsAnswerOnline();
                    answermodel.question_id = reference.question_id;
                    answermodel.type_answer = reference.type_answer;
                    answermodel.question = reference.question;
                    answermodel.question_form_id = question.question_form_id;
                    var item = await App.Database.SaveQuestionsAnswerAsync(answermodel);

                }

                List<QuestionsAnswerOnline> questionlist = await App.Database.GetQuestionsAnswerAsync(question.question_form_id);

                var questionPage = new QuestionPage();
                questionPage.BindingContext = questionlist;

                Debug.WriteLine("\tJOINITEMS {0}", questionlist);
                questionPage.Disappearing += (sender2, e2) =>
                {
                    _isScanning = true;
                };
                aiLayout.IsVisible = false;
                ai.IsRunning = false;
                await Navigation.PushAsync(questionPage);
            }
        }

        protected override async void OnDisappearing()
        {
            _isScanning = false;

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status == PermissionStatus.Granted)
            {
                zxing.IsScanning = false;
                zxing.IsAnalyzing = false;
               
            }
            base.OnDisappearing();
        }
    }
}