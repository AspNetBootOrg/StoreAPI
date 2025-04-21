using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Data;
using StoreAPI.Models;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(StoreContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetUsers()
    {
        return await context.Users
            .Include(u => u.Permissions)
            .Select(u => new
            {
                u.UserID,
                u.Username,
                u.Email,
                Permissions = u.Permissions.Select(p => new
                {
                    p.PermissionID,
                    p.Name
                })
            })
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetUser(int id)
    {
        var user = await context.Users
            .Include(u => u.Permissions)
            .Where(u => u.UserID == id)
            .Select(u => new
            {
                u.UserID,
                u.Username,
                u.Email,
                Permissions = u.Permissions.Select(p => new
                {
                    p.PermissionID,
                    p.Name
                })
            }).FirstOrDefaultAsync();

        if (user == null) return NotFound();

        return user;
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserID }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.UserID)
        {
            return BadRequest();
        }

        context.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null) return NotFound();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(int id)
    {
        return context.Users.Any(e => e.UserID == id);
    }
}