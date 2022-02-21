using System;
using System.Collections.Generic;

public class FunctorComparer<T> : IComparer<T>
{
    public FunctorComparer( Comparison<T> comparison )
    {
        _comparison = comparison;
    }

    public int Compare( T x, T y )
    {
        return _comparison( x, y );
    }

    private Comparison<T> _comparison = null;
}

public static class ListExtension
{
    #region 定数
    // マージソートの代わりに挿入ソートを適用する要素数
    private const int insertionSortThreshold = 4;
    #endregion

    #region 要素の取得
    public static T first<T>( this List<T> list )
    {
        return list[0];
    }
    public static T last<T>( this List<T> list )
    {
        return list[list.Count - 1];
    }
    #endregion

    #region ソート
    #region 挿入ソート
    public static void insertionSort<T>( this List<T> list )
    {
        insertionSort( list, 0, list.Count, null );
    }
    public static void insertionSort<T>( this List<T> list, Comparison<T> comparison )
    {
        if( list.Count <= 1 ) return;
        IComparer<T> comparer = (comparison != null) ? new FunctorComparer<T>(comparison) : null;
        insertionSort( list, comparer );
    }
    public static void insertionSort<T>( this List<T> list, IComparer<T> comparer )
    {
        insertionSort( list, 0, list.Count, comparer );
    }
    public static void insertionSort<T>( this List<T> list, int index, int count, IComparer<T> comparer )
    {
        if( count <= 1 ) return;
        if( comparer == null )
        {
            comparer = Comparer<T>.Default;
        }

        insertionSortUnsafe( list, index, count, comparer );
    }
    private static void insertionSortUnsafe<T>( this List<T> list, int index, int count, IComparer<T> comparer )
    {
        var end = index + count;
        for( var i = index + 1; i < end; ++i )
        {
            var value = list[i];

            if( comparer.Compare( list[i - 1], value ) > 0 )
            {
                var j = i;
                do
                {
                    list[j] = list[j - 1];
                    --j;
                } while( j > index && comparer.Compare( list[j - 1], value ) > 0 );

                list[j] = value;
            }
        }
    }
    #endregion

    #region マージソート
    public static void mergeSort<T>( this List<T> list, int minUnit = insertionSortThreshold )
    {
        mergeSort( list, 0, list.Count, null, minUnit );
    }
    public static void mergeSort<T>( this List<T> list, Comparison<T> comparison, int minUnit = insertionSortThreshold )
    {
        if( list.Count <= 1 ) return;
        IComparer<T> comparer = (comparison != null) ? new FunctorComparer<T>(comparison) : null;
        mergeSort( list, comparer, minUnit );
    }
    public static void mergeSort<T>( this List<T> list, IComparer<T> comparer, int minUnit = insertionSortThreshold )
    {
        mergeSort( list, 0, list.Count, comparer, minUnit );
    }
    public static void mergeSort<T>( this List<T> list, int index, int count, IComparer<T> comparer, int minUnit = insertionSortThreshold )
    {
        if( count <= 1 ) return;
        if( comparer == null )
        {
            comparer = Comparer<T>.Default;
        }

        if( minUnit < 1 )
        {
            minUnit = 1;
        }

        // 全体要素数が少なければ挿入ソート
        if( count <= minUnit )
        {
            insertionSortUnsafe( list, index, count, comparer );
            return;
        }

        var end = index + count;
        var work = new T[count];

        // 少ない要素数の部分リストは挿入ソートを適用
        if( minUnit > 1 )
        {
            for( var i = index; i < end; i += minUnit )
            {
                var subEnd = i + minUnit;
                if( subEnd < end )
                {
                    insertionSortUnsafe( list, i, minUnit, comparer );
                }
                else
                {
                    // 末尾の部分リストはサイズが中途半端になることがある
                    var subCount = end - i;
                    if( subCount > 1 )
                    {
                        insertionSortUnsafe( list, i, subCount, comparer );
                    }
                    break;
                }
            }
        }

        // nは部分リストの長さ
        // 1, 2, 4, ... というように2倍されていく
        for( var n = minUnit; n < count; n <<= 1 )
        {
            // 長さnの部分リストに分割されている前提で、2組ずつソートしていく
            var n_x2 = n << 1;
            for( var begin1 = index; begin1 <= end - n; begin1 += n_x2 )
            {
                var begin2 = begin1 + n;

                // 前方部分リストをバッファに詰める
                list.CopyTo( begin1, work, 0, n );

                var i = begin1;

                // 前方部分リスト（バッファ）の範囲
                var i1 = 0;
                var end1 = n;

                // 後方部分リストの範囲
                var i2 = begin2;
                var end2 = i2 + n;
                if( end2 > end )
                {
                    end2 = end;
                }

                // マージ
                while( i1 < end1 && i2 < end2 )
                {
                    var value1 = work[i1];
                    var value2 = list[i2];

                    if( comparer.Compare( value1, value2 ) > 0 )
                    {
                        list[i] = value2;
                        ++i2;
                    }
                    else
                    {
                        list[i] = value1;
                        ++i1;
                    }

                    ++i;
                }

                // バッファに残った要素の追加
                while( i1 < end1 )
                {
                    list[i] = work[i1];
                    ++i1;
                    ++i;
                }
            }
        }
    }
    #endregion
    #endregion
}