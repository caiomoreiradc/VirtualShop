using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualShop.ProductApi.DTOs;
using VirtualShop.ProductApi.Services;

namespace VirtualShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var productsDTO = await _productService.GetProducts();

            if (productsDTO is null)
                return NotFound("Products not found");

            return Ok(productsDTO);
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var productsDTO = await _productService.GetProductsById(id);

            if (productsDTO is null)
                return NotFound("Product not found");

            return Ok(productsDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductDTO productDTO)
        {
            if (productDTO is null)
                return BadRequest("Invalid Data");

            await _productService.AddProduct(productDTO);

            return new CreatedAtRouteResult("GetProduct", new { id = productDTO.Id }, productDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.Id)
                return BadRequest();

            if (productDTO is null)
                return BadRequest();

            await _productService.UpdateProduct(productDTO);

            return Ok(productDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Remover(int id)
        {
            var productDTO = await _productService.GetProductsById(id);

            if (productDTO is null)
                return NotFound("Product not found");

            await _productService.RemoveProduct(id);

            return Ok(productDTO);
        }
    }
}
