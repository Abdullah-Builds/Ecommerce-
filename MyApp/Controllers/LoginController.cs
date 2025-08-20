using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.UI.Services;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginDBContext _context;
        private readonly IEmailSender _emailSender;

        public LoginController(LoginDBContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToLoginDBAsync([FromBody] LoginModel model)
        {
            model.Date = DateTime.Now;

#pragma warning disable CS8601 // Possible null reference assignment.
            model.IP = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                     ?? HttpContext.Connection.RemoteIpAddress?.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.

            _context.Logins.Add(model);
            await _context.SaveChangesAsync();

            string subject = $"🎉 Welcome to MyApp, {model.FirstName}!";

            string body = $@"
    <div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6; max-width: 600px; margin: auto;'>
        <h2 style='color: #2c3e50;'>Hi {model.FirstName},</h2>
        <p>Thank you for registering with <strong>MyApp</strong>! We're excited to have you on board.</p>
        <p>You can now log in and explore everything we have to offer.</p>
        <p>If you have any questions or need support, feel free to reach out at any time.</p>
        <br />
        <p style='font-size: 0.9em; color: #888;'>Cheers,</p>
        <p style='font-weight: bold;'>The MyApp Team</p>
    </div>";

            await _emailSender.SendEmailAsync(model.Email, subject, body);

            return Ok(new { message = "User registered and welcome email sent." });
        }
    }
}
