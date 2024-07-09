using System.ComponentModel.DataAnnotations;

namespace book_store_be.Models
{
    public class BookModel
    {
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }


        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

       


        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public double Discount { get; set; }



        public string? ImageUrl { get; set; }


        [Required(ErrorMessage = "Summary is required.")]
        public string Summary { get; set; }
    }




}
