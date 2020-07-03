using System;
using System.Collections.Generic;
using System.Linq;
using AutomatMachine.Common.Types;
using AutomatMachine.Data;

namespace AutomatMachine.Services
{
    public class InitializeService : IInitializeService
    {
        private readonly DataContext _dataContext;
        private readonly Dictionary<string, int> _productDefinitionList;
        public InitializeService(DataContext dataContext)
        {
            _dataContext = dataContext;
            _productDefinitionList = new Dictionary<string, int>
            {
                {"Chocolate|Food", 5},
                {"Biscuit|Food", 5},
                {"Sandwich|Food", 5},
                {"Cake|Food", 5},
                {"Tea|HotDrink", 3},
                {"Coffee|HotDrink", 2},
                {"Cola|ColdDrink", 3},
                {"IceTea|ColdDrink", 2}
            };
        }

        public void InitializeDatabase()
        {
            AddProducts();
        }

        private void AddProducts()
        {
            if (_dataContext.Product.Any()) { return; }

            var random = new Random();
            var productList = (from productDefinition in _productDefinitionList
                               let price = random.Next(1, 10)
                               let keylist = productDefinition.Key.Split('|')
                               let name = keylist[0]
                               let descripton = keylist[1]
                               select new Product
                               {
                                   Name = name,
                                   Description = descripton,
                                   Price = price,
                                   Stock = productDefinition.Value,
                                   Type = Enum.Parse<ProductType>(descripton)
                               }).ToList();

            _dataContext.AddRange(productList);
            _dataContext.SaveChanges();
        }
    }

    public interface IInitializeService
    {
        void InitializeDatabase();
    }
}
