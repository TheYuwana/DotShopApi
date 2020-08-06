using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotShopApi.Data;
using DotShopApi.Models;
using DotShopApi.DTO;

namespace DotShopApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShopContext _context;

        public UserController(ShopContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTOSimple>>> GetUsers()
        {
            // Fetch all users. Returns an empty list or a list of user objects
            return await _context.Users.Select(user => SimpleUser(user)).ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTOSimple>> GetUser(long id)
        {
            // Fetch user by the given ID, returns null or a user object
            var user = await GetActiveUser(id);

            // If the user is not found, return a 404
            if (user == null)
            {
                return NotFound();
            }

            // If found, then retun the user object
            return SimpleUser(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, UserDTOCreateUpdate userDTOCreateUpdate)
        {
            // Fetch user by the given ID, returns null or a user object
            var user = await GetActiveUser(id);

            // If the user is not found, return a 404
            if (user == null)
            {
                return NotFound();
            }

            // Update user values
            user.Name = userDTOCreateUpdate.Name;
            user.Email = userDTOCreateUpdate.Email;
            user.DateModified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTOCreateUpdate userDTOCreateUpdate)
        {
            // Create a new product based on the given product DTO
            var user = new User
            {
                Id = DotShopApi.Utils.GenerateRandomId(),
                Name = userDTOCreateUpdate.Name,
                Email = userDTOCreateUpdate.Email,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Deleted = false
            };

            // Persit and save in the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return created user
            return CreatedAtAction("GetUser", new { id = user.Id }, SimpleUser(user));
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            // Fetch user by the given ID, returns null or a user object
            var user = await GetActiveUser(id);

            // If the user is not found, return a 404
            if (user == null)
            {
                return NotFound();
            }

            user.Deleted = true;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Fetch product if by id and not deleted
        private async Task<User> GetActiveUser(long id)
        {
            return await _context.Users.Where(u => !u.Deleted && u.Id == id).FirstOrDefaultAsync<User>();
        }

        // Convert to simple user data
        private static UserDTOSimple SimpleUser(User user) => new UserDTOSimple
        {
            Name = user.Name
        };
    }
}
