using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HigalaApp.Models
{
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CustomerID { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerMiddleName { get; set; }

        public string CustomerLastName { get; set; }

        public string CustomerContact { get; set; }

        public string CustomerEmail { get; set; }
    }

    public class UserDataLogin
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string CustomerID { get; set; }

        public string CustomerUsername { get; set; }

        public string Passwords { get; set; }

        public string IsAcitve { get; set; }
    }
}