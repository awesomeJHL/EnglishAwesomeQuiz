using EnglishAwesomeQuiz.Interfaces;
using EnglishAwesomeQuizShared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishAwesomeQuiz.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpPost("api/list")]
        public async Task<IActionResult> GetQuiz([FromBody] QuizOptionModel param)
        {
            return Ok(_quizService.GetWordQuiz(param));
        }

        [HttpPost("api/sentence/list")]
        public async Task<IActionResult> GetSentenceQuiz([FromBody] QuizOptionModel param)
        {
            //TODO 난이도에 따라서 blank숫자를 설정하도록 하자
            return Ok(_quizService.GetSentenceQuiz(param));
        }
    }
}
