/*
 * NOT USED ANYMORE 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimplexSolver
{
    public static Vector3 Solve(Vector3 maxFunction,
           List<Vector3> A,
           List<float> b)
    {
        List<List<float>> simplex = SetSimplex(maxFunction, A, b);

        return DoSimplex(simplex);

    }

    static List<List<float>> SetSimplex(Vector3 maxFunction,
           List<Vector3> A,
           List<float> b)
    {
        List<List<float>> simplex = new List<List<float>>();

        int numEquations = A.Count;
        int numCols = 2 + numEquations + 1 + 1;

        for (int iRow = 0; iRow < numEquations; ++iRow)
        {
            float[] row = new float[numCols];

            for (int iCol = 0; iCol < 2; ++iCol)
            {
                row[iCol] = A[iRow][iCol * 2];
            }

            row[2 + iRow] = 1;
            row[numCols - 1] = b[iRow];

            simplex.Add(new List<float>(row));

        }


        float[] lastRow = new float[numCols];
        for (int iVar = 0; iVar < 2; ++iVar)
        {
            lastRow[iVar] = 0 - maxFunction[iVar * 2];
        }

        lastRow[2 + numEquations] = 1;

        simplex.Add(new List<float>(lastRow));


        return simplex;
    }

    static bool GetPivots(List<List<float>> simplex, ref int pivotCol, ref int pivotRow, bool noSolution)
    {
        int numRows = simplex.Count;
        int numCols = simplex[0].Count;

        noSolution = false;


        float min = 0;
        for (int iCol = 0; iCol < numCols - 2; iCol++)
        {
            float value = simplex[numRows - 1][iCol];
            if (value < min)
            {
                pivotCol = iCol;
                min = value;
            }
        }


        if (min == 0)
            return false;


        float minRatio = 0.0f;
        bool first = true;
        for (int iRow = 0; iRow < numRows - 1; iRow++)
        {
            float value = simplex[iRow][pivotCol];

            if (value > 0.0f)
            {
                float colValue = simplex[iRow][numCols - 1];
                float ratio = colValue / value;


                if ((first || ratio < minRatio) && ratio >= 0.0)
                {
                    minRatio = ratio;
                    pivotRow = iRow;
                    first = false;
                }
            }
        }


        noSolution = first;
        return !noSolution;
    }


    static Vector3 DoSimplex(List<List<float>> simplex)
    {
        int pivotCol = 0, pivotRow = 0;
        int numRows = simplex.Count;
        int numCols = simplex[0].Count;


        bool noSolution = false;
        while (GetPivots(simplex, ref pivotCol, ref pivotRow, noSolution))
        {
            float pivot = simplex[pivotRow][pivotCol];

            for (int iCol = 0; iCol < numCols; iCol++)
            {
                simplex[pivotRow][iCol] /= pivot;
            }

            for (int iRow = 0; iRow < numRows; iRow++)
            {
                if (iRow != pivotRow)
                {
                    float ratio = -1 * simplex[iRow][pivotCol];
                    for (int iCol = 0; iCol < numCols; iCol++)
                    {
                        simplex[iRow][iCol] = simplex[pivotRow][iCol] * ratio + simplex[iRow][iCol];
                    }
                }
            }
        }

        if (noSolution)
        {
            return Vector3.zero;
        }

        Vector3 x = Vector3.zero;

        for (int iCol = 0; iCol < 2; iCol++)
        {
            bool isUnit = true;
            bool first = true;
            float value = float.MaxValue;
            for (int j = 0; j < numRows; j++)
            {
                if (simplex[j][iCol] == 1.0f && first)
                {
                    first = false;
                    value = simplex[j][numCols - 1];
                }
                else if (simplex[j][iCol] != 0.0f)
                {
                    isUnit = false;
                }
            }

            if (isUnit && !first)
                x[iCol * 2] = value;
            else
                x[iCol * 2] = 0.0f;
        }


        return x;
    }
}