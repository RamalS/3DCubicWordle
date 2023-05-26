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
}
