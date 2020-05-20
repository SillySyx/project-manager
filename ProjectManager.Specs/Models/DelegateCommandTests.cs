using Should;
using Xunit;

namespace ProjectManager.Components.MVVM
{
    public class DelegateCommandTests
    {
        [Fact(DisplayName = "Can delegate command execute without parameters")]
        public void CanDelegateCommandExecuteWithoutParameters()
        {
            var result = "";

            var command = new DelegateCommand(() => { result = "done"; }, () => { return true; });
            command.CanExecute().ShouldBeTrue();
            command.Execute();

            result.ShouldEqual("done");
        }

        [Fact(DisplayName = "Can delegate command execute with parameters")]
        public void CanDelegateCommandExecuteWithParameters()
        {
            var result = "hello";

            var command = new DelegateCommand((p) => { result = p as string; }, (p) => { return p is string; });
            command.CanExecute(result).ShouldBeTrue();
            command.Execute("done");

            result.ShouldEqual("done");
        }
    }
}
