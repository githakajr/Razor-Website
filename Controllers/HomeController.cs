using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace RazorWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        // GET: Contact Page
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        // POST: Handle form submission
        [HttpPost]
        public async Task<IActionResult> Contact(string Name, string Email, string Subject, string Message)
        {
            // Send email
            if (await SendEmail(Name, Email, Subject, Message))
            {
                ViewData["MessageSent"] = "Thank you for your message. We will get back to you soon!";
            }
            else
            {
                ViewData["MessageSent"] = "Oops! Something went wrong. Please try again.";
            }

            return View();
        }

        private async Task<bool> SendEmail(string name, string email, string subject, string message)
        {
            try
            {
                var senderEmail = "githakajr@gmail.com"; // Your email
                var senderPassword = "xzzkljvtjvlhtama"; // Your email password or app password

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(email), // Email from the user who filled the form
                    Subject = subject,
                    Body = $"Name: {name}\nEmail: {email}\n\nMessage:\n{message}",
                    IsBodyHtml = false,
                };

                // Email address to receive the message
                mailMessage.To.Add("githakajr@gmail.com");

                await smtpClient.SendMailAsync(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
