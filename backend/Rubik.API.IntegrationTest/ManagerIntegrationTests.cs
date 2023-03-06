using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Rubik.API.Models;

namespace Rubik.API.IntegrationTest
{
    public class ManagerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient client;

        public ManagerIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        [Fact]
        public async void GetRanking()
        {
            var response = await client.GetAsync("/Rankings");

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async void UserRegistrationSuccessful()
        {
            const string username = "TestUser";
            const string password = "Password1";
            
            var registrationRequest = new RegisterRequest
            {
                Email = "a@gmail.com",
                Password = password,
                Username = username,
            };
            var registrationResponse = await client.PostAsJsonAsync("users/register", registrationRequest);
            
            Assert.True(registrationResponse.IsSuccessStatusCode, registrationResponse.Content.ReadAsStringAsync().Result);

            var authenticateRequest = new AuthenticateRequest { Username = username, Password = password };
            var authenticateResponse = await client.PostAsJsonAsync("users/auth", authenticateRequest);
            
            Assert.True(authenticateResponse.IsSuccessStatusCode, authenticateResponse.Content.ReadAsStringAsync().Result);
        }
    }
}
