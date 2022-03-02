using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Api.Data;
using Catalog.Api.Dtos;
using Catalog.Api.Entities;

using Catalog.Api.ProductExtensions;
using Catalog.Api.RequestHelpers;
using Catalog.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Api.Controllers
{

    public class ProductsController : BaseApiController
    {   
        private readonly StoreContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public ProductsController(StoreContext context , IMapper mapper , ImageService imageService)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
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

        [HttpGet("{id}",Name="GetProduct")] //api/products/3
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

        [Authorize(Roles="Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if(productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);
               
                if (imageResult.Error != null) return BadRequest(new ProblemDetails{Title= imageResult.Error.Message});
            
                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            _context.Products.Add(product);



            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new{Id = product.Id},product);
            
            return BadRequest(new ProblemDetails {Title = "Problem creating new product"});
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct([FromForm]UpdateProductDto productDto){
            var product = await _context.Products.FindAsync(productDto.Id);
          
            if(product == null) return NotFound(); 
            
            _mapper.Map(productDto,product);
            
            if(productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);
                
                if (imageResult.Error != null) return BadRequest(new ProblemDetails{Title= imageResult.Error.Message});

                if(!string.IsNullOrEmpty(product.PublicId))
                    await _imageService.DeleteImageAsync(product.PublicId);
                
                product.PictureUrl = imageResult.SecureUrl.ToString();
                product.PublicId = imageResult.PublicId;
            }

            var result = await _context.SaveChangesAsync() > 0;
       
            if (result) return Ok(product);
            
            return BadRequest(new ProblemDetails{Title = "Problem updating"});       
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            
            if(product == null) return NotFound();
            

            if(!string.IsNullOrEmpty(product.PublicId))
                await _imageService.DeleteImageAsync(product.PublicId);

            _context.Products.Remove(product);
                var result = await _context.SaveChangesAsync() > 0;
            Console.WriteLine(result);
            if (result) return Ok();
            
            return BadRequest(new ProblemDetails{Title = "Problem deleting"});       
        }

    }
}