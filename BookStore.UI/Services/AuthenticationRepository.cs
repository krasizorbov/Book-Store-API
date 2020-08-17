namespace BookStore.UI.Services
{
    using Blazored.LocalStorage;
    using BookStore.UI.Contracts;
    using BookStore.UI.Models;
    using BookStore.UI.Providers;
    using BookStore.UI.Static;
    using Microsoft.AspNetCore.Components.Authorization;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory client;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authStateProvider;
        public AuthenticationRepository(IHttpClientFactory client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            this.client = client;
            this.localStorage = localStorage;
            this.authStateProvider = authStateProvider;
        }

        public async Task<bool> Login(LoginModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.LoginEndpoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var client = this.client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);
            // Store the token
            await localStorage.SetItemAsync("authToken", token.Token);

            // Chane the state of the app
            await ((ApiAuthenticationStateProvider)authStateProvider).LoggedIn();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.Token);
            return true;
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)authStateProvider).LoggedOut();
        }

        public async Task<bool> Register(RegistrationModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, Endpoints.RegisterEndpoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var client = this.client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
    }
}
