using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobiles_accounting.Model
{
    class ReportFilter
    {
        public string Context { get; set; }

        public string Content { get; set; }

        public delegate bool Filtrator(string content, object value);
        Filtrator filtrator { get; set; }

        public void RegisterHandler(Filtrator lambda)
        {
            filtrator = lambda;
        }

        public bool GetResult(object value)
        {
            if (filtrator != null)
                return filtrator(Content, value);
            return true;
        }
    }
}
