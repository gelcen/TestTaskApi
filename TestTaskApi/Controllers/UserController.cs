using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTaskApi.Models;

namespace TestTaskApi.Controllers
{
    /// <summary>
    /// Controller for users
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// User context
        /// </summary>
        private readonly UserContext _context;

        /// <summary>
        /// Constructor for UserController class
        /// </summary>
        /// <param name="context">UserContext</param>
        public UsersController(UserContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        // GET: api/users
        /// <summary>
        /// A method for getting all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/users/1
        /// <summary>
        /// A method for getting a one specific user.
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Returns the user or 404 if there is no 
        /// user with that id</returns>
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
        /// <summary>
        /// A method for creating a user
        /// </summary>
        /// <param name="user">The user entity from the request body.
        /// The Id field should be null.</param>
        /// <returns>201 if the user has been added,
        /// 400 if the modelstate is not valid or the user has Id field.</returns>
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
        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">Id of person to delete</param>
        /// <returns>404 if there is no person with that id.
        /// 204 if the person was deleted successfully.</returns>
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
        /// <summary>
        /// A method for adding/withdrawing money.
        /// </summary>
        /// <param name="id">Id of person whose balance
        /// you want to change</param>
        /// <param name="operation"><see cref="Operation"></param>
        /// <returns>200 if the operation was successful.
        /// 404 if the user with id wasn't found.
        /// 400 if the user has insufficient amount of money on the balance
        /// to withdraw.</returns>
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
