using Sanet.Kniffel.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public static class PlayerAIExtensions
    {
        /// <summary>
        /// Method for bot to decide where to put result
        /// </summary>
        /// <param name="player"></param>
        public static void AIDecideFill(this Player player)
        {
            //TODO
            //delay to simulate thinking...

            //this converted from my old vb kniffel game

            int i = 0;
            int j = 0;
            int[] n = new int[7];

            //сколько кубикоков с каждым значением
            for (i = 1; i <= 6; i++)
            {
                n[i] = player.Game.LastDiceResult.KniffelNumberScore(i) / i;
            }
            //проверка на книффель
            var result=player.GetResultForScore(KniffelScores.Kniffel);
            if (result!=null && !result.HasValue && result.PossibleValue == result.MaxValue)
            {
                player.Game.ApplyScore(result);
                return;
            }

            //проверка на фн
            result = player.GetResultForScore(KniffelScores.FullHouse);
            if (result != null && !result.HasValue && result.PossibleValue == result.MaxValue)
            {
                player.Game.ApplyScore(result);
                return;
            }
            
            //six's if at least 4 and no value 
            result = player.GetResultForScore(KniffelScores.Sixs);
            if (result != null && !result.HasValue && n[6] == 4)
            {
                player.Game.ApplyScore(result);
                return;
            }
            
            //checking poker hands LS, SS, FH
            for (i = 11; i >= 9; i += -1)
            {
                result = player.GetResultForScore((KniffelScores)i);
                if (result != null && !result.HasValue && result.PossibleValue >= result.MinValue())
                {
                    player.Game.ApplyScore(result);
                    return;
                }
            }
            
            //4 and 3 in a row
            for (i = 8; i >= 7; i += -1)
            {
                result = player.GetResultForScore((KniffelScores)i);
                if (result != null && !result.HasValue && result.PossibleValue >= result.MinValue() - (int)((player.Game.Move - 1) / 2))
                {
                    player.Game.ApplyScore(result);
                    return;
                }
            }
            //numerics 
            for (j = 5; j >= 1; j += -1)
            {
                //Step -1
                for (i = 1; i <= 6; i++)
                {
                    result = player.GetResultForScore((KniffelScores)i);
                    if (result != null && !result.HasValue && n[i] >= j)
                    {
                        player.Game.ApplyScore(result);
                        return;
                    }
                }
            }
            //chance
            result = player.GetResultForScore(KniffelScores.Total);
            if (result != null && !result.HasValue && result.PossibleValue >= result.MinValue() - (int)((player.Game.Move - 1) / 2))
            {
                player.Game.ApplyScore(result);
                return;
            }
            //once again 4 and 3in row
            for (i = 8; i >= 7; i += -1)
            {
                result = player.GetResultForScore((KniffelScores)i);
                if (result != null && !result.HasValue && result.PossibleValue >  0)
                {
                    player.Game.ApplyScore(result);
                    return;
                }
            }
            //if not filled - filling at least anything including 0
            for (i = 1; i <= 12; i++)
            {
                result = player.GetResultForScore((KniffelScores)i);
                if (result != null && !result.HasValue)
                {
                    player.Game.ApplyScore(result);
                    return;
                }
            }
            result = player.GetResultForScore(KniffelScores.Kniffel);
            if (result != null && !result.HasValue)
            {
                player.Game.ApplyScore(result);
                return;
            }
        }

        /// <summary>
        /// Method for bot to fix dices he needs
        /// </summary>
        /// <param name="player"></param>
        public static void AIFixDices(this Player player)
        {
            
            int[] n = new int[7];
            
            //сколько кубикоков с каждым значением
            for (int i = 1; i <= 6; i++)
            {
                n[i] = player.Game.LastDiceResult.KniffelNumberScore(i) / i;
            }
            RollResult result;
            //проверяем на три пятерки и шестерки
            for (int i = 6; i >= 5; i += -1)
            {
                result = player.GetResultForScore((KniffelScores)i);
                if (result != null && !result.HasValue && n[i] > 2)
                {
                    foreach (int value in n)
                        player.Game.FixDice(value, true);
                    return;
                }
            }

            //проверяем нужны ли подряд и если больше трех отмечаем
            result = player.GetResultForScore(KniffelScores.LargeStraight);
            var result2 = player.GetResultForScore(KniffelScores.SmallStraight);
            if ((result != null && !result.HasValue) || (result2 != null && !result2.HasValue))
            {
                int count=0;//count of dices in row (3 || 4)
                int first =player.Game.LastDiceResult.XInRow(ref count);//first dice in row
                if (first > 0)
                {
                    for (int i = first; i <= first + count; i++)
                    {
                        if (!player.Game.IsDiceFiexed(i))
                            player.Game.FixDice(i, true);
                    }
                    
                }
            }
            

            //прверка на FH
            result = player.GetResultForScore(KniffelScores.FullHouse);
            if (result != null && !result.HasValue)
            {
                if (player.Game.LastDiceResult.NumPairs()==2)//2 пары
                {
                    for (int j = 1; j <= 6; j++)
                    {
                        if (n[j] > 1)
                            player.Game.FixAllDices(j, true);
                    }
                    return;
                }
            }

            for (int j = 5; j >= 1; j += -1)
            {

                for (int i = 6; i >= 1; i += -1)
                {
                    //not clear with this condition
                    //if more then 2 same and  
                    if (j > 2 | player.AllNumericFilled)
                    {
                        if (!player.IsScoreFilled(KniffelScores.Kniffel) |
                            !player.IsScoreFilled(KniffelScores.ThreeOfAKind) |
                            !player.IsScoreFilled(KniffelScores.FourOfAKind) |
                            !player.IsScoreFilled((KniffelScores)i))
                        {
                            if (n[i] == j)
                            {
                                player.Game.FixAllDices(i,true);
                                return;
                            }
                        }
                    }
                    else
                    {
                        //(Scores(0) = -1 Or Scores(7) = (-1) Or Scores(8) = (-1)) And
                        if (!player.IsScoreFilled((KniffelScores)i))
                        {
                            if (n[i] == j)
                            {
                                player.Game.FixAllDices(i, true);
                                return;
                            }
                        }
                    }
                }
            }
            if (!player.IsScoreFilled(KniffelScores.Kniffel))
            {
                for (int i = 6; i >= 5; i += -1)
                {
                    player.Game.FixDice(i, true);
                }
            }
        }

        /// <summary>
        /// Method for bot to decide if he need to roll again
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool AINeedRoll(this Player player)
        {
            
            var result = player.GetResultForScore(KniffelScores.Kniffel);
            if (result != null && !result.HasValue && result.PossibleValue == result.MaxValue)
                return false;
            //If Scores(7) = -1 And Now(7) > 25 Then Return False
            //If Scores(8) = -1 And Now(8) > 25 Then Return False
            result = player.GetResultForScore(KniffelScores.FullHouse);
            if (result != null && !result.HasValue && result.PossibleValue == result.MaxValue)
                return false;
            result = player.GetResultForScore(KniffelScores.LargeStraight);
            if (result != null && !result.HasValue)
            {
                if (result.PossibleValue == result.MaxValue)
                    return false;
            }
            else
            {
                result = player.GetResultForScore(KniffelScores.SmallStraight);
                if (result != null && !result.HasValue && result.PossibleValue == result.MaxValue)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Min acceptable value mostly used by AI 
        /// Public ScoresMin() = {50, 1, 2, 3, 4, 5, 6, 22, 17, 25, 30, 40, 25}
        /// TODO-to AIExtensions
        /// </summary>
        public static int MinValue(this RollResult result)
        {
            switch (result.ScoreType)
            {
                case KniffelScores.Ones:
                    return 1;
                case KniffelScores.Twos:
                    return 2;
                case KniffelScores.Threes:
                    return 3;
                case KniffelScores.Fours:
                    return 4;
                case KniffelScores.Fives:
                    return 5;
                case KniffelScores.Sixs:
                    return 6;
                case KniffelScores.ThreeOfAKind:
                    return 22;

                case KniffelScores.FourOfAKind:
                    return 17;

                case KniffelScores.FullHouse:
                    return 25;

                case KniffelScores.SmallStraight:
                    return 30;
                case KniffelScores.LargeStraight:
                    return 40;

                case KniffelScores.Total:
                    return 25;

                case KniffelScores.Kniffel:
                    return 50;

            }
            return 0;
        }

        

    }
}
