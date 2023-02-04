using BlogSiteFinal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace BlogSiteFinal.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly BlogDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public HomeController(ILogger<HomeController> logger, BlogDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogItems.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogItems == null)
            {
                return NotFound();
            }

            var blogItemsModel = await _context.BlogItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogItemsModel == null)
            {
                return NotFound();
            }

            return View(blogItemsModel);
        }



    }
}