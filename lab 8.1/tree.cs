﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_8._1;

namespace lab_8._1
{

    class CTree
    {
        string formula = Program.formula;
        string fRev;
        CNode rootNode;
        char[] vars;
        int[] varsValues;
        public  int varsNum { get; set; }
        double result;
        public CTree(char value,string f)
        {
            vars = new char[f.Length];
            fRev = f;
            rootNode = new CNode();
            rootNode.Insert(value,0,0,rootNode);
            int j = 0;
            for (int i = 0; i < fRev.Length; i++)
            {
                
                if (((int)fRev[i] >= 65 && (int)fRev[i] <= 90) || ((int)fRev[i] >= 97 && (int)fRev[i] <= 122)) // DIGIT
                {
                    varsNum++;
                    vars[j] = fRev[i];
                    j++;
                }
            }
        }

        public void calculate()
        {
            string infix = fRev;
            Stack<double> s = new Stack<double>();
            
            for (int i = 0; i < infix.Length; i++)
            {
                
                if (double.TryParse(infix[i].ToString(), out double n) ) // DIGIT
                {
                   
                    s.Push(double.Parse(infix[i].ToString()));
                }
                else if (Program.isOperator(infix[i]))
                {
                    double preLast;
                    double Last;
                    double fTemp = 0; ;
                    switch (infix[i])
                    {
                        case '+':
                             fTemp = s.Pop() + s.Pop();
                            break;
                        case '-':
                             preLast = s.Pop();
                             Last = s.Pop();
                            fTemp = Last - preLast;
                            break;
                        case '*':
                             fTemp = s.Pop() * s.Pop();
                            break;
                        case '/':
                             preLast = s.Pop();
                             Last = s.Pop();
                            fTemp = Last / preLast;
                            break;
                        default:
                            break;
                    }
                    s.Push(fTemp);
                }
            }
            result = s.Pop();
            Console.WriteLine($"\nRES: {result}");
            return;
        }
        public void inputValues(params int[] values)
        {
            if (values.Length!=varsNum)
            {
                Console.WriteLine($"You entered less than {varsNum} variable!");
            }else
            {
                StringBuilder str = new StringBuilder(fRev);

                int j = 0;
                for (int i = 0; i < fRev.Length; i++)
                {
                    if (((int)fRev[i] >= 65 && (int)fRev[i] <= 90) || ((int)fRev[i] >= 97 && (int)fRev[i] <= 122)) // DIGIT
                    {
                        str[i] = char.Parse(values[j].ToString());
                        j++;
                        
                    }
                }
                fRev = str.ToString();
            }
        }

        public void Join()
        {
            rootNode.joine(char value, string f);
        }
    }

    class CNode
    {
        char? head;
        int childNum;
        int level;
        static int ii;
       
        
        public CNode LeftChild { get; set; }
        public CNode RightChild { get; set; }
        public CNode Parent { get; set; }

        public void Insert(char value,int iI,int lvl,CNode pParent)
        {
            
            if (head==null)
            {
                Parent = pParent;
                head = value;
                level = lvl;
                ii = iI;
            }
            if (Program.isOperator(value))
            {
                ii++;
                lvl++;
                childNum = 2;
                LeftChild = new CNode();
                LeftChild.Insert(Program.formula[ii], ii, lvl, this);
                
                //////////////////////////////////////////////////
                lvl++;
                ii+=2;
                RightChild = new CNode();
                RightChild.Insert(Program.formula[ii], ii, lvl, this);



            }
            else if(Program.isConstant(value))
            {
                childNum = 0;
                head = value;
                level = lvl;
                ii--;

                return;
            }
            
            
        }

        public void joine()
        {
            if (RightChild == null)
            {
                RightChild.Insert();
            }
            else
            {
                RightChild.joine();
            }
        }
    }
}