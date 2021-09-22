using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class DialougeModel
    {
        public string Subject { get; set; }
        public string Source { get; set; }
        public List<Sentence> Dialouge { get; set; }
    }
}
