using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab_8._1;

namespace lab_8._1
{

    class CTree
    {
        string fOrig = Program.expression+" ";
        string formula = Program.formula;
        string fRev;
        CNode rootNode;
        char[] vars;
        Dictionary<int, double> constants;
        Dictionary<int,double> varsValues;
        public  int varsNum { get; set; }
        double result;
        public CTree(char value,string f)
        {
            constants = new Dictionary<int, double>();
            vars = new char[f.Length];
            fRev = f;
            rootNode = new CNode();
            rootNode.Insert(value,0,0,rootNode);
            int j = 0;
            string const1="";
            for (int i = 0; i < fOrig.Length; i++)
            {
                
                if (((int)fOrig[i] >= 65 && (int)fOrig[i] <= 90) || ((int)fOrig[i] >= 97 && (int)fOrig[i] <= 122)) // DIGIT
                {
                    varsNum++;
                    vars[j] = fOrig[i];
                    j++;
                    continue;
                }

                if (int.TryParse(fOrig[i].ToString(),out int n))
                {
                    int j1 = i;
                    try
                    {
                        while (!Program.isOperator(fOrig[i]) && (fOrig[i] != ' '))
                        {
                            if (int.TryParse(fOrig[i].ToString(),out int N))
                            {
                                const1 += fOrig[i];
                                i++;
                            }
                            else
                            {
                                break;
                            }
                            
                        }
                    }
                    catch (Exception)
                    {

                        break;
                    }
                    //enter (a+b+(c*d))/(10*5)
                    i = j1+const1.Length-1;
                    position(const1);
                    constants.Add(position(const1), double.Parse(const1));
                    const1 = "";

                }
            }
        }

        public int position(string digit) {
           return fRev.IndexOf(digit);

        }
        public double calculate()
        {
            string infix = fRev;
            Stack<double> s = new Stack<double>();
            int j = 0;
            for (int i = 0; i < infix.Length; i++)
            {
                
                if (double.TryParse(infix[i].ToString(), out double n)) // DIGIT
                {
                    if (varsValues.ContainsKey(i) || constants.ContainsKey(i))
                    {
                        s.Push(varsValues[i]);
                        i += varsValues[i].ToString().Length - 1;
                        j++;

                       
                    }
                   
                   

                   
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
            fRev = formula;
            return result;
        }
        public void inputValues(params int[] values)
        {
            varsValues = new Dictionary<int, double>();
            if (values.Length!=varsNum)
            {
                Console.WriteLine($"You entered less than {varsNum} variable!");
            }else
            {
                StringBuilder str = new StringBuilder(fRev);

                int j = 0;
                int numAdds = 0;
                for (int i = 0; i < str.Length; i++)
                {
                    if (((int)str[i] >= 65 && (int)str[i] <= 90) || ((int)str[i] >= 97 && (int)str[i] <= 122)) // DIGIT
                    {
                        str.Replace(str[i].ToString(), values[j].ToString()) ;
                        varsValues.Add(i, values[j]);

                        i += values[j].ToString().Length -1;
                        numAdds+= values[j].ToString().Length - 1;
                        //   str[i] = char.Parse(values[j].ToString());
                        j++;
                    }
                    else if (int.TryParse(str[i].ToString(), out int n))
                    {
                        string const1="";
                        int j1 = i;
                        int k = 0;
                        while (!Program.isOperator(str[i]) && constants.ContainsKey(i-numAdds-k))
                        {
                            const1 += str[i];
                            if (const1.Length == constants[i - numAdds-k].ToString().Length)
                            {
                                break;
                            }
                            k++;

                            i++;
                           
                        }
                        if (constants.ContainsValue(double.Parse(const1)))
                        {
                            varsValues.Add(j1, double.Parse(const1));
                        }
                        
                    }
                }
                fRev = str.ToString();
            }
        }

        public void printVars()
        {
            
            for (int i = 0; i < vars.Length; i++)
            {
                if (Program.isConstant( vars[i]))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{i+1} - {vars[i]}");
                    Console.ResetColor();
                   
                }
                
            }
        }
        public void Join(CTree temp)
        {
            rootNode.joine(temp.rootNode);
            this.formula += temp.formula;
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

        public void joine(CNode temp)
        {
            if (RightChild == null)
            {
                RightChild = temp;
            }
            else
            {
                RightChild.joine(temp);
            }
        }
    }
}
