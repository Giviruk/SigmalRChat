using ChatSignalR.Data;
using ChatSignalR.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ChatSignalR.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext _context;

        public AccountController(ApplicationContext context)
        {
            _context = context;
        }

        // тестирование SignalR
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WelcomePage()
        {
            return View();
        }

        public async Task<IActionResult> ChatsPage()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return View(rooms);
        }

        public IActionResult Chat(int id)
        {
            var room  = _context.Rooms.First(x=>x.Id==id);
            return View(room);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Signin()
        {
            return View();
        }

        public async Task<IActionResult> AllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Ban(User user)
        {
            user.RoleId = _context.Roles.First(x => x.Name=="banned").Id;;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("AllUsers", "Account");
        }

        public IActionResult Unban(User user)
        {
            user.RoleId = _context.Roles.First(x => x.Name=="user").Id;;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("AllUsers", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    await Authenticate(user); // аутентификация
                    return RedirectToAction("WelcomePage", "Account"); // переадресация на метод Index
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Nickname),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        //Добавление участника
        public async Task<IActionResult> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("WelcomePage");
        }

        public async Task<IActionResult> Createroom(string name)
        {
            var ownerId = _context.Users.First(user => user.Nickname == User.Identity.Name).Id;
            var room = new Room
            {
                Name = name,
                OwnerID = ownerId
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("ChatsPage");
        }
        public async Task<IActionResult> Deleteroom(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction("ChatsPage");
        }
    }
}