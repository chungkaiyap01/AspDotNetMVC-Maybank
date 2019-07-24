using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Maybank.Web.ViewModels
{
    public class VMRegister
    {
        [Required(ErrorMessage = "Please enter your fullname.")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Please enter your nric.")]
        [RegularExpression("^[0-9]{6}-[0-9]{2}-[0-9]{4}$", ErrorMessage = "NRIC must follow the XXXXXX-XX-XXXX format!")]
        public string NRIC { get; set; }

        [Required(ErrorMessage = "Please enter your login username.")]
        [StringLength(100, MinimumLength = 4)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your login password.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}