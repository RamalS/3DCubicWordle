using System.Collections;
using System.Collections.Generic;

public static class ListExtensions
{
    public static T[,] ToMatrix<T>(this List<T> list, int rows, int cols)
    {
        T[,] matrix = new T[rows, cols];
        int k = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = list[k++];
            }
        }
        return matrix;
    }

    public static bool IsOrderedSequence(this IList<int> list, int n)
    {
        if (list.Count != n) return false;

        for (int i = 0; i < n; i++)
        {
            if (list[i] != i)
            {
                return false;
            }
        }
        return true;
    }
}
