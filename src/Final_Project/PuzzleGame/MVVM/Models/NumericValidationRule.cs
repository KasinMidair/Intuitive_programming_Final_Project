using System.Globalization;
using System.Windows.Controls;

public class NumericValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult(false, "Value cannot be empty.");
        }

        if (!int.TryParse(value.ToString(), out _))
        {
            return new ValidationResult(false, "Value must be a number.");
        }

        return ValidationResult.ValidResult;
    }
}

