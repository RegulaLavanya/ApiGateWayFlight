using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class Users
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role{ get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }



        //[ForeignKey("RoleId")]
        //public Roles Roles { get; set; }
    }
}
