using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Dtos.Account
{
    public class RegisterResponse
    {
        public bool HasError { get; set; }
        public string Error { get; set; }

        public int? ErrorCode { get; set; }
    }
}
