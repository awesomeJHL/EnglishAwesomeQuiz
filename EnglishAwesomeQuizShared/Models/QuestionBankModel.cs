using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishAwesomeQuizShared.Models
{
    public class QuestionBankModel
    {
        public int BankId { get; set; }
        public List<QuestionModel> Bank { get; set; }
    }
}
