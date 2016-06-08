using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Structura.Shared.Utilities
{
    public class DataAnnotationsValidatorEx
    {
        public static ValidationResultEx ValidateObject(object objectToValidate)
        {
            var context = new ValidationContext(objectToValidate, null, null);
            var messages = new List<ValidationResult>();
            var result = new ValidationResultEx
                {
                    IsValid = Validator.TryValidateObject(objectToValidate, context, messages),
                    ValidationMessages = messages
                };
            return result;
        }
    }
}