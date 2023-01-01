using System.Text;
using Tools.Utility.Messages;

namespace Tools.Utility.CarFilterAndValidator
{
    public static class CarFilterFormatter
    {
        public static readonly Func<int, string> FormatMileage = (mileage) =>
        {
            string unit = "km";
            string cast = mileage.ToString();
            string res;
            if (cast.Length == 3)
            {
                res = $"{cast} {unit}";
            }
            else
            {
                res = $"{cast[0]}";
                if (cast.Length == 4)
                {
                    res += $" {cast.Substring(1)} {unit}";
                }

                if (cast.Length == 5)
                {
                    res += $"{cast[1]} {cast.Substring(2)} {unit}";
                }

                if (cast.Length == 6)
                {
                    res += $"{cast[1]}{cast[2]} {cast.Substring(3)} {unit} ";
                }
            }

            return res;
        };

        public static readonly Func<int, string> FormatPrice = (price) =>
        {
            string currency = "€";
            string casted = price.ToString();

            StringBuilder formatted = new StringBuilder();
            if (casted.Length == 4)
            {
                formatted.Append(casted[0]);
                formatted.Append(' ');
                formatted.Append(casted.Substring(1));
            }
            else
            {
                formatted.Append(casted[0]);
                formatted.Append(casted[1]);
                if (casted.Length == 6)
                    formatted.Append(casted[2]);
                formatted.Append(' ');
                formatted.Append(casted.Length == 6 ? casted.Substring(3) : casted.Substring(2));
            }

            formatted.Append($" {currency}");
            return formatted.ToString();
        };


        public static readonly Func<int, int, List<int>> InitializeYears = (start, end) =>
        {
            List<int> result = new();

            int current = start;
            while (current >= end)
            {
                result.Add(current);
                current--;
            }

            return result;
        };


        public static readonly Action<int, int, UpperLowerFilter> CompareValuesAndGenerateErrorsIfExisting =
            (lower, upper, option) =>
            {
                if (lower > upper)
                {
                    CrossPlatformMessageRenderer.RenderMessages(
                        $"The lower {option.ToString().ToLower()} bound cannot be greater than the upper one!", "Retry",
                        4);
                }

                if (lower == upper)
                {
                    CrossPlatformMessageRenderer.RenderMessages(
                        $"The {option.ToString().ToLower()} bounds cannot be equal!", "Retry", 4);
                }
            };
    }
}