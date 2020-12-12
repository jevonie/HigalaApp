using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace HigalaApp.Models
{

    public class ClientLoginOnline
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("customer_username")]
        public string customer_username { get; set; }

        [JsonProperty("Passwords")]
        public string Passwords { get; set; }

        [JsonProperty("is_active")]
        public string is_active { get; set; }
    }
    public class CustomerDataOnline
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("QrCombination")]
        public string QrCombination { get; set; }

        [JsonProperty("customer_firstname")]
        public string customer_firstname { get; set; }

        [JsonProperty("customer_middlename")]
        public string customer_middlename { get; set; }

        [JsonProperty("customer_lastname")]
        public string customer_lastname { get; set; }

        [JsonProperty("customer_extension")]
        public string customer_extension { get; set; }

        [JsonProperty("customer_contact")]
        public string customer_contact { get; set; }

        [JsonProperty("customer_email")]
        public string customer_email { get; set; }

    }

    public class EstablishmentOnline
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty("establishment_id")]
        public string establishment_id { get; set; }

        [JsonProperty("QrCombination")]
        public string QrCombination { get; set; }

        [JsonProperty("establishment_name")]
        public string establishment_name { get; set; }

        [JsonProperty("category_id")]
        public string category_id { get; set; }

        [JsonProperty("has_questions")]
        public string has_questions { get; set; }

        [JsonProperty("is_active")]
        public string is_active { get; set; }
    }

    public class QuestionFormOnline
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty("question_form_id")]
        public string question_form_id { get; set; }

        [JsonProperty("customer_id")]
        public string customer_id { get; set; }

        [JsonProperty("establishment_id")]
        public string establishment_id { get; set; }

        [JsonProperty("establishment_name")]
        public string establishment_name { get; set; }

        [JsonProperty("customer_name")]
        public string customer_name { get; set; }

        [JsonProperty("answer_date")]
        public string answer_date { get; set; }

        [JsonProperty("answer_time")]
        public string answer_time { get; set; }

        [JsonProperty("entity")]
        public string entity { get; set; }

        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }
        public DateTime history_date { get; set; }
        public int is_sync { get; set; }

    }

    public class QuestionsReferenceOnline
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty("question_id")]
        public int question_id { get; set; }

        [JsonProperty("question_order")]
        public int question_order { get; set; }

        [JsonProperty("question")]
        public string question { get; set; }

        [JsonProperty("type_answer")]
        public int type_answer { get; set; }

        public string templateLayout { get; set; }

        [JsonProperty("options")]
        public string options { get; set; }

        [JsonProperty("is_active")]
        public int is_active { get; set; }

    }

    public class QuestionsAnswerOnline
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonProperty("answer_id")]
        public string answer_id { get; set; }

        [JsonProperty("question_form_id")]
        public string question_form_id { get; set; }

        [JsonProperty("question_id")]
        public int question_id { get; set; }

        [JsonProperty("question_answer")]
        public string question_answer { get; set; }

        [JsonProperty("question_text")]
        public string question_text { get; set; }

        public int type_answer { get; set; }
        public string options { get; set; }
        public string question { get; set; }

    }

    public class QuestionsAnswerJoin
    {

        public int question_id { get; set; }

        public string question { get; set; }

        public string question_form_id { get; set; }

        public string question_order { get; set; }

        public string type_answer { get; set; }

        public string question_answer { get; set; }

        public string question_text { get; set; }


    }
    public class AppVersionOnline
    {
        [JsonProperty("version_id")]
        public int version_id { get; set; }

        [JsonProperty("version_no")]
        public string version_no { get; set; }
    }
}


