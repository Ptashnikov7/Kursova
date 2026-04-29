using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace Kursova.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        [HttpPost("create-session")]
        public ActionResult CreateCheckoutSession([FromBody] List<CartItemDto> cartItems)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + "/Home/Index?checkout=success",
                CancelUrl = domain + "/Home/Index?checkout=cancel",
            };

            foreach (var item in cartItems)
            {
                options.LineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // in cents/kopecks
                        Currency = "uah",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                        },
                    },
                    Quantity = item.Quantity,
                });
            }

            try
            {
                var service = new SessionService();
                Session session = service.Create(options);

                return Ok(new { sessionId = session.Id, url = session.Url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class CartItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
