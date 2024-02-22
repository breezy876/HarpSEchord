using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hscm.UI
{
    public class VisibileBinding : Freezable
    {
        public static readonly DependencyProperty DataProperty =
           DependencyProperty.Register("Visible", typeof(object),
              typeof(VisibileBinding));

        public object Visible
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new VisibileBinding();
        }

        #endregion
    }
}
