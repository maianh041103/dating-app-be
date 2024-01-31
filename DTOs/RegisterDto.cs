using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        public RegisterDto(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}