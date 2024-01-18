using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phishing_Platform_Midterm.Context;
using Phishing_Platform_Midterm.Entities;

namespace Phishing_Platform_Midterm.Controllers
{
    public class PhishingEmailController : Controller
    {
        private readonly MyDbContext _context;

        public PhishingEmailController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult SendMail(String message, string selectedLink)
        {
            string randomTargetEmail = GetRandomTargetEmail();
            string randomEmail = GenerateRandomEmailAddress();
            SaveToDatabase(randomEmail);
            SendEmail(randomEmail, randomTargetEmail, selectedLink);


            return View();
        }


        private string GetRandomTargetEmail()
        {
            // Get the count of targetemails in the database
            int targetEmailCount = _context.Targetemails.Count();

            // Generate a random index to select a targetemail
            Random random = new Random();
            int randomIndex = random.Next(0, targetEmailCount);

            // Get the targetemail at the random index
            string randomTargetEmail = _context.Targetemails
                // Optional: Order by another property or remove ordering
                //.OrderBy(e => EF.Property<DateTime>(e, "AnotherProperty"))
                .Skip(randomIndex)
                .Take(1)
                .FirstOrDefault()?.Targetemail1;

            return randomTargetEmail;
        }


        public static string GenerateRandomEmailAddress()
        {
            string[] domains = { "example.com", "test.com", "sample.org", "demo.net", "admin.com", "homework.org" };

            Random random = new Random();
            int randomIndex = random.Next(0, domains.Length);

            string username = Guid.NewGuid().ToString().Substring(0, 8); // Generate a random string
            string domain = domains[randomIndex];

            return $"{username}@{domain}";
        }

        private void SaveToDatabase(string emailAddress)
        {
            // Create a new Phishingtemplate instance
            Phishingtemplate phishingTemplate = new Phishingtemplate
            {
                Templatemail = emailAddress,
                Content = "Phishing mail was created and saved." ?? string.Empty, // Set content with a non-null value
                Createdat = DateTime.Now
            };

            // Add the instance to the context and save changes
            _context.Phishingtemplates.Add(phishingTemplate);
            _context.SaveChanges();
        }


        private void SendEmail(string from, string to, string selectedLink)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = "Phishing Email";

                // Get the selected link URL based on the user's choice
                string linkUrl = GetLinkUrl(selectedLink);

                // Generate a unique identifier for tracking
                Guid trackingId = Guid.NewGuid();

                // Modify the email body to include the tracking link
                mail.Body =
                    $"Click here: <a href=\"https://yourdomain.com/track?id={trackingId}&url={Uri.EscapeDataString(linkUrl)}\">{selectedLink}</a>";

                smtpClient.Host = "my-smtp-host"; 
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(from, "my-smtp-password");
                smtpClient.EnableSsl = true;
                
                // Save the tracking information to the database
                SaveSentEmailToDatabase(from, to, selectedLink, trackingId, mail.Body);
                
                smtpClient.Send(mail);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
            finally
            {
                mail.Dispose();
                smtpClient.Dispose();
            }
        }

        private string GetLinkUrl(string selectedLink)
        {
            switch (selectedLink)
            {
                case "Netflix":
                    return "Netflix/Nlogin/";
                case "Udemy":
                    return "Udemy/Ulogin/";
                case "Instagram":
                    return "Instagram/Ilogin/";
                default:
                    return string.Empty;
            }
        }



        private void SaveSentEmailToDatabase(string from, string to, string selectedLink, Guid trackingId,
            string message)
        {
            if (!ModelState.IsValid)
            {
                // Log or print validation errors
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var errors = ModelState[modelStateKey].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }


                try
                {
                    // Find the template based on the email address (assuming Templatemail is unique)
                    Phishingtemplate template = _context.Phishingtemplates
                        .Where(t => t.Templatemail == from)
                        .OrderByDescending(t => t.Id)
                        .FirstOrDefault();

                    if (template != null)
                    {
                        // Find the target based on the target email
                        Targetemail target = _context.Targetemails
                            .Where(e => e.Targetemail1 == to)
                            .OrderByDescending(e => e.Id)
                            .FirstOrDefault();

                        if (target != null)
                        {
                            // Create a new Sentemail instance
                            Sentemail sentEmail = new Sentemail
                            {
                                Templateid = template.Id,
                                Targetid = target.Id,
                                Sentat = DateTime.Now,
                                Isclicked = false,
                                Clickedat = null,
                                // Additional properties as needed
                            };

                            // Add the instance to the context and save changes
                            _context.Sentemails.Add(sentEmail);
                            _context.SaveChanges();


                            // Save user interaction to the database
                            SaveUserInteraction(sentEmail.Emailid, "Email Sent", $"Sent to: {to}", DateTime.Now);
                        }
                        else
                        {
                            Console.WriteLine($"Error: Target not found for email {to}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: Template not found for email {from}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception instead of writing to the console
                    Console.WriteLine($"Error saving sent email to database: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                }
            }

        }
        
        private void SaveUserInteraction(int? emailId, string interactionType, string interactionDetail, DateTime interactionTime) {
            // Create a new Userinteraction instance
            Userinteraction userInteraction = new Userinteraction
            {
                Emailid = emailId,
                Interactiontype = interactionType,
                Interactiondetail = interactionDetail,
                Interactiontime = interactionTime
            };

            // Add the instance to the context and save changes
            _context.Userinteractions.Add(userInteraction);
            _context.SaveChanges();
        }
    }
}
