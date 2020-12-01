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

namespace HigalaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRcodeScanPage : ContentPage
    {
        RestService _restService;
        private bool _isScanning = true;
        public QRcodeScanPage()
        {
            InitializeComponent();
            _restService = new RestService();
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
                    var establisment = await App.Database.GetEstablismentAsync(result.Text);
                    if (establisment != null)
                    {
                        Debug.WriteLine("\tTIBONG {0}", "Enter Establishment");
                        var question = new QuestionFormOnline();

                        //create random string
                        question.question_form_id = DateTime.Now.ToString("MMddyyyymmhhss") + Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                        question.establishment_id = establisment.establishment_id;
                        question.establishment_name = establisment.establishment_name;
                        question.customer_id = App.UserID;
                        question.customer_name = App.CustomerName;
                        question.answer_date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                        question.answer_time = DateTime.Now.ToString("hh:mm:ss");
                        question.created_at = DateTime.Now;
                        question.entity = establisment.has_questions;
                        question.is_sync = 0;

                        Debug.WriteLine("\tTIBONG {0}", "Done Setting");
                        await App.Database.SaveScannedItemsAsync(question);
                        
                        if (establisment.has_questions != "1")
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
                            foreach(QuestionsReferenceOnline reference in questionsitems)
                            {   
                                var answermodel = new QuestionsAnswerOnline();
                                answermodel.question_id = reference.question_id;
                                answermodel.type_answer = reference.type_answer;
                                answermodel.question = reference.question;
                                answermodel.question_form_id = question.question_form_id;
                                var item = await App.Database.SaveQuestionsAnswerAsync(answermodel);
                            }

                            List<QuestionsAnswerOnline> questionlist= await App.Database.GetQuestionsAnswerAsync(question.question_form_id);

                            var questionPage = new QuestionPage();
                            questionPage.BindingContext = questionlist;

                            Debug.WriteLine("\tJOINITEMS {0}", questionsitems);
                            questionPage.Disappearing += (sender2, e2) =>
                            {
                                _isScanning = true;
                            };
                            aiLayout.IsVisible = false;
                            ai.IsRunning = false;
                            await Navigation.PushAsync(questionPage);
                        }
                    }
                    else
                    {
                        var current = Connectivity.NetworkAccess;
                        if (current == NetworkAccess.Internet)
                        {
                            Debug.WriteLine("\tTIBONG {0}", "Looking to remote");

                            EstablishmentOnline remoteEstablishment = await _restService.GetEstablishmentAsync(ConstantData.HigalaApi + "getestablishment/" + result.Text);
                            if (remoteEstablishment != null)
                            {
                                Debug.WriteLine("\tTIBONG {0}", "Enter in remote");
                                EstablishmentOnline localEstablishment = await App.Database.GetEstablismentAsync(result.Text);
                                if (localEstablishment != null)
                                {
                                    Debug.WriteLine("\tTIBONG {0}", "Update");
                                    remoteEstablishment.ID = localEstablishment.ID;
                                    await App.Database.SaveEstablishmentAsync(remoteEstablishment);
                                }
                                else
                                {
                                    Debug.WriteLine("\tTIBONG {0}", "Insert");
                                    await App.Database.SaveEstablishmentAsync(remoteEstablishment);
                                }
                                DateTime dateTime = DateTime.UtcNow.Date;
                                Debug.WriteLine("\tTIBONG {0}", "save remote");

                                var question = new QuestionFormOnline();
                                question.question_form_id = DateTime.Now.ToString("MMddyyyymmhhss") + Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                question.establishment_id = remoteEstablishment.establishment_id;
                                question.establishment_name = remoteEstablishment.establishment_name;
                                question.customer_id = App.UserID;
                                question.customer_name = App.CustomerName;
                                question.answer_date = DateTime.Now.Date.ToString("yyyy-MM-dd");
                                question.answer_time = DateTime.Now.ToString("hh:mm:ss");
                                question.created_at = DateTime.Now;
                                question.entity = remoteEstablishment.has_questions;
                                question.is_sync = 0;

                                if (remoteEstablishment.has_questions != "1")
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

                                    Debug.WriteLine("\tJOINITEMS {0}", questionsitems);
                                    questionPage.Disappearing += (sender2, e2) =>
                                    {
                                        _isScanning = true;
                                    };

                                    aiLayout.IsVisible = false;
                                    ai.IsRunning = false;
                                    await Navigation.PushAsync(questionPage);
                                }

                            }
                            else
                            {
                                bool answer = await DisplayAlert("Establishment not found!", "Scan Again?", "OK", "Cancel");
                                if (answer)
                                {
                                    _isScanning = true;
                                    aiLayout.IsVisible = false;
                                    ai.IsRunning = false;
                                }
                                else
                                {
                                    await Navigation.PopAsync();
                                }

                            }
                        }
                        else
                        {
                            await DisplayAlert("Establishment not found!", "Try Opening youre internet connection for updates.", "OK");
                            _isScanning = true;
                        }
                        
                    }

                    
                }
            });
        }
    }
}