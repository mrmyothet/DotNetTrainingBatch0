using DotNetTrainingBatch0.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch0.Mvc.Controllers;

public class BlogController : Controller
{
    private readonly DotNetTrainingBatch0DbContext _db;

    public BlogController(DotNetTrainingBatch0DbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blogs = await _db.TblBlogs
            .AsNoTracking()
            .OrderByDescending(x=> x.BlogId)
            .ToListAsync();

        return View(blogs);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(TblBlog blog)
    {
        await _db.TblBlogs.AddAsync(blog);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var blog = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
        if (blog is null)
        {
            return RedirectToAction("Index");
        }
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, TblBlog blog)
    {
        var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
        if (item is null)
        {
            return RedirectToAction("Index");
        }

        item.BlogTitle = blog.BlogTitle;
        item.BlogAuthor = blog.BlogAuthor;
        item.BlogContent = blog.BlogContent;

        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.TblBlogs.FirstOrDefaultAsync(x => x.BlogId == id);
        if (item is null)
        {
            return RedirectToAction("Index");
        }

        _db.TblBlogs.Remove(item);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
