﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace SegmentPlaner.Infrastructure;

internal static class EnumerableExtensions
{
    public static IOrderedEnumerable<T> OrderByAlphaNumeric<T>(this IEnumerable<T> source, Func<T, string> selector)
    {
        int max = source
            .SelectMany(i => Regex.Matches(selector(i), @"\d+").Cast<Match>().Select(m => (int?)m.Value.Length))
            .Max() ?? 0;

        return source.OrderBy(i => Regex.Replace(selector(i), @"\d+", m => m.Value.PadLeft(max, '0')));
    }
}
