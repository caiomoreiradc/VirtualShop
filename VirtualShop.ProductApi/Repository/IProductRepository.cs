using VirtualShop.ProductApi.Models;

namespace VirtualShop.ProductApi.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<Product> Created(Product product);
        Task<Product> Update(Product product);
        Task<Product> Delete(int id);
    }
}
