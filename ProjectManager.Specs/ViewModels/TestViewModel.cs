using ProjectManager.Components.MVVM;
using ProjectManager.Services;

namespace ProjectManager.Specs.ViewModels
{
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel(ILanguageService service)
            : base(service)
        {
        }

        private string _testValue { get; set; }
        public string TestValue
        {
            get { return _testValue; }
            set { SetValue((TestValue) => _testValue, value); }
        }
    }
}
