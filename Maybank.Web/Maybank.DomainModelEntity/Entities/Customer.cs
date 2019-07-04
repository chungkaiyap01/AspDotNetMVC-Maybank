using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maybank.DomainModelEntity.Entities
{
    public partial class Customer
    {
        [Key]
        [Display(Name = "Customer ID")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter your login username.")]
        [StringLength(100, MinimumLength = 4)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your login password.")]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your fullname.")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Please enter your nric.")]
        [RegularExpression("^[0-9]{6}-[0-9]{2}-[0-9]{4}$", ErrorMessage = "NRIC must follow the XXXXXX-XX-XXXX format!")]
        public string NRIC { get; set; }

        [Required(ErrorMessage = "Please choose your date of birth.")]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Range(18, 100)]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        public byte[] Photo { get; set; }

        public virtual ICollection<BankAccount> BankAccount { get; set; }
    }
}
