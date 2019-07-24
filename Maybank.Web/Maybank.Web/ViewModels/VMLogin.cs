using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Maybank.Web.ViewModels
{
    public class VMLogin
    {
        [Required(ErrorMessage = "Please enter your login username.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your login password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}