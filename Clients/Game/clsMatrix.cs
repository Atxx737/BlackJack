using System;
using System.Collections.Generic;
using System.Text;

namespace Clients.Game
{
    [Serializable]
   public  class Matrix
    {
        List<List<int>> list;
        public Matrix()
        {
            list = new List<List<int>>();
            list.Add(new List<int>());
        }
        public void Add(int i)
        {
            if (i != 1)
            {
                foreach (List<int> l in list)
                {
                    l.Add(i);
                }
            }
            else
            {
                List<List<int>> tmpList = new List<List<int>>();
                foreach (List<int> l in list)
                {
                    List<int> tmp = new List<int>();
                    foreach (int a in l)
                    {
                        tmp.Add(a);
                    }
                    tmpList.Add(tmp);
                    l.Add(i);
                }
                foreach (List<int> l in tmpList)
                {
                    l.Add(11);
                    list.Add(l);
                }
            }
        }
        public void Reduce()
        {
            List<List<int>> RemoveList = new List<List<int>>();
            foreach (List<int> l in list)
            {
                if (SumList(l) > 21)
                {
                    RemoveList.Add(l);
                }
            }
            foreach (List<int> a in RemoveList)
            {
                list.Remove(a);
            }
        }
        public void Print()
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Empty list!");
            }
            foreach (List<int> l in list)
            {
                foreach (int i in l)
                {
                    Console.Write(i);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        public int SumList(List<int> l)
        {
            int sum = 0;
            foreach (int a in l)
            {
                sum += a;
            }
            return sum;
        }
        public int GetStatus()
        {
            int i = 1;
            foreach (List<int> l in list)
            {
                int k = SumList(l);
                if (k > 21)
                {
                    i *= 2;
                }
                else
                {
                    if (k >= 16)
                    {
                        i = 0;
                    }
                    else
                    {
                        if (i > 0)
                        {
                            i *= -1;
                        }
                    }
                }
            }
            return i;
        }
        public int GetMaxSum()
        {
            if (list.Count != 0)
            {
                int max = SumList(list[0]);
                foreach (List<int> l in list)
                {
                    if (SumList(l) > max)
                    {
                        max = SumList(l);
                    }
                }
                return max;
            }
            else
            {
                return 22;
            }
        }
    }
}
