using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        //Đại diện cho middleware tiếp theo trong pipeline
        private readonly RequestDelegate _next;
        //Logger được sử dụng để ghi log
        private readonly ILogger<ExceptionMiddleware> _logger;
        //Đối tượng môi trường kiểm tra xem ứng dụng có chạy trong môi trường phát triển không
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                //Ghi log lỗi
                _logger.LogError(ex, ex.Message);

                //Cấu hình response khi có lỗi
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                //Tạo đối tượng APIException để trả về thông tin lỗi
                var response = _env.IsDevelopment()
                ? new APIException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new APIException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                //Cấu hình options : Tên trong json là kiểu camelCase
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                //Chuyển đối tượng APIException thành Json
                var json = JsonSerializer.Serialize(response, options);

                //Gửi JSON về client
                await context.Response.WriteAsync(json);
            }
        }
    }
}