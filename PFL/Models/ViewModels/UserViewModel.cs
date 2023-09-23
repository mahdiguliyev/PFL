using System;
using System.ComponentModel.DataAnnotations;

namespace PFL.Models.ViewModels
{
    public class UserViewModel
    {

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(7)]
        public string Pin { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }


        [Required]
        public bool IsActive { get; set; }
    }
}