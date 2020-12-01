using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HigalaApp.Models;
namespace HigalaApp.Services
{
    class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<ClientLoginOnline> GetClientDataAsync(string uri)
        {
            ClientLoginOnline clientLogin = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    clientLogin = JsonConvert.DeserializeObject<ClientLoginOnline>(content);
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return clientLogin;
        }

        //UsersData
        public async Task<CustomerDataOnline> GetClientDataDetailsAsync(string uri)
        {
            CustomerDataOnline clientdata = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    clientdata = JsonConvert.DeserializeObject<CustomerDataOnline>(content);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return clientdata;
        }
        //establishment
        public async Task<EstablishmentOnline> GetEstablishmentAsync(string uri)
        {
            EstablishmentOnline establishment = null;
            Debug.WriteLine("\tURL  {0}", uri);
            try
            {
                Debug.WriteLine("\tTIBONG {0}", "Calling remote");
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    establishment = JsonConvert.DeserializeObject<EstablishmentOnline>(content);
                    Debug.WriteLine("\tRETURN CONTENT {0}", content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return establishment;
        }
        public async Task<List<EstablishmentOnline>> GetAllEstablishmentAsync(string uri)
        {
            List<EstablishmentOnline> establishment = null;
            try
            {
                Debug.WriteLine("\tTIBONG {0}", "Calling remote");
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    establishment = JsonConvert.DeserializeObject<List<EstablishmentOnline>>(content);
                }
                Debug.WriteLine("\tTIBONG {0}", "failed remote");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return establishment;
        }

        public async Task<List<QuestionFormOnline>> GetAllQuestionHistoryAsync(string uri)
        {
            List<QuestionFormOnline> establishment = null;
            try
            {
                Debug.WriteLine("\tTIBONG {0}", "Calling remote");
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    establishment = JsonConvert.DeserializeObject<List<QuestionFormOnline>>(content);
                }
                Debug.WriteLine("\tTIBONG {0}", "failed remote");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return establishment;
        }

        public async Task<QuestionFormOnline> SyncQuestionHistoryAsync(string uri, QuestionFormOnline question)
        {
            QuestionFormOnline questions = null;

            string requestUri = uri;
            requestUri += $"?question_form_id={question.question_form_id}";
            requestUri += $"&customer_id={question.customer_id}";
            requestUri += $"&customer_name={System.Web.HttpUtility.UrlEncode(question.customer_name)}";
            requestUri += $"&establishment_id={question.establishment_id}";
            requestUri += $"&entity={question.entity}";
            requestUri += $"&answer_date={question.answer_date}";
            requestUri += $"&answer_time={question.answer_time}";
            Debug.WriteLine("\tURL  {0}", requestUri);
            try
            {
                Debug.WriteLine("\tTIBONG {0}", "Calling remote");
                HttpResponseMessage response = await _client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    questions = JsonConvert.DeserializeObject<QuestionFormOnline>(content);
                    Debug.WriteLine("\tRETUN QUESTIONS {0}", questions);
                }
                else
                {
                    Debug.WriteLine("\tTIBONG {0}", "failed sync questions");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return questions;
        }
        public async Task<List<QuestionsReferenceOnline>> GetAllReferenceQuestionAsync(string uri)
        {
            List<QuestionsReferenceOnline> establishment = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATASYNC {0}", content);
                    establishment = JsonConvert.DeserializeObject<List<QuestionsReferenceOnline>>(content);
                }
                Debug.WriteLine("\tINFO {0}", "failed remote");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tINFO {0}", ex.Message);
            }

            return establishment;
        }

        public async Task<List<QuestionsAnswerOnline>> GetAnswersQuestionAsync(string uri)
        {
            List<QuestionsAnswerOnline> establishment = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("\tDATA {0}", content);
                    establishment = JsonConvert.DeserializeObject<List<QuestionsAnswerOnline>>(content);
                }
                Debug.WriteLine("\tINFO {0}", "failed remote");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tINFO {0}", ex.Message);
            }

            return establishment;
        }

        public async Task<QuestionsAnswerOnline> SendAnswersQuestionAsync(string uri, QuestionsAnswerOnline answer)
        {
            QuestionsAnswerOnline questionsFromOnline = null;

            string requestUri = uri;
            requestUri += $"?question_form_id={answer.question_form_id}";
            requestUri += $"&question_id={answer.question_id}";
            requestUri += $"&question_answer={System.Web.HttpUtility.UrlEncode(answer.question_answer)}";
            requestUri += $"&question_text={System.Web.HttpUtility.UrlEncode(answer.question_text)}";
            Debug.WriteLine("\tURL  {0}", requestUri);
            try
            {
                HttpResponseMessage response = await _client.GetAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    questionsFromOnline = JsonConvert.DeserializeObject<QuestionsAnswerOnline>(content);
                }
                else
                {
                    Debug.WriteLine("\tTIBONG {0}", "failed sync questions");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return questionsFromOnline;
        }

    }
}
