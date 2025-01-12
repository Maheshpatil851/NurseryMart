using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace NurseryMart.Utility
{
    public static class Extension
    {
        public static string ToSlugCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            // Replace any non-alphanumeric characters with a hyphen
            input = Regex.Replace(input, @"[^a-zA-Z0-9]", "-");

            input = Regex.Replace(input, @"([a-z])([A-Z])", "$1-$2");

            // Remove any leading or trailing hyphens
            input = input.Trim('-');

            // Replace multiple consecutive hyphens with a single hyphen
            input = Regex.Replace(input, @"-+", "-");

            // Convert to lowercase
            return input.ToLower();
        }
        public static Expression<Func<T, bool>> AddFilter<T>(this Expression<Func<T, bool>> existingFilter, Expression<Func<T, bool>> newFilter)
        {
            if (existingFilter == null)
                return newFilter;

            var parameter = Expression.Parameter(typeof(T));

            var combined = Expression.AndAlso(
                Expression.Invoke(existingFilter, parameter),
                Expression.Invoke(newFilter, parameter)
            );

            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }
    }
}
