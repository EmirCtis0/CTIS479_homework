using CORE.APP.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public enum Genders
    {
        Unknown,
        Male,
        Female,
        Other
    }

    public class User : Entity
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(100)]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public decimal Score { get; set; }

        public bool IsActive { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
    }
}