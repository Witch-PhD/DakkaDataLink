using System.Globalization;
using System.Windows.Controls;

namespace DakkaDataLink.Validators
{
    internal class IntMinMaxValidator : ValidationRule
    {
        public int Min { get; set; } = int.MinValue;
        public int Max { get; set; } = int.MaxValue;
        public IntMinMaxValidator() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int val = 0;
            try
            {
                if (((string)value).Length > 0)
                {
                    val = int.Parse((string)value);
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "int minmax validation error");
            }

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false, "int minmax validation error: out of range");
            }
            return ValidationResult.ValidResult;
        }
    }
}