using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CRM.WPF
{
    public class StringLenghtValidation : ValidationRule
    {
        private int minLenght = -1;
        private string errorMessage;

        public int MinLenght { get { return minLenght; } set { minLenght = value; } }
        public string ErrorMessage { get { return errorMessage; } set { errorMessage = value; } }

        public override ValidationResult Validate(object value, CultureInfo culture)
        {
            ValidationResult result = new ValidationResult(true, null);

            string inputString = (value ?? string.Empty).ToString();
            if (inputString.Length < this.MinLenght)
            {
                result = new ValidationResult(false, this.ErrorMessage);
            }

            return result;
        }
    }
}
