using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HigalaApp.Models
{
    public class ScanHistory
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Transaction { get; set; }
        public int CleintID { get; set; }
        public DateTime Date { get; set; }
    }
}
