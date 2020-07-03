using AutomatMachine.Data;
using System;
using System.Linq;

namespace AutomatMachine.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _dataContext;
        public ProductService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Product[] GetProducts()
        {
            return _dataContext.Product.ToArray();
        }

        public Product GetProduct(Guid id)
        {
            return _dataContext.Product.FirstOrDefault(p => p.Id == id);
        }
    }

    public interface IProductService
    {
        Product[] GetProducts();
        Product GetProduct(Guid id);
    }
}
