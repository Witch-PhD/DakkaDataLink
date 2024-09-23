using System.Globalization;
using System.Windows.Controls;

namespace PitBoss.Validators
{
    internal class DoubleMinMaxValidator : ValidationRule
    {
        public double Min { get; set; } = Double.MinValue;
        public double Max { get; set; } = Double.MaxValue;
        public DoubleMinMaxValidator() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double val = 0.0;
            try
            {
                if (((string)value).Length > 0)
                {
                    val = double.Parse((string)value);
                    // Console.WriteLine($"DoubleMinMaxValidator(): parse success string: {value} double: {val}");
                }
            }
            catch (System.FormatException ex)
            {
                Console.WriteLine($"DoubleMinMaxValidator() exception: cultureInfo parameter: {cultureInfo.Name},  {ex.GetType()} \n {ex.Message}");
                return new ValidationResult(false, "double minmax validation error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DoubleMinMaxValidator() exception: {ex.GetType()} \n {ex.Message}");
                return new ValidationResult(false, "double minmax validation error");
            }

            if ((val < Min) || (val > Max))
            {
                Console.WriteLine($"DoubleMinMaxValidator(): {val} out of range {Min} to {Max}");
                return new ValidationResult(false, "double minmax validation error: out of range");
            }
            return ValidationResult.ValidResult;
        }
    }
}
