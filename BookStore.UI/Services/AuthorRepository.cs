namespace BookStore.UI.Services
{
    using Blazored.LocalStorage;
    using BookStore.UI.Contracts;
    using BookStore.UI.Models;
    using System.Net.Http;
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IHttpClientFactory client;
        private readonly ILocalStorageService localStorage;
        public AuthorRepository(IHttpClientFactory client, ILocalStorageService localStorage) : base(client, localStorage)
        {
            this.client = client;
            this.localStorage = localStorage;
        }
    }
}
