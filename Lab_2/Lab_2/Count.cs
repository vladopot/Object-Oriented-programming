using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    internal class Count
    {
        private int value;

        public Count(int value)
        {
            this.value = value;
        }

        public void add(int value)
        {
            this.value += value;
        }

        public void show()
        {
            Console.WriteLine(this.value);
        }
    }
}
