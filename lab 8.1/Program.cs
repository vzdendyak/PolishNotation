using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace lab_8._1
{
   internal class Program
    {
        static public string formula;
        static CTree tree,treeJoin;
        static List<char> ConvertToPref(string exp)
        {
            string infix = exp;
            string[] tokens = infix.Split(' ');

            Stack<char> s = new Stack<char>();
            List<char> outputList = new List<char>();
            try
            {
               
                int n;
                for (int i = 0; i < infix.Length; i++)
                {
                    if (int.TryParse(infix[i].ToString(), out n) || ((int)infix[i] >= 65 && (int)infix[i] <= 90) || ((int)infix[i] >= 97 && (int)infix[i] <= 122)) // DIGIT
                    {
                        outputList.Add(infix[i]);
                    }
                    else if (infix[i] == '(')
                    {
                        s.Push(infix[i]);
                    }
                    else if (infix[i] == ')')
                    {
                        while (s.Count != 0 && s.Peek() != '(')
                        {
                            outputList.Add(s.Pop());
                        }
                        s.Pop();
                    }
                    else if (isOperator(infix[i]))
                    {
                        while (s.Count != 0 && Priority(s.Peek()) >= Priority(infix[i]))
                        {
                            outputList.Add(s.Pop());
                        }
                        s.Push(infix[i]);
                    }
                }
                while (s.Count != 0)//if any operators remain in the stack, pop all & add to output list until stack is empty 
                {
                    outputList.Add(s.Pop());
                }

                outputList.Reverse();
            }
            catch (Exception)
            {

                Console.WriteLine("Your entered invalid formula!") ;
                
            }

            return outputList;

        }

        static void EnterCommand()
        {
            Console.WriteLine("\nEnter Command...");
            string expression = Console.ReadLine();
            expression = Regex.Replace(expression, @"\s+", " ");
            if (expression.EndsWith(" "))
            {
                expression = expression.Remove(expression.Length - 1);
            }
            string[] words = expression.Split(' ');
            string firstWord = words[0].ToLower();
            Console.WriteLine();
            switch (firstWord)
            {
                case "enter":
                    List<char> items = ConvertToPref(expression);
                    if (items.Count == 0)
                    {
                        Console.ReadLine();
                        return;
                    }
                    string formulaReversed = "";
                    for (int i = 0; i < items.Count; i++)
                    {
                        Console.Write("{0}", items[i]);
                        formula += items[i];
                    }
                    for (int i = items.Count - 1; i >= 0; i--)
                    {
                        formulaReversed += items[i];
                    }
                    tree = new CTree(formula[0], formulaReversed);
                    break;
                case "comp":
                    int[] val = new int[tree.varsNum];

                    for (int i = 0; i < val.Length; i++)
                    {
                        Console.WriteLine($"\nEnter var {i}");
                        val[i] = int.Parse(Console.ReadLine());
                    }
                    tree.inputValues(val);
                    tree.calculate();
                    break;
                default:
                    break;
            }
            


        }
        static void Main()
        {
            string expression = Console.ReadLine();
            List<char>items= ConvertToPref(expression);
            if (items.Count==0)
            {
                Console.ReadLine();
                return;
            }
            string formulaReversed="";
            for (int i = 0; i < items.Count; i++)
            {
                Console.Write("{0}", items[i]);
                formula += items[i];
            }
            for (int i = items.Count-1; i >= 0; i--)
            {
                formulaReversed += items[i];
            }
            tree = new CTree(formula[0],formulaReversed);

            int[] val= new int[tree.varsNum];

            for (int i = 0; i < val.Length; i++)
            {
                Console.WriteLine($"\nEnter var {i}");
                val[i] = int.Parse(Console.ReadLine());
            }
            tree.inputValues(val);
            
            tree.calculate();

            Console.ReadLine();
            
        }
        
        public static int Priority(char c)
        {
            if (c == '^')
            {
                return 3;
            }
            else if (c == '*' || c == '/')
            {
                return 2;
            }
            else if (c == '+' || c == '-')
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public static bool isOperator(char? c)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool isConstant(char c)
        {
            return (int.TryParse(c.ToString(), out int n) || ((int)c >= 65 && (int)c<= 90) || ((int)c>= 97 && (int)c <= 122));
        }
    }
}