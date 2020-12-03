using HigalaApp.Data;
using HigalaApp.Models;
using HigalaApp.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
namespace HigalaApp.Services
{
    class DataServices
    {
        RestService _restService;

        public DataServices()
        {
            _restService = new RestService();
        }
        public async Task<List<QuestionsReferenceOnline>> SyncQuestions()
        {
            //Retrieve Questions data online
            List<QuestionsReferenceOnline> questions = await _restService.GetAllReferenceQuestionAsync(ConstantData.HigalaApi + "getquestions");
            foreach (QuestionsReferenceOnline question in questions)
            {
                Debug.WriteLine("\tSUCCESS {0}", "Able to get Questions Reference Online data online " + question.question_id);
                QuestionsReferenceOnline questionLocal = await App.Database.GetQuestionsReferenceAsync(question.question_id);
                if (questionLocal != null)
                {
                    Debug.WriteLine("\tSUCCESS {0}", "Local " + questionLocal.question_id);
                    question.ID = questionLocal.ID;
                    Debug.WriteLine("\tSUCCESS {0}", "Update Questions Reference OFFline " + await App.Database.SaveQuestionsReferenceAsync(question));
                }
                else
                {
                    Debug.WriteLine("\tSUCCESS {0}", "Save Questions Reference OFFline " + await App.Database.SaveQuestionsReferenceAsync(question));
                }

            }

            return questions;
        }
        public async Task<List<QuestionsAnswerOnline>> UploadAnswers()
        {

            //Send Answers data online
            List<QuestionsAnswerOnline> AllanswersLocal = await App.Database.GetAllQuestionsAnswerAsync();
            foreach (QuestionsAnswerOnline answerlocal in AllanswersLocal)
            {
                if (string.IsNullOrEmpty(answerlocal.answer_id))
                {
                    QuestionsAnswerOnline sendAnswerOnline = await _restService.SendAnswersQuestionAsync(ConstantData.HigalaApi + "sendanswers/", answerlocal);
                    if (sendAnswerOnline != null)
                    {
                        answerlocal.ID = answerlocal.ID;
                        answerlocal.answer_id = sendAnswerOnline.answer_id;
                        var state = await App.Database.SaveQuestionsAnswerAsync(answerlocal);
                        Debug.WriteLine("\tSUCCESS {0}", "Success Upload Answers" + state);
                    }
                    else
                    {
                        Debug.WriteLine("\tFAILED {0}", "Failed to Upload Answers data");
                    }
                }
                else
                {
                    Debug.WriteLine("\tINFOR {0}", "Skip  Upload Answers  data already sync");
                }
            }

            return AllanswersLocal;
        }

        public async Task<List<QuestionsAnswerOnline>> DownloadAnswers(string question_form_id)
        {
            //Retrieve Answers data online
            List<QuestionsAnswerOnline> answers = await _restService.GetAnswersQuestionAsync(ConstantData.HigalaApi + "getanswers/" + question_form_id);
            foreach (QuestionsAnswerOnline answer in answers)
            {
                Debug.WriteLine("\tSUCCESS {0}", "Able to get answers data online");
                List<QuestionsAnswerOnline> answerLocal = await App.Database.GetQuestionsAnswerAsync(answer.question_form_id);
                if (answerLocal == null)
                {
                    await App.Database.SaveQuestionsAnswerAsync(answer);
                    Debug.WriteLine("\tSUCCESS {0}", "Save Local answers Data");
                }
            }

            return answers;
        }

        //Download Establishment
        public async Task<List<EstablishmentOnline>> DowloadEstablishments()
        {
            List<EstablishmentOnline> establishments = await _restService.GetAllEstablishmentAsync(ConstantData.HigalaApi + "getallestablishment");
            if (establishments != null)
            {

                foreach (EstablishmentOnline establishment in establishments)
                {
                    EstablishmentOnline localEstablishment = await App.Database.GetEstablismentAsync(establishment.QrCombination);
                    if (localEstablishment != null)
                    {
                        establishment.ID = localEstablishment.ID;
                        await App.Database.SaveEstablishmentAsync(establishment);
                        Debug.WriteLine("\tINFO {0}", "Update local Establishment" + establishment.establishment_id);
                    }
                    else
                    {
                        await App.Database.SaveEstablishmentAsync(establishment);
                        Debug.WriteLine("\tINFO {0}", "Insert local Establishment");
                    }
                }
            }
            else
            {
                Debug.WriteLine("\tERROR {0}", "Connot get any data in Establishment");
            }

            return establishments;
        }

