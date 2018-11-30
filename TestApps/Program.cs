using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApps
{
    public class Program
    {
        public static void Main(string[] args)
        {
           


            start();
          
        }

        static void start()
        {
            try
            {
                int i = 0;
                while (true)
                {
                    i++;
                    f(i);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception)
            {

            }
           
        }

        public static void f(int a)
        {
            Console.WriteLine(a);
        }
    }
}
