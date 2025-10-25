using APP.Domain;
using CORE.APP.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserResponse : Response
    {
        public int Id { get; set; }

        public string Guid { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}".Trim();

        [Display(Name = "Gender")]
        public Genders Gender { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime? RegistrationDate { get; set; }

        [Display(Name = "Score")]
        public decimal Score { get; set; }

        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }
    }
}