using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CRM.WPF
{
    internal class FloatsValidation : ValidationRule
    {
        private float value;
        private string errorMessage;

        public float Value { get { return value; } set { this.value = value; } }
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; } }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult result = new ValidationResult(true, null);

            string inputString = (value ?? string.Empty).ToString();

            if (!Single.TryParse(inputString, out this.value) || this.value < 0)
            {
                result = new ValidationResult(false, this.ErrorMessage);
            }

            return result;
        }
    }
}
