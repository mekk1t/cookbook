using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP.Cookbook.Domain.Entities
{
    public class User : Entity
    {
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public DateTime JoinedAt { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
