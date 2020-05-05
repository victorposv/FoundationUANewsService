namespace Shared.Models
{
    public interface IEmbedField
    {
        bool Inline { get; set; }
        string Name { get; set; }
        string Value { get; set; }
    }
}