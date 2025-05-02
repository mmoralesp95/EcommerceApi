using EcommerceApi.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace EcommerceApi.Api.Controllers;

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public WebhookController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        var endpointSecret = _config["Stripe:WebhookSecret"];
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                endpointSecret
            );
        }
        catch (StripeException e)
        {
            return BadRequest($"Webhook error: {e.Message}");
        }

        if (stripeEvent.Type == "payment_intent.succeeded")
        {
            var intent = (PaymentIntent)stripeEvent.Data.Object;

            var order = _context.Orders.FirstOrDefault(o => o.StripePaymentIntentId == intent.Id);
            if (order != null)
            {
                order.PaymentStatus = "Pagado";
                await _context.SaveChangesAsync();
            }
        }

        return Ok();
    }
}
