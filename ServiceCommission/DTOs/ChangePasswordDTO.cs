using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.DTOs
{
    public class ChangePasswordDTO
    {
        public string Token { get; set; }
        public string Password { get; set; }
    }
}
