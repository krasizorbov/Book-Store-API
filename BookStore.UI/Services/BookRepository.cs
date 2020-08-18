namespace BookStore.UI.Services
{
    using Blazored.LocalStorage;
    using BookStore.UI.Contracts;
    using BookStore.UI.Models;
    using System.Net.Http;
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IHttpClientFactory client;
        private readonly ILocalStorageService localStorage;

        public BookRepository(IHttpClientFactory client,
            ILocalStorageService localStorage) : base(client, localStorage)
        {
            this.client = client;
            this.localStorage = localStorage;
        }
    }
}
