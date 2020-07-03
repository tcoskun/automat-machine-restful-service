using Microsoft.AspNetCore.Mvc;
using System;
using AutomatMachine.Common.Response;
using AutomatMachine.Data;
using AutomatMachine.Services;

namespace AutomatMachine.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProcessService _processService;
        public ProductsController(IProductService productService, IProcessService processService)
        {
            _productService = productService;
            _processService = processService;
        }

        [HttpGet]
        [Route("")]
        public Product[] GetProducts()
        {
            return _productService.GetProducts();
        }

        [HttpGet]
        [Route("{id}")]
        public Product GetProduct(Guid id)
        {
            return _productService.GetProduct(id);
        }


        [HttpPost]
        [Route("{id}")]
        public StartProcessResponse SelectProduct(Guid id)
        {
            var product = _productService.GetProduct(id);
            return _processService.StartProcess(product);
        }
    }
}
