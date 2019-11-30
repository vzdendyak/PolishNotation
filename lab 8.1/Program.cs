using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace lab_8._1
{
   internal class Program
    {
        static public string expression;
        static public string formula;
        static CTree tree,treeJoin;
        static List<char> ConvertToPref(string exp)
        {
            string infix = exp;
            string[] tokens = infix.Split(' ');
            for (int i = 0; i < exp.Length; i++)
            {
                if ( isValid(exp[i]))
                {
                    throw new Exception("...!");
                }
            }
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

               // outputList.Reverse();
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine("Reading formula error!") ;
                Console.ResetColor();
            }

            return outputList;

        }

        static void EnterCommand()
        {
            Console.WriteLine("\nEnter Command...");
             expression = Console.ReadLine();
            List<char> items;
            string formulaReversed;
            expression = Regex.Replace(expression, @"\s+", " ");

            if (expression.EndsWith(" "))
            {
                expression = expression.Remove(expression.Length - 1);
            }
            string[] words = expression.Split(' ');
            string firstWord = words[0].ToLower();
            Console.WriteLine();
            try
            {
                switch (firstWord)
                {
                    case "enter":
                        formula = "";
                        expression = expression.Remove(0, 6);
                        expression = expression.Replace(" ", "");

                        items = ConvertToPref(expression);
                        if (items.Count == 0)
                        {
                            Console.ReadLine();
                            return;
                        }
                        formulaReversed = "";
                        for (int i = 0; i < items.Count; i++)
                        {
                            formula += items[i];
                        }
                        tree = new CTree(formula[0], formula);
                        compCommand(1);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Formula accepted!");
                        Console.ResetColor();
                        
                        EnterCommand();
                        break;
                    case "comp":
                        compCommand();
                        break;
                    case "vars":
                        tree.printVars();
                        EnterCommand();
                        break;
                    case "join":
                        string tempFormula = formula;
                        formula = "";
                        expression = expression.Remove(0, 4);
                        items = ConvertToPref(expression);
                        if (items.Count == 0)
                        {
                            Console.ReadLine();
                            return;
                        }
                        formulaReversed = "";
                        for (int i = 0; i < items.Count; i++)
                        {
                            Console.Write("{0}", items[i]);
                            formula += items[i];
                        }
                        for (int i = items.Count - 1; i >= 0; i--)
                        {
                            formulaReversed += items[i];
                        }

                        treeJoin = new CTree(formula[0], formulaReversed);
                        tree.Join(treeJoin);
                        formula = tempFormula + formula;
                        EnterCommand();
                        break;
                    case "print":
                        Console.WriteLine(formula);
                        EnterCommand();
                        break;
                    case "quit":
                        return;
                    case "\\help":
                        commandList();
                        EnterCommand();
                        break;
                    case "\\clear":
                        Console.Clear();
                        EnterCommand();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unknown command! Try again.");


                        Console.ResetColor();
                        EnterCommand();
                        break;

                        
                }
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nTry again!");
                Console.ResetColor();
                EnterCommand();
            }
           
            


        }
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Write \\help to see the command list\nWrite \\clear to clear the console\nWrite \\quit to exit");
            Console.ResetColor();
            EnterCommand();
            Console.ReadLine();
            
        }

        public static void commandList()
        {
            Console.Clear();
            string help = "* enter <formula> - Enter a new formula. \n" +    
                "* vars - list  the variables of formula currently stored by the program\n" +
                "* print - prints the currently entered tree in prefix form \n" +
                "* comp  - Enter the variables and calculation of the value of the entered formula.\n"+
                "* join <formula> - execution attempts to create a tree based on the given oneexpression. ";
            Console.WriteLine(help);
        }
        public static void compCommand(int state = 0)
        {
            int[] val = new int[tree.varsNum];

            for (int i = 0; i < val.Length; i++)
            {
                if (state==1)
                {
                    val[i] = 1;
                    continue;
                }
                Console.WriteLine($"\nEnter var {i}");
                val[i] = int.Parse(Console.ReadLine());
            }
            tree.inputValues(val);
            if (state==0)
            {
                Console.WriteLine($"\nResult: {tree.calculate()}");
                EnterCommand();

            }
            tree.calculate();

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
        public static bool isValid(char? c)
        {
            return !(int.TryParse(c.ToString(), out int n) || ((int)c >= 65 && (int)c <= 90) || ((int)c >= 97 && (int)c <= 122) || isOperator(c));
        }
        public static bool isConstant(char c)
        {
            return (int.TryParse(c.ToString(), out int n) || ((int)c >= 65 && (int)c<= 90) || ((int)c>= 97 && (int)c <= 122));
        }
    }
}