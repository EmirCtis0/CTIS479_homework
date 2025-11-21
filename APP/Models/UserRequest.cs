// APP/Models/UserRequest.cs
using APP.Domain;
using CORE.APP.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace APP.Models
{
    public class UserRequest : Request
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        // [Required] YOK ve string? KULLANILIYOR
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; } // string? KULLANILIYOR

        [StringLength(100)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Gender")]
        public Genders Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [StringLength(500)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Roles")]
        public List<int> RoleIds { get; set; } = new List<int>();

    }
}