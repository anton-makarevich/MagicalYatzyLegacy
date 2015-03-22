using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    /// <summary>
    /// Attempt to create more realistic random results
    /// not sure if it good enough
    /// </summary>
    public static class DiceRandomGenerator
    {
        static Random rnd = new Random();

        public static int GetNextDiceResult(int [] prevresults)
        {
            //we will select result fromthis list
            List<int> initValues = new List<int> { 1,2,3,4,5,6};
            //here we have previous results
            List<int> prevValues = prevresults.Where(f => f != 0).ToList();
            var c=getCount(prevValues);
            if (c < 3)
                for (int i = 0; i < prevValues.Count; i++)
                {
                    var r = rnd.Next(6 + prevValues.Count);//the less number in brackets - the higher probability of same dices
                    if (initValues.Contains(r) && !prevValues.Contains(r))
                    {
                        //LogManager.Log(LogLevel.Message, "", "removing: {0} - {1} ", string.Join(", ", prevValues), r);
                        initValues.Remove(r);
                    }
                }
            else
            {
                for (int i = 0; i < prevValues.Count; i++)
                {
                    var r = rnd.Next(6 + prevValues.Count);//the less number in brackets - the higher probability of same dices
                    if (initValues.Contains(r) && prevValues.Contains(r))
                    {
                        //LogManager.Log(LogLevel.Message, "", "removing: {0} - {1} ", string.Join(", ", prevValues), r);
                        initValues.Remove(r);
                    }
                }
            }

            var j = rnd.Next(initValues.Count);
            //LogManager.Log(LogLevel.Message, "", "returning: {0} - {1}->{2}", string.Join(", ", initValues), j, initValues[j]);
            return initValues[j];
        }

        static int getCount(List<int> list)
        {
            int res = 1;
            if (list.Count == 0)
                return 0;
            else if (list.Count==1)
                return 1;
            else     
            {
                var sortedList = list.OrderBy(f=>f).ToList();
                for (int i = 1; i < sortedList.Count; i++)
                {
                    if (sortedList[i] != sortedList[i - 1])
                        res++;
                }
            }
            return res;
        }
    }
}
