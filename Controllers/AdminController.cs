using Microsoft.AspNetCore.Mvc;
using Phishing_Platform_Midterm.Context;
using Phishing_Platform_Midterm.Models;

namespace Phishing_Testing_Platform.Controllers;

public class AdminController : Controller
{
    private readonly MyDbContext _dbContext;

    public AdminController(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public IActionResult Admin()
    {

        var users = _dbContext.Users.ToList();

        // Calculate counts
        int nCount = users.Count(u => u.Sourcepage == "Netflix");
        int iCount = users.Count(u => u.Sourcepage == "Instagram");
        int uCount = users.Count(u => u.Sourcepage == "Udemy");

        // Pass counts to the view
        ViewBag.nCount = nCount;
        ViewBag.iCount = iCount;
        ViewBag.uCount = uCount;
        // Now, nCount, iCount, and uCount contain the counts for each Sourcepage value
        // You can use these counts as needed in your view or elsewhere

        return View(users);
    }

}