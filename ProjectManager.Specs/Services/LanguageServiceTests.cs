using Should;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProjectManager.Services
{
    public class LanguageServiceTests
    {
        protected ILanguageService Service;

        public LanguageServiceTests()
        {
            Service = new LanguageService();
        }

        [Fact(DisplayName = "Can load language")]
        public void CanLoadLanguage()
        {
            var name = "Test";

            var result = Service.LoadLanguage(name);

            result.ContainsKey("Test").ShouldBeTrue();
        }

        [Fact(DisplayName = "Can parse translation")]
        public void CanParseTranslation()
        {
            var translations = new Dictionary<string, string>()
            {
                { "Title", "Hello" },
                { "Name", "Krille" },
            };
            var translationToParse = "Why {Title} there {Name}";

            var result = Service.ParseTranslation(translationToParse, translations);

            result.ShouldEqual("Why Hello there Krille");
        }

        [Fact(DisplayName = "Can parse translation with modifiers")]
        public void CanParseTranslationWithModifiers()
        {
            var translations = new Dictionary<string, string>()
            {
                { "Test", "THERE" },
            };
            var translationToParse = "Hello {Test}[tolower]";

            var result = Service.ParseTranslation(translationToParse, translations);

            result.ShouldEqual("Hello there");
        }
    }
}
