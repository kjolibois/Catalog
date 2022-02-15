using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Data;
using Catalog.Api.Dtos;
using Catalog.Api.Entities.OrderAggregate;
using Catalog.Api.ProductExtensions;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Catalog.Api.Controllers.ecom
{
    public class PaymentsController : BaseApiController
    {
        private readonly PaymentService _paymentService;
        private readonly StoreContext _context;

        private readonly IConfiguration _config;
        public PaymentsController(PaymentService paymentService, StoreContext context, IConfiguration config)
        {
            _paymentService = paymentService;
            _context = context;
            _config = config;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
        {
            var basket = await _context.Baskets
                .RetrieveBasketWithItems(User.Identity.Name)
                .FirstOrDefaultAsync();
            Console.WriteLine(basket.Items);
            if (basket == null) return NotFound();

            var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);
            Console.WriteLine(intent+"fdf");
            if (intent == null) return BadRequest(new ProblemDetails{Title = "Problem creating payment intent"});
            Console.WriteLine(intent.ClientSecret);
            Console.WriteLine(intent.Id);
            basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;

            basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;
            
            _context.Update(basket);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return  BadRequest(new ProblemDetails{Title = "Proble updating basket with intent"});
           
            return basket.MapBasketToDto();
        }
        [HttpPost("webhook")]
        public async Task<ActionResult>StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent= EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"],
            _config["StripeSettings:WhSecret"] );
            var charge = (Charge)stripeEvent.Data.Object;
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.PaymentIntentId == charge.PaymentIntentId);
            if (charge.Status =="succeeded") order.OrderStatus = OrderStatus.PaymentReceived;

            await _context.SaveChangesAsync();
            
            return new EmptyResult();
        }
    }
}