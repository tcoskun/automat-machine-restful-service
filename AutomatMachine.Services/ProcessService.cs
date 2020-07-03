using System;
using System.Linq;
using AutomatMachine.Common.Request;
using AutomatMachine.Common.Response;
using AutomatMachine.Common.Types;
using AutomatMachine.Data;
using Microsoft.EntityFrameworkCore;

namespace AutomatMachine.Services
{
    public class ProcessService : IProcessService
    {
        private readonly DataContext _dataContext;
        private readonly IValidationService _validationService;
        public ProcessService(DataContext dataContext, IValidationService validationService)
        {
            _dataContext = dataContext;
            _validationService = validationService;
        }

        public StartProcessResponse StartProcess(Product product)
        {
            _validationService.RequiredValidation<Product>(product, "product");

            var process = new Process
            {
                Product = product,
                State = ProcessState.ProductSelected
            };

            _dataContext.Add(process);
            _dataContext.SaveChanges();

            return new StartProcessResponse(process.Id);
        }

        public Process[] GetProcesses()
        {
            return _dataContext.Process.ToArray();
        }

        public Process GetProcess(Guid id)
        {
            _validationService.RequiredValidation<Guid>(id, "id");
            return _dataContext.Process.FirstOrDefault(p => p.Id == id);
        }

        public Process UpdateProcess(Guid id, SetProcessRequest request)
        {
            _validationService.RequiredValidation<Guid>(id, "id")
                .RequiredValidation<SetProcessRequest>(request, "request")
                .RequiredValidation<int>(request.Piece, "piece");

            var process = _dataContext.Process.Include("Product").FirstOrDefault(p => p.Id == id);
            _validationService.NullReferenceValidation<Process>(process, "process")
                .ProcessStateValidation(process, ProcessState.ProductSelected)
                .LessOrEqualValidation(request.Piece, process.Product.Stock, "productPiece");

            process.ProductPiece = request.Piece;

            if (process.Product.Type == ProductType.HotDrink)
            {
                process.NumberOfSuger = request.NumberOfSugar;
            }

            process.State = ProcessState.PieceSelected;

            _dataContext.SaveChanges();

            return process;
        }

        public PaymentResponse Payment(Guid id, PaymentRequest request)
        {
            _validationService.RequiredValidation<Guid>(id, "id")
                .RequiredValidation<PaymentRequest>(request, "request")
                .RequiredValidation<PaymentType>(request.PaymentType, "paymentType");
            var isCash = request.PaymentType == PaymentType.Banknotes || request.PaymentType == PaymentType.Coin;

            if (isCash)
            {
                _validationService.RequiredValidation<decimal>(request.Amount, "amount");
            }

            var process = _dataContext.Process.Include("Product").FirstOrDefault(p => p.Id == id);
            _validationService.NullReferenceValidation<Process>(process, "process")
                .ProcessStateValidation(process, ProcessState.PieceSelected);

            var product = process.Product;
            var totalAmount = product.Price * process.ProductPiece;
            _validationService.NullReferenceValidation<int>(product.Stock, "stock")
                .LessOrEqualValidation(process.ProductPiece, product.Stock, "productPiece");
            decimal returnAmount = 0;

            if (isCash)
            {
                _validationService.GreaterOrEqualValidation(request.Amount, totalAmount, "amount"); 
                returnAmount = request.Amount - totalAmount;
            }

            process.State = ProcessState.SuccessPayment;
            process.PaymentType = request.PaymentType;
            product.Stock -= 1;
            _dataContext.SaveChanges();

            return  new PaymentResponse(product.Name, process.ProductPiece, process.PaymentType, returnAmount);
        }
    }

    public interface IProcessService
    {
        StartProcessResponse StartProcess(Product product);
        Process[] GetProcesses();
        Process GetProcess(Guid id);
        Process UpdateProcess(Guid id, SetProcessRequest request);
        PaymentResponse Payment(Guid id, PaymentRequest request);
    }
}
