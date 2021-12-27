using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDIContainer
{
    public class B : IB
    {
        public B(IA a) { }
        public void classB()
        {
            Console.WriteLine("classB");
        }
    }
}
