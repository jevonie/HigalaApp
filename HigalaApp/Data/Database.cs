using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using HigalaApp.Models;
using System.Threading.Tasks;
using System.Security.Cryptography;
using HigalaApp.Utility;
using System.Diagnostics;

namespace HigalaApp.Data
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Users>().Wait();
            _database.CreateTableAsync<CustomerDataOnline>().Wait();
            _database.CreateTableAsync<ClientLoginOnline>().Wait();
            _database.CreateTableAsync<ScanHistory>().Wait();
            _database.CreateTableAsync<UserDataLogin>().Wait();
            _database.CreateTableAsync<QuestionFormOnline>().Wait();
            _database.CreateTableAsync<EstablishmentOnline>().Wait();
            _database.CreateTableAsync<QuestionsReferenceOnline>().Wait();
            _database.CreateTableAsync<QuestionsAnswerOnline>().Wait();
            
        }


        public Task<List<Users>> GetUserAsync()
        {
            return _database.Table<Users>().ToListAsync();
        }

        public Task<Users> GetUserByIDAsync(int id)
        {
            return _database.Table<Users>()
                           .Where(i => i.ID == id)
                           .FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(Users user)
        {

            if (user.ID != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }

        public Task<List<ScanHistory>> GetScanHistoryAsync()
        {
            return _database.Table<ScanHistory>().ToListAsync();
        }
        public Task<int> SaveScanQRAsync(ScanHistory qr)
        {

            if (qr.ID != 0)
            {
                return _database.UpdateAsync(qr);
            }
            else
            {
                return _database.InsertAsync(qr);
            }
        }

        // CLIENT DATA //

        public Task<List<CustomerDataOnline>> GetCustomerAsync()
        {
            return _database.Table<CustomerDataOnline>().ToListAsync();
        }

        public Task<CustomerDataOnline> GetCustomerByIDAsync(string customerid)
        {

            return _database.Table<CustomerDataOnline>()
                           .Where(i => i.customer_id == customerid)
                           .FirstOrDefaultAsync();
        }
        public Task<int> SaveCustomerAsync(CustomerDataOnline customer)
        {

            if (customer.ID != 0)
            {
                return _database.UpdateAsync(customer);
            }
            else
            {
                return _database.InsertAsync(customer);
            }
        }

        //LOGIN block
        public Task<int> SaveClientLoginAsync(ClientLoginOnline client)
        {
            if (client.ID != 0)
            {
                return _database.UpdateAsync(client);
            }
            else
            {
                return _database.InsertAsync(client);
            }
        }

        public Task<ClientLoginOnline> ClientLoginAsync(string username)
        {
            return _database.Table<ClientLoginOnline>()
                            .Where(x => x.customer_username == username)
                            .FirstOrDefaultAsync();
        }

        public Task<ClientLoginOnline> GetLoginAsync(string username)
        {
            return _database.Table<ClientLoginOnline>()
                            .Where(x => x.customer_username == username)
                            .FirstOrDefaultAsync();
        }
        public Task<ClientLoginOnline> GetUserByCustomerIDAsync(string customer_id)
        {
            return _database.Table<ClientLoginOnline>()
                            .Where(x => x.customer_id == customer_id)
                            .FirstOrDefaultAsync();
        }
        // QUESTIONS FORMS //

        public Task<List<QuestionFormOnline>> GetQuestionsAsync(string customerid)
        {
            DateTime last30days = DateTime.Now.AddDays(-30);

            return _database.Table<QuestionFormOnline>()
                            .Where(i => i.customer_id == customerid && i.created_at >= last30days)
                            .OrderByDescending(i => i.created_at)
                            .ToListAsync();
        }
        public Task<QuestionFormOnline> GetQuestionsByIDAsync(string id)
        {
            return _database.Table<QuestionFormOnline>()

                            .Where(i => i.question_form_id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<List<QuestionFormOnline>> GetQuestionsNotSyncAsync()
        {
            return _database.Table<QuestionFormOnline>()
                            .Where(i => i.customer_id == App.UserID && i.is_sync == 0)
                            .ToListAsync();
        }

        public Task<QuestionFormOnline> GetQuestionsFormByIDAsync(string question_form_id)
        {
            return _database.Table<QuestionFormOnline>()
                            .Where(i => i.question_form_id == question_form_id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveScannedItemsAsync(QuestionFormOnline question)
        {

            if (question.ID != 0)
            {
                return _database.UpdateAsync(question);
            }
            else
            {
                return _database.InsertAsync(question);
            }

        }
            

        public Task<int> DeleteQuestionsFormByIDAsync(string question_form_id)
        {
            return _database.Table<QuestionFormOnline>()
                            .Where(i => i.question_form_id == question_form_id)
                            .DeleteAsync();
        }

        //Establishment

        public Task<int> SaveEstablishmentAsync(EstablishmentOnline establishment)
        {

            if (establishment.ID != 0)
            {
                return _database.UpdateAsync(establishment);
            }
            else
            {
                return _database.InsertAsync(establishment);
            }
        }

        public Task<EstablishmentOnline> GetEstablismentAsync(string result)
        {

            return _database.Table<EstablishmentOnline>()
                            .Where(i => i.QrCombination ==  result)
                            .FirstOrDefaultAsync();
        }

        public Task<List<EstablishmentOnline>> GetEstablismentSearchAsync(string result)
        {
            return  _database.QueryAsync<EstablishmentOnline>($"SELECT * FROM  EstablishmentOnline WHERE QrCombination LIKE '%{ result }%' LIMIT 1");
        }
        
        public Task<EstablishmentOnline> GetEstablismentByIDAsync(string establishment_id)
        {
            return _database.Table<EstablishmentOnline>()
                            .Where(i => i.establishment_id == establishment_id)
                            .FirstOrDefaultAsync();
        }

        //Questions Reference

        public Task<int> SaveQuestionsReferenceAsync(QuestionsReferenceOnline questions)
        {

            if (questions.ID != 0)
            {
                return _database.UpdateAsync(questions);
            }
            else
            {
                return _database.InsertAsync(questions);
            }
        }

        public Task<QuestionsReferenceOnline> GetQuestionsReferenceAsync(int question_id)
        {

            return _database.Table<QuestionsReferenceOnline>()
                            .Where(i => i.question_id == question_id)
                            .FirstOrDefaultAsync();
        }

        public Task<List<QuestionsReferenceOnline>> GetQuestionsAllReferenceAsync()
        {

            return _database.Table<QuestionsReferenceOnline>().ToListAsync();
        }
        //Questions Answers

        public Task<int> SaveQuestionsAnswerAsync(QuestionsAnswerOnline answer)
        {

            if (answer.ID != 0)
            {
                return _database.UpdateAsync(answer);
            }
            else
            {
                return _database.InsertAsync(answer);
            }
        }

        public Task<List<QuestionsAnswerOnline>> GetQuestionsAnswerAsync(string question_form_id)
        {

            return _database.Table<QuestionsAnswerOnline>()
                            .Where(i => i.question_form_id == question_form_id)
                            .ToListAsync();
        }
        public Task<List<QuestionsAnswerOnline>> GetAllQuestionsAnswerAsync()
        {
            return _database.Table<QuestionsAnswerOnline>().ToListAsync();
        }

        public Task<int> DeleteQuestionsAnswerByIDAsync(string question_form_id)
        {
            return _database.Table<QuestionsAnswerOnline>()
                            .Where(i => i.question_form_id == question_form_id)
                            .DeleteAsync();
        }

        //Question and answer

        public Task<List<QuestionsAnswerJoin>> GetJoinAnswerQuestionsAsync(string question_form_id)
        {
            var q = _database.QueryAsync<QuestionsAnswerJoin>(
                     @"select QAO.question_id, QAO.question_answer, 
                           QAO.question_text,QRO.question,QRO.type_answer,QRO.options from QuestionsAnswerOnline QAO inner join QuestionsReferenceOnline QRO on 
                           QAO.question_id = QRO.id where QAO.question_form_id = ?",
                    question_form_id);
            
            return q;
        }
        public Task<List<QuestionsAnswerJoin>> GetJoinAnswerAllQuestionsAsync()
        {
            var q = _database.QueryAsync<QuestionsAnswerJoin>(
                     @"select QAO.question_id, QAO.question_answer, 
                           QAO.question_text,QRO.question,QRO.type_answer,QRO.options from QuestionsReferenceOnline QRO left join QuestionsAnswerOnline QAO on 
                           QAO.question_id = QRO.id");

            return q;
        }

    }
}
