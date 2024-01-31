using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    //Tạo token JWT
    public class TokenService : ITokenService
    {
        //Tạo chữ ký JWT
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            //Đọc giá trị khóa tokenkey và khởi tạo cho key
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            //Lưu thông tin có userName 
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };
            //Định nghĩa chữ ký 
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            //Thông tin về token cần tạo, thời gian hết hạn, chữ ký
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}