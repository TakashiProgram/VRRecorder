using System;
using UnityEngine;

public static class NumberToCharArrayExtentions
{
    public static int ToCharsNonAlloc(this int self, char[] output, int start = 0)
    {
        int digitsNum = (self==0) ? 1 : (int)System.Math.Log10(self) + 1;
        int zero = '0';

        for (int i = digitsNum - 1; i >= 0; i--)
        {
            int digit = self % 10;
            output[start + i] = (char)(digit + zero);
            self /= 10;
        }

        return digitsNum;
    }
}