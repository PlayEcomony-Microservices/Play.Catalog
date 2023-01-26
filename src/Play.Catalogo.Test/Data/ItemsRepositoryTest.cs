using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

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
                                services.AddSingleton(serviceProvider =>
                                {
                                    var mongoClient = new MongoClient("mongodb://localhost:27017");
                                    return mongoClient.GetDatabase("Catalog");
                                });
                                services.AddSingleton<IRepository<Item>>(serviceProvider =>
                                {
                                    var database = serviceProvider.GetService<IMongoDatabase>();
                                    return new MongoRepository<Item>(database, "items");
                                });
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