using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private static readonly List<ItemDto> items = new()
    {
        new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 7, DateTimeOffset.UtcNow),
        new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of Damage", 20, DateTimeOffset.UtcNow)
    };

    [HttpGet]
    public ActionResult<IEnumerable<ItemDto>> Get()
    {
        return items;
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetById(Guid id)
    {
        var item = items.SingleOrDefault(i => i.Id == id);

        if(item is null) return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public ActionResult<ItemDto> Create(CreateItemDto itemDto)
    {
        var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);

        items.Add(item);

        return CreatedAtAction(nameof(GetById), new {id = item.Id}, item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UpdateItemDto updateItemDto)
    {
        var itemToUpdate = items.Where(i => i.Id == id).SingleOrDefault();

        if(itemToUpdate is null) return NotFound();

        var item = itemToUpdate with{
            Name = updateItemDto.Name,
            Description = updateItemDto.Description,
            Price = updateItemDto.Price
        };

        var index = items.FindIndex(i => i.Id == id);
        items[index] = item;

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {      
        var index = items.FindIndex(i => i.Id == id);

        if(index < 0) return NotFound();

        items.RemoveAt(index);

        return NoContent();
    }
}
