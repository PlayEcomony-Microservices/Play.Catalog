using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Common;
using Play.Common.MongoDB;

namespace Play.Catalogo.Test.Data
{
    public class ItemsRepositoryTest
    {
        private readonly WebApplicationFactory<Program> app;

        public ItemsRepositoryTest()
        {
            app = new WebApplicationFactory<Program>()
                        .WithWebHostBuilder(builder => 
                        {
                            builder.ConfigureServices(services =>
                            {
                                services.AddMongo().AddMongoRepository<Item>("items");
                            });
                        });
        }

        [Fact]
        public async Task GetAllItemsAsync()
        {
            using (var scope = app.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var itemsRepository = scopedServices.GetRequiredService<IRepository<Item>>();

                var items = await itemsRepository.GetAllAsync();

                var item = items.FirstOrDefault();

                Assert.NotNull(items);
                Assert.Equal("Potion", item?.Name);
            }
        }
    }
}