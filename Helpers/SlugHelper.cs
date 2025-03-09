using System.Text.RegularExpressions;

namespace api.Helpers;

public class SlugHelper
{
    public static string GenerateSlug(string name)
    {
        // Normalize string to separate diacritics from characters
        var slug = name.Normalize(System.Text.NormalizationForm.FormD);

        // Remove any diacritical marks
        slug = Regex.Replace(slug, @"[\p{Mn}]", "");

        // Remove all non-alphanumeric characters except periods, commas, spaces and dashes
        slug = Regex.Replace(slug, @"[^a-zA-Z0-9.,\s-]", "");

        // Replace any sequence of periods, commas or dashes with single dash
        slug = Regex.Replace(slug, @"[., ]+|-{2,}", "-");

        slug = slug.Trim('-');

        return slug.ToLower();
    }
}