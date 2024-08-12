using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "double minmax validation error");
            }

            if ((val < Min) || (val > Max))
            {
                return new ValidationResult(false, "double minmax validation error: out of range");
            }
            return ValidationResult.ValidResult;
        }
    }
}
