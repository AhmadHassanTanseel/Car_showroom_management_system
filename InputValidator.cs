using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoNova_Car_Showroom
{
    public static class InputValidator
    {
        public static bool IsPlaceholder(string text, params string[] placeholders)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }

            string trimmed = text.Trim();
            foreach (string placeholder in placeholders)
            {
                if (string.Equals(trimmed, placeholder, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetRequiredText(TextBox textBox, string fieldName, out string value, params string[] placeholders)
        {
            value = textBox.Text.Trim();
            if (IsPlaceholder(value, placeholders))
            {
                AppExceptionHandler.ShowWarning("Please enter a valid " + fieldName + ".");
                value = string.Empty;
                return false;
            }

            return true;
        }

        public static bool TryParseInt(TextBox textBox, string fieldName, out int result, int minValue = 1, int maxValue = int.MaxValue)
        {
            result = 0;
            if (!int.TryParse(textBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                AppExceptionHandler.ShowWarning(fieldName + " must be a whole number.");
                return false;
            }

            if (result < minValue || result > maxValue)
            {
                AppExceptionHandler.ShowWarning(fieldName + " must be between " + minValue + " and " + maxValue + ".");
                return false;
            }

            return true;
        }

        public static bool TryParseDecimal(TextBox textBox, string fieldName, out decimal result, decimal minValue = 0m)
        {
            result = 0m;
            if (!decimal.TryParse(textBox.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            {
                AppExceptionHandler.ShowWarning(fieldName + " must be a valid number.");
                return false;
            }

            if (result < minValue)
            {
                AppExceptionHandler.ShowWarning(fieldName + " cannot be negative.");
                return false;
            }

            return true;
        }

        public static bool TryParseDecimalFromDb(object dbValue, string fieldName, out decimal result)
        {
            result = 0m;
            if (dbValue == null || dbValue == DBNull.Value)
            {
                AppExceptionHandler.ShowWarning("Missing " + fieldName + " in database record.");
                return false;
            }

            if (!decimal.TryParse(dbValue.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            {
                AppExceptionHandler.ShowWarning("Invalid " + fieldName + " in database record.");
                return false;
            }

            return true;
        }

        public static bool TryParseIntFromDb(object dbValue, string fieldName, out int result)
        {
            result = 0;
            if (dbValue == null || dbValue == DBNull.Value)
            {
                AppExceptionHandler.ShowWarning("Missing " + fieldName + " in database record.");
                return false;
            }

            if (!int.TryParse(dbValue.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                AppExceptionHandler.ShowWarning("Invalid " + fieldName + " in database record.");
                return false;
            }

            return true;
        }

        public static bool TryParseOtp(TextBox textBox, out int otp)
        {
            otp = 0;
            if (!int.TryParse(textBox.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out otp))
            {
                AppExceptionHandler.ShowWarning("OTP must be a valid number.");
                return false;
            }

            return true;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return Regex.IsMatch(email.Trim(),
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        public static bool TryGetEmail(TextBox textBox, string fieldName, out string email, params string[] placeholders)
        {
            if (!TryGetRequiredText(textBox, fieldName, out email, placeholders))
            {
                return false;
            }

            if (!IsValidEmail(email))
            {
                AppExceptionHandler.ShowWarning("Please enter a valid email address.");
                email = string.Empty;
                return false;
            }

            return true;
        }
    }
}
