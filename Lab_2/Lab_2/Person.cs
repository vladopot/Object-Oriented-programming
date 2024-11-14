using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    internal class Person
    {
        private string FirstName = "";
        private string LastName;
        private int age = 0;
        private readonly string pessel = "";

        public Person(string FirstName, string LastName, int age, string pessel)
        {
            this.age = age > 0 ? age : 0;
            this.pessel = pessel;
            this.FirstName = FirstName;
            this.LastName = LastName;

        }

        public string PrzedstawSie()
        {
            return $"Nazywam się {{FirstName}}\r\n{{LastName}} i mam {{age}} lat";
        }
    }

}
