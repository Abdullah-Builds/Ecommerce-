using MailKit.Search;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyApp.Data;
public class OrderController : Controller
{
    private readonly OrderDBContext _context;
    private readonly IEmailSender _emailSender;
    private readonly IHubContext<OrderHub> _hubContext;

    public OrderController(OrderDBContext context, IEmailSender emailSender, IHubContext<OrderHub> hubContext)
    {
        _context = context;
        _emailSender = emailSender;
        _hubContext = hubContext;
    }

    public async Task<int> SendConfirmationEmail()
    {
        try
        {
            var order = new Order
            {
                CustomerEmail = "khan.abdullah135790@gmail.com",
                ConfirmationToken = Guid.NewGuid().ToString(),
                IsConfirmed = false
            };

            _context.OrdersConfirmed.Add(order);
            await _context.SaveChangesAsync();

            var confirmationUrl = Url.Action("ConfirmOrder", "Order", new
            {
                id = order.Id,
                token = order.ConfirmationToken
            }, protocol: Request.Scheme);

            string emailBody = $@"
    <p>Dear Customer,</p>

    <p>Thank you for your order. To complete the process, please confirm your order by clicking the link below:</p>

    <p><a href='{confirmationUrl}' style='color: #007bff; text-decoration: none;'>Confirm Your Order</a></p>

    <p>If you did not place this order, please disregard this email.</p>

    <p>Best regards,<br />
    The MyApp Team</p>";


            await _emailSender.SendEmailAsync(order.CustomerEmail, "Confirm Your Order", emailBody);
            return order.Id; 
        }
        catch (Exception)
        {

            return -1;
        }


    }
    public IActionResult Order()
    {
        return View();
    }
    public IActionResult OrderConfirmationNotice()
    {
        return View();
    }

    // Confirmation landing page


    [HttpPost]

    public async Task<IActionResult> PlaceOrder()
    {
        var OrderId = await SendConfirmationEmail();
        if (OrderId != -1)
        {
            return Ok();
        }
        return BadRequest();

    }

    [HttpGet]
    public async Task<IActionResult> ConfirmOrder(int id, string token)
    {
        var order = await _context.OrdersConfirmed.FindAsync(id);

        if (order == null || order.ConfirmationToken != token)
        {
            return NotFound("Invalid or expired confirmation link.");
        }

        if (order.IsConfirmed)
        {
            return Content("Order already confirmed.");
        }

        order.IsConfirmed = true;
        await _context.SaveChangesAsync();

        // Push real-time notification to clients
        await _hubContext.Clients.All.SendAsync("OrderConfirmed", order.Id);

        return View("OrderConfirmationNotice",order.Id);
    }

}
