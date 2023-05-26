using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringExtensions
{
    public static IEnumerable<int> GetAllIndexes(string str, string searchstring)
    {
        int minIndex = str.IndexOf(searchstring);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
        }
    }
}
