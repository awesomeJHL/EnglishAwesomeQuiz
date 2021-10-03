using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishAwesomeQuiz.Helpers
{
    public static class JsonHelper
    {
        public static T GetJsonFileToModel<T>(string filePath)
        {
            StreamReader r = new StreamReader(filePath);
            string jsonString = r.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
