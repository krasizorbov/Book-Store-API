namespace BookStore.API.DTOs
{
    using System.ComponentModel.DataAnnotations;
    public class UserDTO
    {
        private const string passwordError = "";

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Your password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
