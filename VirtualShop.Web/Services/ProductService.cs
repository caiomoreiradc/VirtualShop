﻿using System.Text.Json;
using VirtualShop.Web.Models;
using VirtualShop.Web.Services.Contracts;

namespace VirtualShop.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _clientFactory;
        private const string apiEndpoint = "/api/products/";
        private readonly JsonSerializerOptions _options;
        private ProductViewModel productVM;
        private IEnumerable<ProductViewModel> productsVM;

        public ProductService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory; 
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
        }

        public Task<IEnumerable<ProductViewModel>> GetAllProducts()
        {
            throw new NotImplementedException();
        }
        public Task<ProductViewModel> FindProductById(int id)
        {
            throw new NotImplementedException();
        }
        public Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
        {
            throw new NotImplementedException();
        }
        public Task<ProductViewModel> UpdateProduct(ProductViewModel productVM)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteProductById(int id)
        {
            throw new NotImplementedException();
        }

    }
}
