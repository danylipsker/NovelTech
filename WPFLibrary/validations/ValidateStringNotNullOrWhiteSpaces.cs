using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace WPFLibrary.validations
{
    public class ValidateStringNotNullOrWhiteSpaces : ValidationRule
    {
        public ValidateStringNotNullOrWhiteSpaces()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(false,
                  $"Please enter a text.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
