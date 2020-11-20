using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Models
{
    public class User : Entity   {

        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
