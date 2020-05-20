using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectManager.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ProjectManager.Services
{
    public class LanguageService : ILanguageService
    {
        public Languages CurrentLanguage = Languages.Sv;

        public virtual Dictionary<string, string> LoadLanguage(params string[] resourceNames)
        {
            var translations = new Dictionary<string, string>();
            if (resourceNames.Length == 0) return translations;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies.Where(a => a.GetName().Name.StartsWith("ProjectManager.")))
            {
                foreach (var name in resourceNames)
                {
                    var resourcePath = String.Format("ProjectManager.Resources.Localization.{0}.{1}.json", CurrentLanguage.ToString(), name);
                    using (var stream = assembly.GetManifestResourceStream(resourcePath))
                    {
                        if (stream == null) continue;
                        using (var streamReader = new StreamReader(stream))
                        {
                            ParseJson(streamReader.ReadToEnd(), translations);
                        }
                    }

                    var localPath = String.Format(@"Resources\Localization\{0}\{1}.json", CurrentLanguage.ToString(), name);
                    if (File.Exists(localPath))
                    {
                        ParseJson(File.ReadAllText(localPath), translations);
                    }
                }
            }
            return translations;
        }

        public virtual void ParseJson(string data, Dictionary<string, string> translations)
        {
            var json = JObject.Parse(data);
            foreach (var translation in json.Properties())
            {
                var parsedTranslation = ParseTranslation(translation.Value.ToString(), translations);

                if (!translations.ContainsKey(translation.Name)) translations.Add(translation.Name, "");

                translations[translation.Name] = parsedTranslation;
            }
        }

        public virtual string ParseTranslation(string value, Dictionary<string, string> translations)
        {
            if (translations.Count == 0) return value;

            // find translations with modifiers
            var matches = Regex.Matches(value, @"(\{[^ \{\}]+\})(\[[^ \[\]]+\])");
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    var val = match.Groups[1].Value;
                    var name = val.StartsWith("{") && val.EndsWith("}") ? val.Substring(1, val.Length - 2) : val;
                    var modifier = match.Groups[2].Value;

                    if (translations.ContainsKey(name))
                    {
                        var translation = translations[name];
                        switch (modifier)
                        {
                            case "[tolower]": translation = translation.ToLower(); break;
                        }

                        value = value.Replace(match.Value, translation);
                    }
                }
            }

            // find translations
            // matches: Hello {name}, how are you?
            matches = Regex.Matches(value, @"(\{[^ \{\}]+\})");
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    // remove brackets
                    var name = match.Value.StartsWith("{") && match.Value.EndsWith("}") ? match.Value.Substring(1, match.Value.Length - 2) : match.Value;
                    if (translations.ContainsKey(name))
                    {
                        value = value.Replace(match.Value, translations[name]);
                    }
                }
            }

            return value;
        }
    }
}
