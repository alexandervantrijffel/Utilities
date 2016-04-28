using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Structura.Shared.Utilities
{
    public class ValidationResultEx
    {
        public bool IsValid { get; set; }
        public IEnumerable<ValidationResult> ValidationMessages { get; set; }

        public string ValidationMessagesToString()
        {
            if (ValidationMessages == null)
                return string.Empty;

            var sb = new StringBuilder();
            foreach (ValidationResult validationResult in ValidationMessages)
            {
                sb.Append(validationResult.ErrorMessage);
                sb.Append(" ");
            }
            return sb.ToString();
        }
    }
}