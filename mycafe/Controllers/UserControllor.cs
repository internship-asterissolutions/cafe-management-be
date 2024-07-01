using Microsoft.AspNetCore.Http;    
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mycafe.Data;
using mycafe.Models;


namespace mycafe.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserControllor : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        public UserControllor(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("SaveUser")]
        public ActionResult<User> PostUser(User user) 
        {
            try {
                _dbContext.Users.Add(user);
                 _dbContext.SaveChanges();
                return CreatedAtAction(nameof(PostUser), new { id =user.ID },user);

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while saving the user.");
            }
        }



        [HttpGet("GetUser")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            return _dbContext.Users.ToList();
        }




        [HttpPut("Update")]
        public async Task<IActionResult> PutUser(int id, User user)
        {


            _dbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _dbContext.Users.Any(e => e.ID == id);
        }


    }
}

