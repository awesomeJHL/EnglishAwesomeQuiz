using EnglishAwesomeQuizShared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishAwesomeQuiz.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        [HttpPost("api/hello")]
        public async Task<IActionResult> Hello([FromForm] ExampleModel model)
        {
            return Ok(model);
        }
    }
    

}
