using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public AppUser(int id, string userName)
        {
            this.Id = id;
            this.UserName = userName;

        }
        public int Id { get; set; }

        public string UserName { get; set; }
    }
}