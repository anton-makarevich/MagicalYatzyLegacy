using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Models
{
    public static class EncriptionExtensions
    {
        public static string Decrypt(this string StringToDecrypt, int key)
        {
            StringBuilder sbDecr = new StringBuilder();
            int dblCountLength = 0;
            int intLengthChar = 0;
            string strCurrentChar = null;
            double dblCurrentChar = 0;
            int intCountChar = 0;
            int intRandomSeed = 0;
            int intBeforeMulti = 0;
            int intAfterMulti = 0;
            int intSubNinetyNine = 0;
            int intInverseAsc = 0;
            int iTK = 126 - key;
            try
            {
                if (StringToDecrypt == null)
                    return string.Empty;
                for (dblCountLength = 1; dblCountLength <= StringToDecrypt.Length; dblCountLength++)
                {
                    intLengthChar = Convert.ToInt32(StringToDecrypt.Substring(dblCountLength - 1, 1));
                    strCurrentChar = StringToDecrypt.Substring(dblCountLength, intLengthChar);
                    dblCurrentChar = 0;
                    for (intCountChar = strCurrentChar.Length; intCountChar >= 1; intCountChar += -1)
                    {
                        int powermod = (int)(Math.Pow(iTK, (strCurrentChar.Length - intCountChar)));
                        var charcode = System.Convert.ToInt32(strCurrentChar[intCountChar - 1]);
                        dblCurrentChar = dblCurrentChar + (charcode - key) * powermod;
                    }

                    intRandomSeed = Convert.ToInt32(dblCurrentChar.ToString().Substring(2, 2));
                    intBeforeMulti = Convert.ToInt32(dblCurrentChar.ToString().Substring(0, 2) + dblCurrentChar.ToString().Substring(4, 2));
                    intAfterMulti = intBeforeMulti / intRandomSeed;
                    intSubNinetyNine = intAfterMulti - 99;
                    intInverseAsc = 256 - intSubNinetyNine;
                    sbDecr.Append(System.Convert.ToChar(intInverseAsc));
                    dblCountLength = dblCountLength + intLengthChar;
                }
                return sbDecr.ToString();
            }
            catch (Exception ex)
            {
                return "something wrong:" + ex.Message;
            }
            finally
            {
                sbDecr = null;
            }

        }
        public static string Encrypt(this string StringToEncrypt, int key)
        {
            StringBuilder sbEncr = new StringBuilder();
            Random rand = new Random();
            int dblCountLength = 0;
            int intRandomNumber = 0;
            char strCurrentChar = '\0';
            int intAscCurrentChar = 0;
            int intInverseAsc = 0;
            int intAddNinetyNine = 0;
            int dblMultiRandom = 0;
            int dblWithRandom = 0;
            int intCountPower = 0;
            int intPower = 0;
            
            const int intLowerBounds = 10;
            const int intUpperBounds = 28;

            int iPK = 126 - key;

            for (dblCountLength = 1; dblCountLength <= StringToEncrypt.Length; dblCountLength++)
            {
                intRandomNumber = rand.Next(intLowerBounds, intUpperBounds);
                strCurrentChar = StringToEncrypt[dblCountLength - 1];
                intAscCurrentChar = System.Convert.ToInt32(strCurrentChar);
                intInverseAsc = 256 - intAscCurrentChar;
                intAddNinetyNine = intInverseAsc + 99;
                dblMultiRandom = intAddNinetyNine * intRandomNumber;
                dblWithRandom = Convert.ToInt32(dblMultiRandom.ToString().Substring(0, 2) + intRandomNumber + dblMultiRandom.ToString().Substring(2, 2));
                for (intCountPower = 0; intCountPower <= 5; intCountPower++)
                {
                    if (dblWithRandom / (Math.Pow(93, intCountPower)) >= 1)
                    {
                        intPower = intCountPower;
                    }
                    else
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                sbEncr.Append((intPower + 1).ToString());
                for (intCountPower = intPower; intCountPower >= 0; intCountPower += -1)
                {
                    int powermod = (int)Math.Pow(iPK, intCountPower);
                    int charcode = (dblWithRandom / powermod) + key;
                    dynamic tchar = System.Convert.ToChar(charcode);
                    sbEncr.Append(tchar);

                    dblWithRandom = dblWithRandom - ((charcode - key) * powermod);
                }

            }
            return sbEncr.ToString();
        }
    }
}
