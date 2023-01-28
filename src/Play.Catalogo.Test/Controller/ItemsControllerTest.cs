using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Play.Catalog.Service.Dtos;
using Xunit;

namespace Play.Catalogo.Test.Controller
{
    public class ItemsControllerTest
    {
        private const string baseApiUrl = "/api/items";
        private readonly HttpClient _httpClient;

        public ItemsControllerTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateDefaultClient();
        }
        
        [Fact]
        public async void Get_AllItems()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ItemDto>>(baseApiUrl);

            Assert.NotNull(response);
            Assert.Collection(response , item => Assert.Contains("Potion", item.Name));
        }
    }
}