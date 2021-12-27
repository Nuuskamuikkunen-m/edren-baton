using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDIContainer
{
    public class A : IA
    {
        public A(IB b) { }
        public void classA()
        {
            Console.WriteLine("classA");
        }
    }
}
