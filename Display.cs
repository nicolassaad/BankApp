using System;

namespace D
{
    class Display
    {
        public void DisplayInt(int n) 
        {
            Console.WriteLine(n);
        }

        public void DisplayDouble(double d)
        {
            Console.WriteLine(d);
        }

        public void DisplayString(string s)
        {
            Console.WriteLine(s);
        }

        public void ClearDisplay()
        {
            Console.Clear();
        }
    }
}