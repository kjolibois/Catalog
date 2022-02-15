using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Catalog.Api.Data;
using Catalog.Api.Entities;

using Catalog.Api.ProductExtensions;
using Catalog.Api.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers
{

    public class ProductsController : BaseApiController
    {   
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }
        // #TODO switch xpression research
        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts(
            [FromQuery] ProductParams productParams
            )
        {
                var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands,productParams.Types)
                .AsQueryable();
                var products = await PagedList<Product>.ToPagedList(
                    query,productParams.PageNumber,productParams.PageSize
                );
                Response.AddPaginationHeader(products.MetaData);
                return products;
        }

        [HttpGet("{id}")] //api/products/3
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if (product == null) return NotFound();
            
            return product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await  _context.Products.Select(p => p.Type).Distinct().ToListAsync();
            return Ok(new {brands,types});
        }


    }
}