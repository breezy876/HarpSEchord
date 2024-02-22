using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public class ProgressReporter
    {
        private int total;

        private int completed;

        public ProgressReporter(int total)
        {
            this.total = total;
            this.completed = 0;
        }

        public void Update()
        {
            completed++;
        }

        public int PercentComplete => (int)Math.Round((double)(completed * 100) / total);
    }

}
