using System;
namespace Sanet.Kniffel.Localization
{
    public interface IResourceModel
    {
        string CurrentLanguage { get; set; }
        string GetString(string resource);
        string GetString(string resource, string languageCode);
    }
}
