using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskApi.Models;

namespace TestTaskApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/users  
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            if (!ModelState.IsValid || user.Id != null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // Delete: api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Patch: api/users/5
        [HttpPatch("{id}")]
        public async Task<ActionResult<User>> BalanceOperation(long id, Operation operation)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            switch (operation.OperationType)
            {
                case OperationType.Add:
                    user.AddToBalance(operation.Sum);
                    break;
                case OperationType.Withdraw:
                    try
                    {
                        user.Withdraw(operation.Sum);
                    }
                    catch (ArgumentException ex)
                    {
                        return BadRequest(ex.Message);                     
                    }
                    break;
                default:
                    break;
            }

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
