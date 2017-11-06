using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.Models
{
    public class Backup
    {
        public int Id { get; set; }

        public string TableName { get; set; }

        public string QueryType { get; set; }

        public string Data { get; set; }
    }
}