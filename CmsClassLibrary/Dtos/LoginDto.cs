using System;
using System.Collections.Generic;
using System.Text;

namespace CmsClassLibrary.Dtos
{
    public class LoginDto
    {
        public string EmpName { get; set; }
        public string EmpEmail { get; set; }
        public string Token { get; set; }
    }
}
