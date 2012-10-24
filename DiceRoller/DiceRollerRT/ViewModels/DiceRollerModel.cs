using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ViewModels
{
    public class DiceRollerModel:BaseViewModel
    {
        List<RollResult> _RollResults;
        public List<RollResult> RollResults
        {
            get
            { 
                return _RollResults; 
            }
            set
            {
                _RollResults = value;
                NotifyPropertyChanged("RollResults");
            }
        }


        public void OnRollEnd(DieResult dr)
        {
            var tl = new List<RollResult>();
            tl.Add(new RollResult { ScoreType = KniffelScores.Total, Value = dr.Total });
            tl.Add(new RollResult { ScoreType = KniffelScores.Total, Value = dr.Total });
            RollResults = tl;
            //foreach (Die d in DicePanel1.aDice)
            //    TipsProvider1.ShowText(d.Result.ToString(), Colors.Blue);
            //if (DicePanel1.NumDice > 1) TipsProvider1.ShowText(DicePanel1.Result.ToString(), Colors.Red);//(string.Format("{0}D6: {1}", DicePanel1.NumDice,DicePanel1.Result), Colors.Red);
            ////if (DicePanel1.YhatzeeeFiveOfAKindScore() > 0) TipsProvider1.ShowText("FiveOfAKind",Colors.Orange);
            //if (DicePanel1.NumDice > 3 && DicePanel1.NumPairs() > 1)
            //{
            //    TipsProvider1.ShowText(string.Format("{0} {1}", DicePanel1.NumPairs(), RModel.GetString("PairsLabel")), Colors.Orange);
            //}
            //if (DicePanel1.NumDice == 5)
            //{
            //    if (DicePanel1.YhatzeeeFullHouseScore() > 0) TipsProvider1.ShowText(RModel.GetString("FullHouseLabel"), Colors.Orange);
            //    if (DicePanel1.YhatzeeeLargeStraightScore() > 0) TipsProvider1.ShowText(RModel.GetString("LargeStraightLabel"), Colors.Orange);
            //    else
            //    {
            //        bool rb = false;
            //        if (DicePanel1.YhatzeeeSmallStraightScore(false, ref rb) > 0) TipsProvider1.ShowText(RModel.GetString("SmallStraightLabel"), Colors.Orange);
            //    }
            //}

            //for (int i = 6; i > 2; i--)
            //    if (DicePanel1.YhatzeeeOfAKindScore(i) > 1)
            //    {
            //        TipsProvider1.ShowText(string.Format("{0} {1}", i, RModel.GetString("OfAKindLabel")), Colors.Orange);
            //        break;
            //    }
        }
    }
}
