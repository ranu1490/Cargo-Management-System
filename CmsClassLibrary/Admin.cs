
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CmsClassLibrary
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DefaultValue("Admin@123")]

        public string Pass { get; set; }

        //private bool ValidatePassword(string pass, out string ErrorMessage)
        //{
        //    var input = pass;
        //    ErrorMessage = string.Empty;

        //    if (string.IsNullOrWhiteSpace(input))
        //    {
        //        throw new Exception("Password should not be empty");
        //    }

        //    var hasNumber = new Regex(@"[0-9]+");
        //    var hasUpperChar = new Regex(@"[A-Z]+");
        //    var hasMiniMaxChars = new Regex(@".{8,15}");
        //    var hasLowerChar = new Regex(@"[a-z]+");
        //    var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

        //    if (!hasLowerChar.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one lower case letter.";
        //        return false;
        //    }
        //    else if (!hasUpperChar.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one upper case letter.";
        //        return false;
        //    }
        //    else if (!hasMiniMaxChars.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should not be lesser than 8 or greater than 15 characters.";
        //        return false;
        //    }
        //    else if (!hasNumber.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one numeric value.";
        //        return false;
        //    }

        //    else if (!hasSymbols.IsMatch(input))
        //    {
        //        ErrorMessage = "Password should contain at least one special case character.";
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
