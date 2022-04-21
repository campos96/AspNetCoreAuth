using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Account
    {
        public Guid? ID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
