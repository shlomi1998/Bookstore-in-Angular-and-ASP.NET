using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace book_store_be.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        public bool IsAdmin { get; set; } = false;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(4, ErrorMessage = "Username must be at least 4 characters long.")]
        public string UserName { get; set; }

        public List<BookModel> Cart { get; set; } = new List<BookModel>();
    }
}
