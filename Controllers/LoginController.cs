using BlogSiteFinal.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogSiteFinal.Controllers
{
    public class LoginController : Controller
    {
        private readonly BlogDbContext _context;
        private IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;

        public LoginController(BlogDbContext context, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(Admin name)
        {
            var info = _context.Admin.FirstOrDefault(x => x.UserName == name.UserName && x.Password == name.Password);
            if (info != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, info.UserName));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                AuthenticationProperties properties = new AuthenticationProperties();
                properties.IsPersistent = true;
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal),properties);
                return RedirectToAction("Index", "admin");
            }
            else
            {
                return View();
            }
        }

        
    }
}
