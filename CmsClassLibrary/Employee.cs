using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CmsClassLibrary
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        
        public string? EmpName { get; set; }
        //[Range(1000000000, 9999999999,
        //    ErrorMessage = "Mobile no should be 10 digits")]
        public string EmpPhNo { get; set; }
        [Required]
        [EmailAddress]
        public string EmpEmail { get; set; }

        // public string Gender { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DefaultValue("Empl@123")]
        public string? Pass { get; set; }

    }
}
