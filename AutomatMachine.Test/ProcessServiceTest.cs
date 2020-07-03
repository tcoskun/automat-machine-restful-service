using System;
using System.Linq;
using AutomatMachine.Common.Request;
using AutomatMachine.Common.Types;
using AutomatMachine.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AutomatMachine.Test
{
    public class ProcessServiceTest : TestBase
    {
        [Test]
        public void startProcess_product_is_required()
        {
            var processService = ServiceProvider.GetService<IProcessService>();
            Assert.Throws<ArgumentNullException>(() => processService.StartProcess(null));
        }

        [Test]
        public void startProcess_create_a_new_process()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First();
            var result = processService.StartProcess(product);

            Assert.IsNotNull(result);

            var process = processService.GetProcess(result.ProcessId);

            Assert.IsNotNull(process);
            Assert.AreEqual(result.ProcessId, process.Id);
        }

        [Test]
        public void getProcesses_gets_processes_if_exist()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First();
            processService.StartProcess(product);
            var processes = processService.GetProcesses();

            Assert.Greater(processes.Length, 1);
        }

        [Test]
        public void updateProcess_id_is_required()
        {
            var processService = ServiceProvider.GetService<IProcessService>();
            Assert.Throws<ArgumentNullException>(() => processService.UpdateProcess(Guid.Empty, new SetProcessRequest(1,0)));
        }

        [Test]
        public void updateProcess_piece_is_required()
        {
            var processService = ServiceProvider.GetService<IProcessService>();
            Assert.Throws<ArgumentNullException>(() => processService.UpdateProcess(Guid.NewGuid(), new SetProcessRequest(0, 0)));
        }

        [Test]
        public void updateProcess_updates_piece_and_numberOfSugar()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First(p => p.Type == ProductType.HotDrink);
            var processResult = processService.StartProcess(product);
            var piece = 1;
            var numberOfSugar = 2;
            var request = new SetProcessRequest(piece, numberOfSugar);
            processService.UpdateProcess(processResult.ProcessId, request);
            var process = processService.GetProcess(processResult.ProcessId);
            
            Assert.AreEqual(piece, process.ProductPiece);
            Assert.AreEqual(numberOfSugar, process.NumberOfSuger);
        }

        [Test]
        public void updateProcess_piece_must_be_less_than_stock()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First();
            var processResult = processService.StartProcess(product);
            var setProcessRequest = new SetProcessRequest(product.Stock + 1, 0);

            Assert.Throws<ArgumentNullException>(() => processService.UpdateProcess(processResult.ProcessId, setProcessRequest));
        }

        [Test]
        public void payment_id_is_required()
        {
            var processService = ServiceProvider.GetService<IProcessService>();
            Assert.Throws<ArgumentNullException>(() => processService.Payment(Guid.Empty, new PaymentRequest(PaymentType.Banknotes, 1)));
        }

        [Test]
        public void payment_paymentType_is_required()
        {
            var processService = ServiceProvider.GetService<IProcessService>();
            Assert.Throws<ArgumentNullException>(() => processService.Payment(Guid.NewGuid(), new PaymentRequest(default(PaymentType), 0)));
        }

        [Test]
        public void payment_if_paymentType_is_cash_then_amount_is_required()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First(p => p.Type == ProductType.HotDrink);
            var processResult = processService.StartProcess(product);
            var piece = 1;
            var numberOfSugar = 2;
            var setProcessRequest = new SetProcessRequest(piece, numberOfSugar);
            processService.UpdateProcess(processResult.ProcessId, setProcessRequest);
            var paymentRequest = new PaymentRequest(PaymentType.Banknotes, 0);

            Assert.Throws<ArgumentNullException>(() => processService.Payment(processResult.ProcessId, paymentRequest));
        }

        [Test]
        public void payment_if_paymentType_is_cash_and_amount_less_than_price()
        {

            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First(p => p.Type == ProductType.HotDrink);
            var processResult = processService.StartProcess(product);
            var piece = 1;
            var numberOfSugar = 2;
            var setProcessRequest = new SetProcessRequest(piece, numberOfSugar);
            processService.UpdateProcess(processResult.ProcessId, setProcessRequest);
            var paymentRequest = new PaymentRequest(PaymentType.Banknotes, product.Price -1);
            
            Assert.Throws<ArgumentNullException>(() => processService.Payment(processResult.ProcessId, paymentRequest));
        }

        [Test]
        public void payment_process_is_finished_and_stock_is_updated()
        {
            var productService = ServiceProvider.GetService<IProductService>();
            var processService = ServiceProvider.GetService<IProcessService>();
            var products = productService.GetProducts();
            var product = products.First();
            var firstStock = product.Stock;
            var processResult = processService.StartProcess(product);
            var piece = 1;
            var numberOfSugar = 2;
            var setProcessRequest = new SetProcessRequest(piece, numberOfSugar);
            processService.UpdateProcess(processResult.ProcessId, setProcessRequest);
            var paymentRequest = new PaymentRequest(PaymentType.CreditCard, 0);
            processService.Payment(processResult.ProcessId, paymentRequest);
            var updatedProduct = productService.GetProduct(product.Id);
            var secondStock = updatedProduct.Stock;
            var process = processService.GetProcess(processResult.ProcessId);

            Assert.Greater(firstStock, secondStock);
            Assert.AreEqual(ProcessState.SuccessPayment, process.State);
        }
    }
}
