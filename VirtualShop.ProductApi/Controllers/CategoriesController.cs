using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using VirtualShop.ProductApi.DTOs;
using VirtualShop.ProductApi.Roles;
using VirtualShop.ProductApi.Services;

namespace VirtualShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categoriesDTO = await _categoryService.GetCategories();

            if (categoriesDTO is null) 
                return NotFound("Categories not found");

            return Ok(categoriesDTO);
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriasProducts()
        {
            var categoriesDTO = await _categoryService.GetCategoriesProducts();

            if (categoriesDTO is null)
                return NotFound("Category not found");

            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var categoriesDTO = await _categoryService.GetCategoryById(id);

            if (categoriesDTO is null)
                return NotFound("Category not found");

            return Ok(categoriesDTO);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
                return BadRequest("Invalid Data");

            await _categoryService.AddCategory(categoryDTO);

            return new CreatedAtRouteResult("GetCategory", new { id = categoryDTO.CategoryId }, categoryDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
                return BadRequest();

            if (categoryDTO is null)
                return BadRequest();

            await _categoryService.UpdateCategory(categoryDTO);

            return Ok(categoryDTO);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<CategoryDTO>> Remover(int id)
        {
            var categoryDTO = await _categoryService.GetCategoryById(id);

            if (categoryDTO is null)
                return NotFound("Category not found");

            await _categoryService.RemoveCategory(id);

            return Ok(categoryDTO);
        }
    }
}
