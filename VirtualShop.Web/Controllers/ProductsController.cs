﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VirtualShop.Web.Models;
using VirtualShop.Web.Roles;
using VirtualShop.Web.Services;
using VirtualShop.Web.Services.Contracts;

namespace VirtualShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<ProductViewModel>>> Index()
        {
            var result = await _productService.GetAllProducts();

            if (result == null)
                return View("Error");

            return View(result);
        }

        #region Criar Produto
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.CategoryId = new SelectList(await
                    _categoryService.GetAllCategories(), "CategoryId", "Name");
            
            return View();
        }        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct(ProductViewModel productVM)
        {
            if(ModelState.IsValid)
            {
                var result = await _productService.CreateProduct(productVM);

                if (result is not null)
                    return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.CategoryId = new SelectList(await 
                                        _categoryService.GetAllCategories(), "CategoryId", "Name");
            }
            return View(productVM);
        }
        #endregion

        #region Editar Produto
        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            ViewBag.CategoryId = new SelectList(await
                             _categoryService.GetAllCategories(), "CategoryId", "Name");

            var result = await _productService.FindProductById(id);

            if(result is null) 
                return View("Error");

            return View(result);
        }        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProduct(productVM);

                if (result is not null)
                    return RedirectToAction(nameof(Index));
            }
            return View(productVM);
        }
        #endregion

        #region Deletar Produto
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProductViewModel>> DeleteProduct(int id)
        {
            var result = await _productService.FindProductById(id);

            if (result is null)
                return View("Error");

            return View(result);
        }
        [HttpPost(), ActionName("DeleteProduct")]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _productService.DeleteProductById(id);

            if(!result)
                return View("Error");

            return RedirectToAction("Index");
        }
        #endregion
    }
}
