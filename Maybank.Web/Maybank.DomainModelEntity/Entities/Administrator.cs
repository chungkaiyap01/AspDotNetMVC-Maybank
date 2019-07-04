using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maybank.DomainModelEntity.Entities
{
    public partial class Administrator
    {
        [Key]
        [Display(Name = "Administrator ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter your login username.")]
        [StringLength(100, MinimumLength = 8)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your login password.")]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your fullname.")]
        public string Fullname { get; set; }
    }
}
