using Sanet.Kniffel.DicePanel;
using Sanet.Kniffel.Models;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ViewModels
{
    public class DiceRollerModel:BaseViewModel
    {
        public ResourceModel RModel = new ResourceModel();
        #region BindingProps
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

        public string RollLabel
        {
            get
            {
                return RModel.GetString("roll");
            }
        }

        public string ClearLabel
        {
            get 
            {
                return RModel.GetString("clear");
            }
        }

        public string AboutLabel
        {
            get
            {
                return RModel.GetString("about");
            }
        }
        
        #endregion
        public void ClearResultsList()
        {
            RollResults = null;
        }
        public void OnRollEnd(DieResult dr)
        {
            //if diceroller...
            setRollerResults(dr);
                        
            
        }
        //results for diceroller
        void setRollerResults(DieResult dr)
        {
            var tl = new List<RollResult>();
            //foreach (Die d in DicePanel1.aDice)
            //    TipsProvider1.ShowText(d.Result.ToString(), Colors.Blue);
            if (dr.NumDice > 1)
                tl.Add(new RollResult(RModel) { ScoreType = KniffelScores.Total, Value = dr.Total.ToString() });
            //TipsProvider1.ShowText(DicePanel1.Result.ToString(), Colors.Red);//(string.Format("{0}D6: {1}", DicePanel1.NumDice,DicePanel1.Result), Colors.Red);
            ////if (DicePanel1.YhatzeeeFiveOfAKindScore() > 0) TipsProvider1.ShowText("FiveOfAKind",Colors.Orange);
            if (dr.NumDice > 3 && dr.NumPairs() > 1)
            {
                tl.Add(new RollResult(RModel) { ScoreType = KniffelScores.Pairs, Value = dr.NumPairs().ToString() });
                //TipsProvider1.ShowText(string.Format("{0} {1}", DicePanel1.NumPairs(), RModel.GetString("PairsLabel")), Colors.Orange);
            }
            if (dr.NumDice == 5)
            {
                if (dr.KniffelFullHouseScore() > 0)
                    tl.Add(new RollResult(RModel) { ScoreType = KniffelScores.FullHouse, Value = "" });
                //TipsProvider1.ShowText(RModel.GetString("FullHouseLabel"), Colors.Orange);
                if (dr.KniffelLargeStraightScore() > 0)
                    tl.Add(new RollResult(RModel) { ScoreType = KniffelScores.LargeStraight, Value = "" });
                //TipsProvider1.ShowText(RModel.GetString("LargeStraightLabel"), Colors.Orange);
                else
                    //{
                    if (dr.KniffelSmallStraightScore() > 0)
                        tl.Add(new RollResult(RModel) { ScoreType = KniffelScores.SmallStraight, Value = "" });
                //TipsProvider1.ShowText(RModel.GetString("SmallStraightLabel"), Colors.Orange);
            }
            //}
            for (int i = dr.NumDice; i > 2; i--)
                if (dr.KniffelOfAKindScore(i) > 1)
                {
                    tl.Add(new RollResult(RModel) { ScoreType = KniffelScores.OfAKind, Value = i.ToString() });
                    //TipsProvider1.ShowText(string.Format("{0} {1}", i, RModel.GetString("OfAKindLabel")), Colors.Orange);
                    break;
                }
            RollResults = tl;
        }
    }
}
