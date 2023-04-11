using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Contracts;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private const string AdminRole = "Admin";
    private readonly IRepository<Item> itemsRepository;
    private readonly IPublishEndpoint publishEndpoint;

    public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
    {
        this.itemsRepository = itemsRepository;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    [Authorize(Policies.Read)]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
    {
        var items = (await itemsRepository.GetAllAsync())
                         .Select(i => i.AsDto());

        return Ok(items);
    }

    [HttpGet("{id}")]
    [Authorize(Policies.Read)]
    public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);

        if (item is null) return NotFound();

        return Ok(item.AsDto());
    }

    [HttpPost]
    [Authorize(Policies.Write)]
    public async Task<ActionResult<ItemDto>> CreateAsync(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await itemsRepository.CreateAsync(item);

        //announce item has been created
        await publishEndpoint.Publish(new CatalogItemCreated(item.Id, item.Name, item.Description, item.Price));

        return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    [Authorize(Policies.Write)]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateItemDto updateItemDto)
    {
        var existingItem = await itemsRepository.GetAsync(id);

        if (existingItem is null) return NotFound();

        existingItem.Name = updateItemDto.Name;
        existingItem.Description = updateItemDto.Description;
        existingItem.Price = updateItemDto.Price;

        await itemsRepository.UpdateAsync(existingItem);

        //announce item has been update
        await publishEndpoint.Publish(new CatalogItemUpdated(existingItem.Id, existingItem.Name, existingItem.Description, existingItem.Price));

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policies.Write)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var item = await itemsRepository.GetAsync(id);

        if (item is null) return NotFound();

        await itemsRepository.RemoveAsync(item.Id);

        //announce item had been deleted
        await publishEndpoint.Publish(new CatalogItemDeleted(id));

        return NoContent();
    }
}
