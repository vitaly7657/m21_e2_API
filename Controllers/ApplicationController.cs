using m21_e2_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace m21_e2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        ApplicationContext db;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        public ApplicationController(ApplicationContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            db = context;
            if (!db.Subscribers.Any())
            {
                //создание записей абонентов в БД, если их нет
                db.Subscribers.Add(new Subscriber { Name = "Борис", Surname = "Савельев", Patronymic = "Валерьевич", PhoneNumber = "+7(950)124-30-66", Address = "360377, Липецкая область, город Наро-Фоминск, наб. Балканская, 01", Description = "описание 1", });
                db.Subscribers.Add(new Subscriber { Name = "Юлиан", Surname = "Веселов", Patronymic = "Ефимович", PhoneNumber = "+7(910)270-11-07", Address = "397816, Владимирская область, город Солнечногорск, въезд Сталина, 79", Description = "описание 2", });
                db.SaveChanges();
            }
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //SUBSCRIBERS----------------------------------------------------
        //получение всех абонентов
        [HttpGet("subscribers")]
        public async Task<ActionResult<IEnumerable<Subscriber>>> GetSubscriber()
        {
            return await db.Subscribers.ToListAsync();
        }

        //получение одного абонента
        [HttpGet("subscribers/{id}")]
        public async Task<ActionResult<Subscriber>> GetSubscriber(int id)
        {
            Subscriber subscriber = await db.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
            if (subscriber == null)
            {
                return NotFound();
            }
            return Ok(subscriber);
        }

        //добавление абонента
        [HttpPost("subscribers")]
        public async Task<IActionResult> PostSubscriber(Subscriber subscriber)
        {
            await db.Subscribers.AddAsync(subscriber);
            var result = await db.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        //удаление абонента
        [HttpDelete("subscribers/{id}")]
        public async Task<IActionResult> DeleteSubscriber(int id)
        {
            Subscriber subscriber = await db.Subscribers.FirstOrDefaultAsync(x => x.Id == id);
            db.Subscribers.Remove(subscriber);
            var result = await db.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        //изменение абонента
        [HttpPut("subscribers")]
        public async Task<IActionResult> PutSubscriber(Subscriber subscriber)
        {
            db.Update(subscriber);
            var result = await db.SaveChangesAsync();
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok();
        }

        //USERS----------------------------------------------------
        //получение всех пользователей
        [HttpGet("users")]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await _userManager.Users.ToListAsync();
        }

        //получение одного конкретного пользователя
        [HttpGet("users/{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            User user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //добавление пользователя
        [HttpPost("users")]
        public async Task<IActionResult> Post(UserForm reg)
        {
            User user = new User()
            {
                UserName = reg.UserName
            };
            var result = await _userManager.CreateAsync(user, reg.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        //удаление пользователя
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        //изменение пользователя
        [HttpPut("users")]
        public async Task<IActionResult> Put(User user)
        {
            User userUpdate = await _userManager.FindByIdAsync(user.Id);
            userUpdate.UserName = user.UserName;
            var result = await _userManager.UpdateAsync(userUpdate);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        //LOGIN----------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForm user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, true, false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }

        //REGISTER----------------------------------------------------        
        [HttpPost("register")]
        public async Task<IActionResult> Registration(UserForm reg)
        {
            User user = new User()
            {
                UserName = reg.UserName
            };
            var result = await _userManager.CreateAsync(user, reg.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
