using PravaCijena.Api.Helpers;

namespace PravaCijena.Api.Models;

public class BaseNamedEntity : BaseEntity
{
    private string _name;

    public required string Name
    {
        get => _name;
        set
        {
            _name = value;
            Slug = SlugHelper.GenerateSlug(value);
        }
    }

    public string Slug { get; private set; }
}