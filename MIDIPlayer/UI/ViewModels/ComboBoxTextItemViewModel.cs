using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hscm.UI
{
    public class ComboBoxTextItemViewModel 
    {
        public ComboBoxTextItemViewModel()
        {

        }

        public ComboBoxTextItemViewModel(int id, string value)
        {
            this.Id = id;
            this.Value = value;
        }

        public int Id { get; set; }
        public string Value { get; set; }
    }
}
