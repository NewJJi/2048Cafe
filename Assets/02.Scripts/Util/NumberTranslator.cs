using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumberTranslator
{
   public static string FormatNumber(int num)
   {
        if (num >= 1_000_000_000)
        {
            return (num / 1000_000_000.0).ToString("0.##") + "B";
        }
        else if (num >= 1_000_000)
        {
            return (num / 1_000_000.0).ToString("0.##") + "M";
        }
        else if (num >= 1000)
        {
            return (num / 1000.0).ToString("0.##") + "K";
        }
        else
        {
            return num.ToString();
        }
   }
}
