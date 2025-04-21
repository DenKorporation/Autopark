using System.Diagnostics;

namespace Autopark.Common.Helpers;

[DebuggerStepThrough]
public static class Check
{
    public static T NotNull<T>(this T? value, string parameterName)
    {
#pragma warning disable IDE0041 // Use 'is null' check
        if (ReferenceEquals(value, null))
#pragma warning restore IDE0041 // Use 'is null' check
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw new ArgumentNullException(parameterName);
        }

        return value!;
    }
    
    public static string NotEmpty(this string? value, string parameterName)
    {
        Exception? e = null;
        if (value is null)
        {
            e = new ArgumentNullException(parameterName);
        }
        else if (string.IsNullOrWhiteSpace(value))
        {
            e = new ArgumentException("Argument is empty string", parameterName);
        }

        if (e != null)
        {
            NotEmpty(parameterName, nameof(parameterName));

            throw e;
        }

        return value!;
    }
}
