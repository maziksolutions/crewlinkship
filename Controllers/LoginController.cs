using crewlinkship.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace crewlinkship.Controllers
{
    public class LoginController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly shipCrewlinkContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly AppSettings _appSettings;


        public LoginController(shipCrewlinkContext context, IHostingEnvironment appEnvironment, IOptions<AppSettings> appSettings)
        {
           
            _context = context;
            _appEnvironment = appEnvironment;
            _appSettings = appSettings.Value;
        }



        public IActionResult UserLogin()
        {
            ViewBag.mssg = TempData["mssg"] as string;
            return PartialView();
        }

        [HttpPost]
        //[AllowAnonymous]
        public IActionResult Login(Userlogin userlogin)
        {
            var User = _context.Userlogins.SingleOrDefault(x => x.UserName == userlogin.UserName && x.Password == userlogin.Password && x.IsDeleted == false);

            if (User != null)
            {
                //JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var Key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescritor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, User.UerId.ToString()),

                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Key),
                        SecurityAlgorithms.HmacSha256Signature)

                };
                var tokenKey = tokenHandler.CreateToken(tokenDescritor);
                string tok = tokenHandler.WriteToken(tokenKey);
                HttpContext.Session.SetString("token", tok);
               
                User.Password = "";
                HttpContext.Session.SetString("name", userlogin.UserName);
                //TempData["name"] = userlogin.UserName;
                return RedirectToAction("vwCrewList","Home");
            }
            TempData["mssg"] = "Please Check Your Username and Password";
            return RedirectToAction("UserLogin");

        }

    }
}
