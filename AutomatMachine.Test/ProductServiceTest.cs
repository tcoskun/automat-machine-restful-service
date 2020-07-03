using System.Linq;
using AutomatMachine.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AutomatMachine.Test
{
    public class ProductServiceTest : TestBase
    {
        [Test]
        public void getProduct_return_a_product_by_id()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var products = productService.GetProducts();
            var actualProduct = products.First();
            var expectedProduct = productService.GetProduct(actualProduct.Id);

            Assert.IsNotNull(expectedProduct);
            Assert.AreEqual(expectedProduct.Id, actualProduct.Id);
        }
    }
}