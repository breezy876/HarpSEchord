using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public class ValueViewModel<T> : ObservableViewModel<T>
    {
        public ValueViewModel(T value) : base(value)
        {

        }

        public T Value
        {
            get { return Model; }
            set { Model = value; RaisePropertyChanged(); }
        }
    }
}
