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
    public class FileUploadController : ControllerBase
    {
        [HttpPost("api/upload")]
        public async Task<IActionResult> FileUploadExample([FromForm] FileUploadModel model)
        {
            return Ok(new ApiResponseModel("ok!!!"));
        }
    }

    public class FileUploadModel
    {
        public List<IFormFile> Files { get; set; }
        public string PageFrom { get; set; }
        public string PageTo { get; set; }
    }

    public class ApiResponseModel : BaseResponseModel
    {
        private readonly static string successMessage = "정상적으로 처리되었습니다";

        public object Data { get; set; }

        public ApiResponseModel()
        {
            this.Success = "Y";
            this.Message = successMessage;
            this.Data = null;
            this.SentDate = DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss");
        }
        public ApiResponseModel(object payload, string message = "", string success = "Y")
        {
            this.Success = success == "Y" ? "Y" : "N";
            this.Message = message == string.Empty ? successMessage : message;
            this.Data = payload;
            this.SentDate = DateTime.Now.ToString("yyyy-MM-dd hh:MM:ss");
        }
    }

    public class BaseResponseModel
    {
        public string Success { get; set; }

        public string Message { get; set; }
        public string SentDate { get; set; }
    }
}
