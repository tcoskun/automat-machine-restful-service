using AutomatMachine.Data;
using AutomatMachine.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AutomatMachine.Test
{
    public class TestBase
    {
        protected ServiceProvider ServiceProvider { get; set; }

        [SetUp]
        public void SetUp()
        {
            InitializeServices();

            var dataContext = ServiceProvider.GetService<DataContext>();
            dataContext.Database.EnsureCreated();

            var initializeService = ServiceProvider.GetService<IInitializeService>();
            initializeService.InitializeDatabase();
        }

        private void InitializeServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Data Source=automat-test.db"));
            services.AddScoped<IInitializeService, InitializeService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<IValidationService, ValidationService>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
