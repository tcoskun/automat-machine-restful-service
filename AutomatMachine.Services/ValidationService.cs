using System;
using AutomatMachine.Common.Types;
using AutomatMachine.Data;

namespace AutomatMachine.Services
{
    public class ValidationService : IValidationService
    {
        public ValidationService RequiredValidation<T>(dynamic value, string parameterName)
        {
            if (value == default(T))
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} is required!");
            }

            return this;
        }
        public ValidationService NullReferenceValidation<T>(dynamic value, string parameterName)
        {
            if (value == default(T))
            {
                throw new NullReferenceException($"{parameterName} is null!");
            }

            return this;
        }

        public ValidationService ProcessStateValidation(Process process, ProcessState state)
        {
            if (process.State != state)
            {
                throw new ArgumentNullException("process", "process state is not available for this operation!");
            }

            return this;
        }

        public ValidationService LessOrEqualValidation(decimal firstArg, decimal secondArg, string parameterName)
        {
            if (firstArg > secondArg)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} is must be less than {secondArg}");
            }

            return this;
        }

        public ValidationService GreaterOrEqualValidation(decimal firstArg, decimal secondArg, string parameterName)
        {
            if (firstArg < secondArg)
            {
                throw new ArgumentNullException(parameterName, $"{parameterName} is must be greater than {secondArg}");
            }

            return this;
        }
    }

    public interface IValidationService
    {
        ValidationService RequiredValidation<T>(dynamic value, string parameterName);
        ValidationService NullReferenceValidation<T>(dynamic value, string parameterName);
        ValidationService ProcessStateValidation(Process process, ProcessState state);
        ValidationService LessOrEqualValidation(decimal firstArg, decimal secondArg, string parameterName);
        ValidationService GreaterOrEqualValidation(decimal firstArg, decimal secondArg, string parameterName);
    }
}
