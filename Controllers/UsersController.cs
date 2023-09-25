using Amazon.Runtime.Internal.Util;
using CSWebAPI.Models;
using CSWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using SampleWebAPI.Models;
using SampleWebAPI.Services;
using SampleWebAPI.Tools;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;        

        public UsersController(UsersService usersService)
        {
            _userService = usersService;            
        }



        //CB-09242023 Customized API
        
        [HttpGet("Login")]        
        public async Task<ActionResult<Users>> Login(string username,string password)
        {
            var user = await _userService.GetUserAsync(username);

            if (user is null)
            {
                return NotFound();
            }

            //CB-09242023 To Check password hash if match on query user
            Encrypt enc = new Encrypt();
            string HashUser = enc.GetHashPassword(password);

            if(HashUser != user.Password)
            {
                return NotFound();
            }

            return user;
        }
        //CB-09242023 Register User
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> PostUsers(Users newUser)
        {
            //hashing password;
            var hash = string.Empty;

            //CB-09242023 Update Password Hashing
            Encrypt enc = new Encrypt();
            newUser.Password = enc.GetHashPassword(newUser.Password);

            await _userService.CreateAsync(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        //CB-09252023 Additional API for User Authentication
        [HttpPost("UserAuthenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var user = await _userService.GetUserAsync(model.Username);

            if (user is null)
            {
                return NotFound();
            }

            //CB-09252023 To Check password hash if match on query user for Authentication
            Encrypt enc = new Encrypt();
            string HashUser = enc.GetHashPassword(model.Password);

            if (HashUser != user.Password)
            {
                return NotFound();
            }

            var response = _userService.CreateToken(user);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        //-------------------------------------------------------------------
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<List<Users>> Get() =>
            await _userService.GetUsersAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Users>> Get(string id)
        {
            var user = await _userService.GetUserIDAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Users newUser)
        {
            //hashing password;
            var hash = string.Empty;

            //CB-09242023 Update Password Hashing
            Encrypt enc = new Encrypt();
            newUser.Password = enc.GetHashPassword(newUser.Password);
           
            await _userService.CreateAsync(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Users updatedUser)
        {
            var user = await _userService.GetUserIDAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            updatedUser.Id = user.Id;
            //CB-09242023 Update Password Hashing
            Encrypt enc = new Encrypt();
            updatedUser.Password =enc.GetHashPassword(user.Password);

            await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetUserIDAsync(id);
            if (user is null)
            {
                return NotFound();
            }
            await _userService.RemoveAsync(id);
            return NoContent();
        }
    }
}
