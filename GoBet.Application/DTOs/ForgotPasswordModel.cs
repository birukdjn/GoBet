using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoBet.Application.DTOs
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage ="email Address is required")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;
    }
}
