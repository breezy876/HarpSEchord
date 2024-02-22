using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public abstract class ObservableViewModel : INotifyPropertyChanged
    {

        private Dictionary<string, PropertyChangedEventHandler> eventHandlers;

        public object Model;

        public ObservableViewModel()
        {
            eventHandlers = new Dictionary<string, PropertyChangedEventHandler>();
        }

        public ObservableViewModel(object model)
        {
            Model = model;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddPropertyChangedHandler(string propertyName, PropertyChangedEventHandler handler)
        {
            if (!this.eventHandlers.ContainsKey(propertyName))
            {
                this.eventHandlers.Add(propertyName, handler);
            }
            else
            {
                this.eventHandlers[propertyName] = handler;
            }
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (eventHandlers.ContainsKey(propertyName))
            {
                //call custom handler
                NotifyPropertyChanged(propertyName, eventHandlers[propertyName]);
            }
            else
            {
                //call default WPF handler
                NotifyPropertyChanged(propertyName, this.PropertyChanged);
            }
        }

        private void NotifyPropertyChanged(string propertyName, PropertyChangedEventHandler handler)
        {
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public abstract class ObservableViewModel<T> : ObservableViewModel
    {
        public new T Model;

        public ObservableViewModel()
        {
        }

        public ObservableViewModel(T model)
        {
            Model = model;
        }

        protected void NotifyPropertyChanged(Expression<Func<T>> propertyLambda)
        {
            var propertyName = GetPropertyName(propertyLambda);
            base.RaisePropertyChanged(propertyName);
        }

        private static string GetPropertyName(Expression<Func<T>> propertyLambda)
        {
            if (propertyLambda == null) throw new ArgumentNullException("propertyLambda");

            var me = propertyLambda.Body as MemberExpression;

            if (me == null)
            {
                throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' or '() => object.Property'");
            }

            return me.Member.Name;
        }
    }
}
