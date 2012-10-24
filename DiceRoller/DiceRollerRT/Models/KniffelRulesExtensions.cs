using Sanet.Kniffel.DicePanel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel
{
    public static class KniffelRulesExtensions
    {
        //the score for the numeric 1-6 categories in Y
        public static int GetKniffelNumberScore(this DieResult result, int number)
        {
            int iTot = 0;

            foreach (int i in result.DiceResults)
            {
                if (i == number)
                {
                    iTot += number;
                }
            }
            return iTot;

        }

        //public int YhatzeeeOfAKindScore(int NumofAKind)
        //{

        //    Die d = null;
        //    int[] iOccur = new int[7];
        //    int i = 0;

        //    foreach (Die d_loopVariable in aDice)
        //    {
        //        d = d_loopVariable;
        //        iOccur[d.Result] += 1;
        //    }

        //    for (i = 0; i <= 6; i++)
        //    {
        //        if (iOccur[i] >= NumofAKind)
        //        {
        //            return this.Result.Total;
        //        }
        //    }
        //    return 0;
        //}

        //public int YhatzeeeFiveOfAKindScore()
        //{

        //    const int SCORE = 50;

        //    Die d = null;
        //    int[] iOccur = new int[7];
        //    int i = 0;

        //    foreach (Die d_loopVariable in aDice)
        //    {
        //        d = d_loopVariable;
        //        iOccur[d.Result] += 1;
        //    }

        //    for (i = 0; i <= 6; i++)
        //    {
        //        if (iOccur[i] >= 5)
        //        {
        //            return SCORE;
        //        }
        //    }
        //    return 0;
        //}

        //public int YhatzeeeChanceScore()
        //{
        //    return this.Result.Total;
        //}

        //public void KniffelTreeInRow(ref bool Fixed, int n = 3)
        //{
        //    bool[] Fr = {
        //    false,
        //    false,
        //    false,
        //    false,
        //    false,
        //    false,
        //    false
        //};
        //    Die d = null;
        //    int[] iOccur = new int[7];
        //    int MinNum = 0;
        //    int i = 0;
        //    foreach (Die d_loopVariable in aDice)
        //    {
        //        d = d_loopVariable;
        //        iOccur[d.Result] += 1;
        //    }

        //    if (iOccur[1] >= 1 & iOccur[2] >= 1 & iOccur[3] >= 1)
        //    {
        //        MinNum = 1;

        //    }

        //    if (iOccur[2] >= 1 & iOccur[3] >= 1 & iOccur[4] >= 1)
        //    {
        //        MinNum = 2;

        //    }

        //    if (iOccur[3] >= 1 & iOccur[4] >= 1 & iOccur[5] >= 1)
        //    {
        //        MinNum = 3;

        //    }
        //    if (iOccur[4] >= 1 & iOccur[5] >= 1 & iOccur[6] >= 1)
        //    {
        //        MinNum = 4;

        //    }
        //    if (!(MinNum == 0))
        //    {
        //        Fixed = true;
        //        for (i = MinNum; i <= MinNum + n; i++)
        //        {
        //            foreach (Die d_loopVariable in aDice)
        //            {
        //                d = d_loopVariable;
        //                if (d.Result == i & i < 7)
        //                {
        //                    if (!Fr[i])
        //                    {
        //                        d.Frozen = true;
        //                        Fr[i] = true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}

        //public int YhatzeeeSmallStraightScore(bool ToFix, ref bool Fixed, int n = 3)
        //{
        //    bool[] Fr = {
        //    false,
        //    false,
        //    false,
        //    false,
        //    false,
        //    false,
        //    false
        //};
        //    const int SCORE = 30;
        //    Die d = null;
        //    int[] iOccur = new int[7];
        //    int MinNum = 0;
        //    int i = 0;

        //    foreach (Die d_loopVariable in aDice)
        //    {
        //        d = d_loopVariable;
        //        iOccur[d.Result] += 1;
        //    }

        //    if (iOccur[1] >= 1 & iOccur[2] >= 1 & iOccur[3] >= 1 & iOccur[4] >= 1)
        //    {
        //        MinNum = 1;
        //    }

        //    if (iOccur[2] >= 1 & iOccur[3] >= 1 & iOccur[4] >= 1 & iOccur[5] >= 1)
        //    {
        //        MinNum = 2;
        //    }

        //    if (iOccur[3] >= 1 & iOccur[4] >= 1 & iOccur[5] >= 1 & iOccur[6] >= 1)
        //    {
        //        MinNum = 3;

        //    }
        //    if (!(MinNum == 0))
        //    {
        //        if (ToFix)
        //        {
        //            Fixed = true;
        //            for (i = MinNum; i <= MinNum + n; i++)
        //            {
        //                foreach (Die d_loopVariable in aDice)
        //                {
        //                    d = d_loopVariable;

        //                    if (d.Result == i & i < 7)
        //                    {
        //                        if (!Fr[i])
        //                        {
        //                            d.Frozen = true;
        //                            Fr[i] = true;

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return SCORE;
        //    }
        //    return 0;
        //}

        //public int YhatzeeeLargeStraightScore()
        //{

        //    const int SCORE = 40;
        //    Die d = null;
        //    int[] iOccur = new int[7];

        //    foreach (Die d_loopVariable in aDice)
        //    {
        //        d = d_loopVariable;
        //        iOccur[d.Result] += 1;
        //    }

        //    if (iOccur[1] == 1 & iOccur[2] == 1 & iOccur[3] == 1 & iOccur[4] == 1 & iOccur[5] == 1)
        //    {
        //        return SCORE;
        //    }

        //    if (iOccur[2] == 1 & iOccur[3] == 1 & iOccur[4] == 1 & iOccur[5] == 1 & iOccur[6] == 1)
        //    {
        //        return SCORE;
        //    }
        //    return 0;
        //}

        //public int YhatzeeeFullHouseScore()
        //{

        //    const int SCORE = 25;
        //    int[] iOccur = new int[7];
        //    int i = 0;
        //    bool bPair = false;
        //    bool bTrip = false;


        //    foreach (Die d in aDice)
        //    {
        //        iOccur[d.Result] += 1;
        //    }

        //    for (i = 0; i <= 6; i++)
        //    {
        //        if (iOccur[i] == 2)
        //        {
        //            bPair = true;
        //        }
        //        else if (iOccur[i] == 3)
        //        {
        //            bTrip = true;
        //        }
        //    }

        //    if (bPair & bTrip)
        //    {
        //        return SCORE;
        //    }
        //    return 0;
        //}

        //public int NumPairs()
        //{

        //    int[] iOccur = new int[7];
        //    int i = 0;
        //    int bPair = 0;

        //    foreach (Die d in aDice)
        //    {
        //        iOccur[d.Result] += 1;
        //    }

        //    for (i = 0; i <= 6; i++)
        //    {
        //        if (iOccur[i] > 1)
        //        {
        //            bPair++;
        //        }
        //        //if (iOccur[i] > 3)
        //        //{
        //        //    bPair++;
        //        //}
        //        //if (iOccur[i] > 5)
        //        //{
        //        //    bPair++;
        //        //}
        //    }


        //    return bPair;
        //}
    }
}
