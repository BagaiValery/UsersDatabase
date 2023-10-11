using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersDatabase.Data;
using UsersDatabase.Interfaces;
using UsersDatabase.Models;

namespace UsersDatabase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly UserContext _dbContext;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        
        public UsersController (UserContext dbContext, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository= roleRepository;
            _dbContext = dbContext;
        }

        #region HttpGet_User

        /// <summary>
        /// Gets the list of all users ordered by ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = _userRepository.GetAllUsers();
            
            if(result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _dbContext.Users.Include(r => r.UserRole).ThenInclude(r => r.Role).ToListAsync());
        }

        [HttpGet]
        [Route("userId")]
        public IActionResult GetUserByID(int userId)
        {
            if(!_userRepository.UserExists(userId))
                return NotFound();
            
            var result = _userRepository.GetUser(userId);
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return Ok(result);
        }

        [HttpGet]
        [Route("userrole")]
        public IActionResult FilterByRole (string roleName) 
        {
            var result = _userRepository.FilterRole(roleName);

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("userAge")]
        public IActionResult FilterByAge (int age)
        {
            var result = _userRepository.FilterAge(age);

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("userName")]
        public IActionResult FilterByName(string name)
        {
            var result = _userRepository.FilterName(name);

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("userEmail")]
        public IActionResult FilterByEmail(string userEmail)
        {
            var result = _userRepository.FilterEmail(userEmail);

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("userAgeSort")]
        public IActionResult SortByAge()
        {
            var result = _userRepository.SortByAge();

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("userNameSort")]
        public IActionResult SortByName()
        {
            var result = _userRepository.SortByName();

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("userEmailSort")]
        public IActionResult SortByEmail()
        {
            var result = _userRepository.SortByEmail();

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(result);
        }

        #endregion

        #region HttpGet_Roles

        [HttpGet]
        [Route("AllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = _roleRepository.GetAllRoles();

            if (result == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _dbContext.Roles.ToListAsync());
        }

        [HttpGet]
        [Route("roleId")]
        public IActionResult GetRoleByID(int roleId)
        {
            if (!_roleRepository.RoleExists(roleId))
                return NotFound();

            var result = _roleRepository.GetRole(roleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        [HttpGet]
        [Route("roleName")]
        public IActionResult GetRole(string roleName)
        {
            if (!_roleRepository.RoleExists(roleName))
                return NotFound();

            var result = _roleRepository.GetRole(roleName);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(result);
        }

        #endregion
        
        #region HttpPost
        
        [HttpPost]
        [Route("{roleId}/AddRole/{userId}", Name = nameof(AddRole))]
        public ActionResult<bool> AddRole( int roleId, int userId)
        {
            var userrole = _dbContext.UsersRoles
            .Where(u => u.UserId == userId)
            .Where(r => r.RoleId == roleId)
            .FirstOrDefault();

            if (userrole != null)
            {
                ModelState.AddModelError("", "User already had this role");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _dbContext.UsersRoles.Add(new() { UserId = userId, RoleId = roleId });
            _dbContext.SaveChangesAsync();
            return Ok(true);
        }
              

        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser([FromQuery] int roleId, [FromBody] User createdUser)
        {
            if (createdUser == null)
                return BadRequest(ModelState);
            
            #region Волидация полей
            if (createdUser.Name== null)
                return BadRequest(ModelState);
            if (createdUser.Email== null)
                return BadRequest(ModelState);
            if (createdUser.Age <= 0)
            {
                ModelState.AddModelError("", "Wrong age (= or < 0)");
                return BadRequest(ModelState);
            }
            
            var email = _userRepository.GetAllUsers()
           .Where(p => p.Email.Trim().ToUpper() == createdUser.Email.TrimEnd().ToUpper())
           .FirstOrDefault();
            if (email != null)
            {
                ModelState.AddModelError("", "Email already used");
                return StatusCode(422, ModelState);
            }
            #endregion

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.CreateUser(roleId, createdUser))
            {
                ModelState.AddModelError("", "Something wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Secsess");
        }

        [HttpPost]
        [Route("CreateRole")]
        public IActionResult CreateRole([FromBody] Role createdRole)
        {
            if (createdRole == null)
                return BadRequest(ModelState);

            var role = _roleRepository.GetAllRoles()
            .Where(p => p.RoleName.Trim().ToUpper() == createdRole.RoleName.TrimEnd().ToUpper())
            .FirstOrDefault();

            if (role != null)
            {
                ModelState.AddModelError("", "Role already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_roleRepository.CreateRole(createdRole))
            {
                ModelState.AddModelError("", "Something wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Secsecc");
        }

        #endregion

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUser( int userId, User updatingUser)
        {
            if(userId != updatingUser.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(updatingUser).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(UserAvailable(userId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updatingUser);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUser (int userId)
        {
            if(_dbContext.Users == null)
            {
                return NotFound();
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        // check if this data from database available or not. If its here someone with this id exists
        private bool UserAvailable(int userId)
        {
            return (_dbContext.Users?.Any(x=>x.Id == userId)).GetValueOrDefault();
        }
    }
}
