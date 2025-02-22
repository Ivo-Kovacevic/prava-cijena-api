using System.Text.RegularExpressions;

namespace api.Helpers;

public class SlugHelper
{
    public static string GenerateSlug(string name)
    {
        var slug = name.Normalize(System.Text.NormalizationForm.FormD);
        slug = Regex.Replace(slug, @"[\p{Mn}]", "");
        slug = slug.Trim().ToLower().Replace(" ", "-");

        return slug;
    }
}