using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    internal class Summarizer
    {
        private int[] Numbers;

        public Summarizer(int[] numbers)
        {
            Numbers = numbers;
        }

        public int sum()
        {
            int sum = 0;
            for (int i = 0; i < this.Numbers.Length; i++)
            {
                sum += this.Numbers[i];
            };

            return sum;
        }

        public int SumDivide3()
        {
            int sum = 0;
            for (int i = 0; i < this.Numbers.Length; i++)
            {
                if (this.Numbers[i] % 3 == 0)
                {
                    sum += this.Numbers[i];
                }
            };

            return sum;
        }

        public int IleElementów()
        {
            return this.Numbers.Length;
        }

        public void showArr()
        {
            for (int i = 0; i < this.Numbers.Length; i++)
            {
                Console.WriteLine(this.Numbers[i]);
            };
        }

        public void lowHigh(int lowIndex, int highIndex)
        {
            int from = lowIndex <= 0 ? lowIndex : 0;
            int to = highIndex < this.Numbers.Length ? highIndex: this.Numbers.Length;

            for (int i = from; i < to; i++) 
            { 
                Console.WriteLine(this.Numbers[i]);
            }
        }
    }
}
