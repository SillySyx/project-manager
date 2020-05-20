using System;
using System.Reflection;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.Generic;
using ProjectManager.Services;

namespace ProjectManager.Components.MVVM
{
    public class ViewModelBase : ObservableObject
    {
        protected ILanguageService LanguageService;

        public ViewModelBase(ILanguageService languageService)
        {
            LanguageService = languageService;
        }

        #region Localization

        private Dictionary<string, string> _language { get; set; }
        public Dictionary<string, string> Language
        {
            get { return _language; }
            set { SetValue((Language) => _language, value); }
        }

        protected virtual void LoadLanguage(params string[] resourceNames)
        {
            Language = LanguageService.LoadLanguage(resourceNames);
        }

        #endregion // Localization
    }
}
