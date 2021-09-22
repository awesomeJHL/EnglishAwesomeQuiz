using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class ScoreModel
    {
        public string UserName { get; set; }
        public int BankId { get; set; }
        public double Score { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
