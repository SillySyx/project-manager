using ProjectManager.Services;
using ProjectManager.Specs.ViewModels;
using Should;
using Xunit;

namespace ProjectManager.Specs
{
    public class ViewModelBaseTests
    {
        protected TestViewModel ViewModel;

        public ViewModelBaseTests()
        {
            ViewModel = new TestViewModel(new LanguageService());
        }

        [Fact(DisplayName = "Set and get value from view model")]
        public void GetValue()
        {
            var val = "hello";

            ViewModel.TestValue = val;

            var result = ViewModel.TestValue;

            result.ShouldEqual("hello");
        }
    }
}
