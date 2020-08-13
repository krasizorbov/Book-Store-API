namespace BookStore.API.DTOs
{
    using System.ComponentModel.DataAnnotations;
    public class AuthorCreateDTO
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }
    }
}
