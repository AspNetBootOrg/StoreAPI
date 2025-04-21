using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(StoreContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetProducts()
    {
        return await context.Orders
            .Include(o => o.User)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetOrder(int id)
    {
        var order = await context.Orders
            .Include(o => o.User)
            .Where(o => o.OrderID == id)
            .FirstOrDefaultAsync();

        if (order == null) return NotFound();

        return order;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostOrder(Order order)
    {
        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.OrderID }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrder(int id, Order order)
    {
        if (id != order.OrderID)
        {
            return BadRequest();
        }

        context.Entry(order).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        context.Orders.Remove(order);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderExists(int id)
    {
        return context.Orders.Any(o => o.OrderID == id);
    }
}