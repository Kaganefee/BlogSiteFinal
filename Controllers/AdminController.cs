using BlogSiteFinal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogSiteFinal.Controllers
{
    [Authorize]
    
    public class AdminController : Controller
    {

        private readonly BlogDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;


        public AdminController(BlogDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

        }



        // GET: Admin
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogItems.ToListAsync());
        }

        // GET: Admin/Details/5
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

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ImageFile")] BlogItemsModel blogItemsModel)
        {
            if (ModelState.IsValid)
            {
                await UploadImageAsync(blogItemsModel);
                _context.Add(blogItemsModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogItemsModel);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogItems == null)
            {
                return NotFound();
            }

            var blogItemsModel = await _context.BlogItems.FindAsync(id);
            if (blogItemsModel == null)
            {
                return NotFound();
            }
            return View(blogItemsModel);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ImageFile")] BlogItemsModel blogItemsModel)
        {

            if (id != blogItemsModel.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                await UploadImageAsync(blogItemsModel);
                try
                {
                    var updatedEntityWithImage = _context.Entry(blogItemsModel);
                    updatedEntityWithImage.State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogItemsModelExists(blogItemsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Attach(blogItemsModel);
                    _context.Entry(blogItemsModel).Property(x=>x.Title).IsModified= true;
                    _context.Entry(blogItemsModel).Property(x => x.Description).IsModified = true;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogItemsModelExists(blogItemsModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(blogItemsModel);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BlogItems == null)
            {
                return Problem("Entity set 'BlogDbContext.BlogItems'  is null.");
            }
            var blogItemsModel = await _context.BlogItems.FindAsync(id);

            //Delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", blogItemsModel.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            //delete from record

            if (blogItemsModel != null)
            {
                _context.BlogItems.Remove(blogItemsModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Get: Icons 


        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool BlogItemsModelExists(int id)
        {
            return _context.BlogItems.Any(e => e.Id == id);
        }


        private async Task UploadImageAsync(BlogItemsModel blogItemsModel)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(blogItemsModel.ImageFile.FileName);
            string extension = Path.GetExtension(blogItemsModel.ImageFile.FileName);
            blogItemsModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmdd") + extension;
            string path = Path.Combine(wwwRootPath + "/image/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await blogItemsModel.ImageFile.CopyToAsync(fileStream);
            }
        }
    }
}
