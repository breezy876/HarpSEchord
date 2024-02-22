using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public class ComboBoxItemViewModel<T> : ObservableViewModel<T>
    {
        public int Id { get; set; }

        public ComboBoxItemViewModel() : base()
        {

        }

        public ComboBoxItemViewModel(T value) : base(value)
        {
        }

        public T Value {
            get { return Model; }
            set { Model = value; }
        }
    }
}
