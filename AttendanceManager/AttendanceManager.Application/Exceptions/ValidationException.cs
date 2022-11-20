using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace AttendanceManager.Application.Exceptions
{
    /*
     * It will be used to return to the client validation errors that have been thrown by fluent validation
     */ 
    public class ValidationException : ApplicationException
    {
        public List<string> ValdationErrors { get; set; }

        public ValidationException(ValidationResult validationResult)
        {
            ValdationErrors = new List<string>();

            foreach (var validationError in validationResult.Errors)
            {
                ValdationErrors.Add(validationError.ErrorMessage);
            }
        }
    }
}
