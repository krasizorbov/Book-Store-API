namespace BookStore.API.DTOs
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AuthorDTO
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }
        public virtual IList<BookDTO> Books { get; set; }
    }
}
