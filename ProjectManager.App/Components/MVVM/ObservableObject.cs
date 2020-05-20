using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectManager.Components.MVVM
{
    public class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void NotifyPropertyChanged<T>(Expression<Func<T>> expression)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs((expression.Body as MemberExpression).Member.Name));
        }

        // SetValue((TestValue) => _testValue, "new value");
        protected virtual bool SetValue<T>(Expression<Func<object, T>> expression, T newValue)
        {
            var name = expression.Parameters[0].Name;
            var function = expression.Compile();
            var value = function(null);
            if (value == null || !value.Equals(newValue))
            {
                var member = expression.Body as MemberExpression;
                if (member != null)
                {
                    var property = member.Member as PropertyInfo;
                    if (property != null)
                    {
                        property.SetValue(this, newValue);

                        NotifyPropertyChanged(name);
                        return true;
                    }

                    var field = member.Member as FieldInfo;
                    if (field != null)
                    {
                        field.SetValue(this, newValue);

                        NotifyPropertyChanged(name);
                        return true;
                    }
                }
            }
            return false;
        }

        // SetValue(() => TestValue, ref _testValue, "new value");
        protected virtual bool SetValue<T>(Expression<Func<T>> expression, ref T property, T newValue)
        {
            if (property == null || !property.Equals(newValue))
            {
                property = newValue;
                var name = (expression.Body as MemberExpression).Member.Name;
                NotifyPropertyChanged(name);
                return true;
            }
            return false;
        }

        #endregion // INotifyPropertyChanged
    }
}
