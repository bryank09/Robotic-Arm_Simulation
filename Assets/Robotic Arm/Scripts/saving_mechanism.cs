using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saving_mechanism : MonoBehaviour
{
    //class Program
    //{
    //    static int[,] add(int[] newcoordinates, int[,] nums)
    //    {
    //        int rows = nums.GetLength(0);
    //        int columns = nums.GetLength(1);
    //        int[,] newNums = new int[rows + 1, columns];

    //        for (int i = 0; i < rows; i++)
    //        {
    //            for (int j = 0; j < columns; j++)
    //            {
    //                newNums[i, j] = nums[i, j];
    //            }
    //        }
    //        // display(newNums);
    //        for (int i = 0; i < columns; i++)
    //        {
    //            newNums[rows, i] = newcoordinates[i];
    //        }

    //        return newNums;
    //    }

    //    static void display(int[,] nums)
    //    {
    //        int rows = nums.GetLength(0);
    //        int columns = nums.GetLength(1);

    //        for (int i = 0; i < rows; i++)
    //        {
    //            Console.Write("{");
    //            for (int j = 0; j < columns; j++)
    //            {
    //                Console.Write("{0},", nums[i, j]);
    //            }
    //            Console.Write("}");
    //        }
    //        Console.WriteLine("");
    //    }

    //    static void display1D(int[] nums)
    //    {
    //        Console.Write("{");
    //        for (int i = 0; i < nums.Length; i++)
    //        {
    //            Console.Write("{0},", nums[i]);
    //        }
    //        Console.Write("}");
    //        Console.WriteLine("");
    //    }

    //    static int[,] removeFirst(int[,] nums, bool first = true)
    //    {
    //        int rows = nums.GetLength(0);
    //        int columns = nums.GetLength(1);
    //        int[,] newNums = new int[rows - 1, columns];

    //        if (first)
    //        {
    //            for (int i = 0; i < rows - 1; i++)
    //            {
    //                for (int j = 0; j < columns; j++)
    //                {
    //                    newNums[i, j] = nums[i + 1, j];
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < rows - 1; i++)
    //            {
    //                for (int j = 0; j < columns; j++)
    //                {
    //                    newNums[i, j] = nums[i, j];
    //                }

    //            }

    //        }
    //        return newNums;
    //    }

    //    static void Main(string[] args)
    //    {

    //        string[,] cars = {{"A", "B", "C", "D"},
    //                      {"h", "I", "J", "g"},
    //                      {"h", "I", "J", "g"}};

    //        int[,] numbers = { { 1, 2, 3 }, { 4, 5, 6 } };
    //        int[] neww = new int[] { 7, 8, 9 };
    //        numbers = add(neww, numbers);
    //        Console.WriteLine(numbers);
    //        display(numbers);
    //        numbers = removeFirst(numbers, false);
    //        display(numbers);

    //    }

    //}

}
