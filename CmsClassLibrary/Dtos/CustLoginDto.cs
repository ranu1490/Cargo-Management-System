using System;
using System.Collections.Generic;
using System.Text;

namespace CmsClassLibrary.Dtos
{
    public class CustLoginDto
    {
        public int CustId { get; set; }
        public string CustName { get; set; }
        public string CustEmail { get; set; }
        public string Token { get; set; }
    }
}
