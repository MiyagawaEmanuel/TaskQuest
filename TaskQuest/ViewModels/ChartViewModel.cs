using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.ViewModels
{
    public class ChartViewModel
    {

        public ChartViewModel()
        {
            Data = new List<int>();
            Labels = new List<string>();
            Offset = new List<int>();
        }

        public string Response { get; set; }

        public string Title { get; set; }

        public List<int> Data { get; set; }

        public List<string> Labels { get; set; }

        public List<int> Offset { get; set; }

        public string Month { get; set; }

        public int DaysInMonth { get; set; }

    }
}