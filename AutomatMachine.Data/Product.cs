using AutomatMachine.Common.Types;
using AutomatMachine.Data.Common;

namespace AutomatMachine.Data
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
    }
}
