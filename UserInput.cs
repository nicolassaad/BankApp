using System;
using System.Linq;

namespace UI
{
    class UserInput
    {
        public int ReadInt(int i)
        {
            i = Convert.ToInt32(Console.ReadLine());
            return i; 
        }

        public double ReadDouble(double d)
        {
            d = Convert.ToDouble(Console.ReadLine());
            return d;
        }

        public string ReadString(string s)
        {
            s = Console.ReadLine();
            return s;
        }

        public char ReadKey(char c)
        {
            c = Console.ReadKey().KeyChar;
            return c;
        }

        public int WriteInt(int i)
        {
            // Access DB and store value 
            return i;
        }

        public double WriteDouble(double d)
        {
            // Access DB and store value
            return d;
        }

        public string WriteString(string s)
        {
            // Access DB and store value
            return s;
        }

        public char WriteChar(char c)
        {
            // Access DB and store value here
            return c;
        }
    }
}