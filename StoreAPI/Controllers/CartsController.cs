using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController(StoreContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetCarts()
    {
        return await context.Carts
            .Include(c => c.Products)
            .Include(c => c.User)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetCart(int id)
    {
        var cart = await context.Carts
            .Include(c => c.Products)
            .Include(c => c.User)
            .Where(c => c.CartID == id)
            .FirstOrDefaultAsync();

        if (cart == null) return NotFound();

        return cart;
    }

    [HttpPost]
    public async Task<ActionResult<Cart>> PostCart(Cart cart)
    {
        context.Carts.Add(cart);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCart), new { id = cart.CartID }, cart);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCart(int id, Cart cart)
    {
        if (id != cart.CartID)
        {
            return BadRequest();
        }

        context.Entry(cart).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CartExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart(int id)
    {
        var cart = await context.Carts.FindAsync(id);
        if (cart == null) return NotFound();

        context.Carts.Remove(cart);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool CartExists(int id)
    {
        return context.Carts.Any(c => c.CartID == id);
    }
}