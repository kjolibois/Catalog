using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Api.Data;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;
using Catalog.Api.ProductExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers
{
    public class BasketController: BaseApiController
    {
        private readonly StoreContext _context;

        public BasketController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket ()
        {
            Basket? basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) return NotFound();
            return basket.MapBasketToDto();
        }
//todo chnge map basket
        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }
            return await _context.Baskets
                        .Include(i => i.Items)
                        .ThenInclude(p => p.Product)
                        .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }


        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }
        [HttpPost] //api/basket?productId=3&quantity=2
        public async Task<ActionResult> AddItemToBasket (int productId, int quantity)
        {
            Basket? basket= await RetrieveBasket(GetBuyerId()); // get basket
            
            if(basket == null) basket = CreateBasket(); // creakte basket if bask dosn't eixist
            
            var product = await _context.Products.FindAsync(productId); //find item product
            
            if(product == null) return BadRequest(new ProblemDetails{Title="Product Not Found"});// not found if no produt
            
            basket.AddItem(product,quantity);// ad new item
            
            var result = await _context.SaveChangesAsync() > 0;
            
            if (result) return CreatedAtRoute("GetBasket",basket.MapBasketToDto());            //save changes
            
            return BadRequest(new ProblemDetails{Title= "Problem saving item to basket"});
            
        }

        private Basket? CreateBasket()
        {
            var buyerId= User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions=new CookieOptions{IsEssential =true, Expires= DateTime.Now.AddDays(30)};
                Response.Cookies.Append("buyerId",buyerId,cookieOptions);
            }
  
            var basket = new Basket{BuyerId = buyerId};
            _context.Add(basket);
            return basket;
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem (int productId, int quantity)
        {
            var basket = await RetrieveBasket(GetBuyerId());
            
            if(basket == null) return NotFound();

            basket.RemoveItem(productId,quantity);

            var result = await _context.SaveChangesAsync() > 0;
            //remove item or reduce quantity
            //save changes
            if (result) { return Ok();}
 
            return BadRequest(new ProblemDetails{Title = "Problem removing item from the basket"});
        }
    }
}