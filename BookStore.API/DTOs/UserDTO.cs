namespace BookStore.API.DTOs
{
    using System.ComponentModel.DataAnnotations;
    public class UserDTO
    {
        private const int maxPasswordLength = 20;
        private const int minPasswordLength = 6;
        private const string passwordError = "Your password is limited to {1} characters, minimum length {2}";

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(maxPasswordLength, ErrorMessage = passwordError,MinimumLength = minPasswordLength)]
        public string Password { get; set; }
    }
}
