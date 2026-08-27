namespace MyRecipeBook.Domain.Extensions;

public static class StringExtensions
{
    public static bool IsNotEmpty(this string? email)
    {
        return !string.IsNullOrWhiteSpace(email);
    }
}
