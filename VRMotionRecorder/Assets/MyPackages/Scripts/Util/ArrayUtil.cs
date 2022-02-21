using System;
using System.Collections.Generic;

public static class ArrayUtil
{
    /// <summary>
    /// 配列版
    /// int[] intArray;
    /// intArray.ForEach (～); のように呼び出せる
    /// 本来は　Array.ForEach (intArray, ～);
    /// </summary>
    public static void ForEach<T>(this T[] source, Action<T> action)
    {
        Array.ForEach(source, action);
    }

    /// <summary>
    /// IEnumerable<T>版
    /// Enumerable.Repeat (0, 5).ForEach (～); のように呼び出せる
    /// 本来は　Enumerable.Repeat (0, 5).ToList ().ForEach (～); か
    /// Array.ForEach (Enumerable.Repeat (0, 5).ToArray (), ～);
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (T val in source)
        {
            action(val);
        }
    }
}