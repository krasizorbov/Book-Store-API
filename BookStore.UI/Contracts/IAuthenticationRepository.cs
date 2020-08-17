namespace BookStore.UI.Contracts
{
    using BookStore.UI.Models;
    using System.Threading.Tasks;
    public interface IAuthenticationRepository
    {
        public Task<bool> Register(RegistrationModel user);
        public Task<bool> Login(LoginModel user);
        public Task Logout();
    }
}
