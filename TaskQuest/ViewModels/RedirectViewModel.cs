using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TaskQuest.ViewModels
{
    public class RedirectViewModel
    {

        public RedirectViewModel() { }

        public RedirectViewModel(string Path,  string Value)
        {
            this.Path = Path;
            this.Value = Value;
        }

        public string Path { get; set; }

        public string Value { get; set; }
    }
}