        //Download Answer History
        public async Task<List<QuestionFormOnline>> DowloadQuestionHistory()
        {

            List<QuestionFormOnline> qrhistoryitems = await _restService.GetAllQuestionHistoryAsync(ConstantData.HigalaApi + "getallquestions/" + App.UserID);
            if (qrhistoryitems != null)
            {

                foreach (QuestionFormOnline qrhistoryitem in qrhistoryitems)
                {
                    EstablishmentOnline establisment = await App.Database.GetEstablismentByIDAsync(qrhistoryitem.establishment_id);
                    if (establisment != null)
                    {
                        QuestionFormOnline questionlocal = await App.Database.GetQuestionsByIDAsync(qrhistoryitem.question_form_id);
                        if (questionlocal != null)
                        {
                            qrhistoryitem.ID = questionlocal.ID;
                            qrhistoryitem.establishment_name = establisment.establishment_name;
                            qrhistoryitem.is_sync = 1;
                            qrhistoryitem.history_date = DateTime.Parse(qrhistoryitem.answer_date + " " + qrhistoryitem.answer_time);
                            Debug.WriteLine("\tLOCAL UPDATE START {0}", await App.Database.SaveScannedItemsAsync(qrhistoryitem));
                        }
                        else
                        {
                            qrhistoryitem.establishment_name = establisment.establishment_name;
                            qrhistoryitem.is_sync = 1;
                            qrhistoryitem.history_date = DateTime.Parse(qrhistoryitem.answer_date + " " + qrhistoryitem.answer_time);
                            await App.Database.SaveScannedItemsAsync(qrhistoryitem);
                            Debug.WriteLine("\tLOCAL INSERT START  {0}", "INSERT Local Questions");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("\tUpdate {0}", "Local QUestion History Establishment not found! " + qrhistoryitem.establishment_id);
                    }
                }

            }
            else
            {
                Debug.WriteLine("\tINFO {0}", "Connot get any data in questions");
            }
            return qrhistoryitems;
        }

        //Upload History
        public async Task<List<QuestionFormOnline>> UploadQuestionHistory()
        {
            List<QuestionFormOnline> questionsHistory = await App.Database.GetQuestionsAsync(App.UserID);
            foreach (QuestionFormOnline question in questionsHistory)
            {
                if (question.is_sync == 0)
                {
                    QuestionFormOnline result = await _restService.SyncQuestionHistoryAsync(ConstantData.HigalaApi + "syncquestions", question);
                    if (result != null)
                    {
                        QuestionFormOnline questionlocal = await App.Database.GetQuestionsByIDAsync(question.question_form_id);
                        if (questionlocal != null)
                        {
                            Debug.WriteLine("\tSYNCING.. {0}", "Updating local Question histtory");
                            question.ID = questionlocal.ID;
                            question.is_sync = 1;
                            Debug.WriteLine("\tSYNCING.. {0}", await App.Database.SaveScannedItemsAsync(question));
                        }

                        //download Answers
                        await DownloadAnswers(question.question_form_id);

                    }
                    else
                    {
                        Debug.WriteLine("\tSYNCING.. {0}", "No RESULT FROM REMOTE Question history");
                    }
                }
                else
                {
                    Debug.WriteLine("\tSYNCING.. {0}", "Skip Already Sync Question => " + question.question_form_id);
                }


            }

            return questionsHistory;
        }

        public string GenerateRequestUri(string endpoint, string username, string password)
        {
            string requestUri = endpoint;
            requestUri += $"?account={username}";
            requestUri += $"&password={password}"; // or units=metric
            return requestUri;
        }
    }
}
