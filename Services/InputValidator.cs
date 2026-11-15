using System;
namespace RepairCenter.Services
{
    public static class InputValidator
    {
        public static string RequiredText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException(name + " is required.");
            return value.Trim();
        }
        public static string SerialNumber(string value)
        {
            value = RequiredText(value, "Serial number").ToUpperInvariant();
            if (value.Length < 3 || value.Length > 30)
                throw new ArgumentException("Serial number must contain 3 to 30 characters.");
            foreach (char c in value)
                if (!((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '-'))
                    throw new ArgumentException("Use English letters, digits and hyphens in serial numbers.");
            return value;
        }
        public static decimal NonNegative(decimal value, string name)
        {
            if (value < 0 || value > 1000000000m)
                throw new ArgumentException(name + " must be between 0 and 1,000,000,000.");
            return value;
        }
        public static DateTime ReceivedDate(DateTime value)
        {
            if (value.Date > DateTime.Today) throw new ArgumentException("Received date cannot be in the future.");
            return value.Date;
        }
    }
}
