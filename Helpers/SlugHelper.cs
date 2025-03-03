using System.Text.RegularExpressions;

namespace api.Helpers;

public class SlugHelper
{
    public static string GenerateSlug(string name)
    {
        // Normalize string to separate diacritics from characters
        var slug = name.Normalize(System.Text.NormalizationForm.FormD);
        
        // Remove any diacritical marks and percentage sign
        slug = Regex.Replace(slug, @"[\p{Mn}%]", "");
        
        // Replace any sequence of periods, commas or dashes with single dash
        slug = Regex.Replace(slug, @"[., ]+|-{2,}", "-");
        
        slug = slug.TrimStart('-');

        return slug.ToLower();
    }
}