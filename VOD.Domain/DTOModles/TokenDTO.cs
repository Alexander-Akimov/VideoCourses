using System;
using System.Collections.Generic;
using System.Text;

namespace VOD.Domain.DTOModles
{
    public class TokenDTO
    {
        public string Token { get; set; }

        public DateTime TokenExpires { get; set; } = default;

        public bool TokenHasExpired
        {
            get => TokenExpires == default ? true :
                !(TokenExpires.Subtract(DateTime.UtcNow).Minutes > 0);
        }
        public TokenDTO()
        {

        }

        public TokenDTO(string token, DateTime expires)
        {
            Token = token;
            TokenExpires = expires;
        }
    }
}
