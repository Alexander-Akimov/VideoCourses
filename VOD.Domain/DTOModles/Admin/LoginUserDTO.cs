using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Domain.DTOModles.Admin
{
    public class LoginUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
    }
}
