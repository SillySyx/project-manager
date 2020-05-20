using System.Collections.Generic;

namespace ProjectManager.Services
{
    public interface ILanguageService
    {
        Dictionary<string, string> LoadLanguage(params string[] resourceNames);

        string ParseTranslation(string value, Dictionary<string, string> translations);
    }
}
