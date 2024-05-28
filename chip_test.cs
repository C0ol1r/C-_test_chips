using System;
using System.Collections.Generic;
using System.Linq;

class MainClass
{
    public static void Main(string[] args)
    {
        int[][] setsOfChips = new int[][]
        {
            new int[] { 6, 2, 4, 10, 3 },
            new int[] { 1, 5, 9, 10, 5 },
            new int[] { 1, 2, 3 },
            new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            new int[] { 0, 1, 1, 1, 2, 1, 1, 1, 1, 1 },
            new int[] { 0, 10, 0, 8, 3, 10, 7, 0, 9, 3 },
            new int[] { 6, 14, 22, 12, 6, 25, 15, 14, 29, 21, 11, 14, 25, 13, 13 },
            new int[] { 13, 8, 28, 21, 30, 6, 13, 27, 23, 1 }
        };

        foreach (int[] chips in setsOfChips)
        {
            Console.WriteLine(string.Join(", ", chips));
            Console.WriteLine("Fewest number of moves: " + MinMovesToEqualize(chips));
        }
    }

    static int MinMovesToEqualize(int[] chips)
    {
        int[] chipsClone = (int[])chips.Clone();
        int movesFirstMethod = 0;
        int movesSecondMethod = 0;

        while (!CheckAllEqual(chipsClone))
        {
            PerformMove(ref chipsClone, ref movesSecondMethod, FindTheMostWantedMoveMethod2);
        }

        while (!CheckAllEqual(chips))
        {
            PerformMove(ref chips, ref movesFirstMethod, FindTheMostWantedMove);
        }

        return Math.Min(movesFirstMethod, movesSecondMethod);
    }

    static void PerformMove(ref int[] chips, ref int moveCounter, Func<List<int>, List<int>, int[], List<int>> findMoveMethod)
    {
        List<int> indicesSmall = FindIndicesWithLessThanAverage(chips);
        List<int> indicesUp = FindIndicesWithUpThanAverage(chips);
        List<int> way = findMoveMethod(indicesSmall, indicesUp, chips);
        Move(way, chips);
        moveCounter++;
    }

    static List<int> FindTheMostWantedMove(List<int> indicesSmall, List<int> indicesUp, int[] chips)
    {
        var listOfLists = CalculateDistances(indicesSmall, indicesUp, chips);
        int maxIndex = listOfLists.Select((val, idx) => new { val, idx })
                                  .OrderByDescending(x => x.val[1])
                                  .First().idx;

        int bias = listOfLists.Count / indicesUp.Count;
        int minIndex = listOfLists.Skip(maxIndex / indicesUp.Count * indicesUp.Count)
                                  .Take(indicesUp.Count)
                                  .Select((val, idx) => new { val, idx })
                                  .OrderBy(x => x.val[1])
                                  .First().idx;

        return new List<int> { listOfLists[minIndex][0], listOfLists[minIndex][2] };
    }

    static List<int> FindTheMostWantedMoveMethod2(List<int> indicesSmall, List<int> indicesUp, int[] chips)
    {
        var listOfLists = CalculateDistances(indicesSmall, indicesUp, chips);
        return new List<int> { listOfLists[0][0], listOfLists[0][2] };
    }

    static List<List<int>> CalculateDistances(List<int> indicesSmall, List<int> indicesUp, int[] chips)
    {
        return indicesSmall.SelectMany(minIndex =>
                   indicesUp.Select(maxIndex =>
                       CalculateDistance(minIndex, maxIndex, chips.Length)))
               .ToList();
    }

    static List<int> CalculateDistance(int minIndex, int maxIndex, int length)
    {
        int directDistance = Math.Abs(maxIndex - minIndex);
        int wrapAroundDistance = length - directDistance;
        int direction = directDistance < wrapAroundDistance ? (maxIndex > minIndex ? -1 : 1) : (maxIndex > minIndex ? 1 : -1);
        return new List<int> { direction, Math.Min(directDistance, wrapAroundDistance), maxIndex, minIndex };
    }

    static int Neighbour(int length, int index)
    {
        return (index >= length) ? (index % length) : (index < 0) ? (length + index) : index;
    }

    static bool CheckAllEqual(int[] array)
    {
        return array.All(x => x == array[0]);
    }

    static List<int> FindIndicesWithLessThanAverage(int[] chips)
    {
        int average = chips.Sum() / chips.Length;
        return chips.Select((chip, index) => new { chip, index })
                    .Where(x => x.chip < average)
                    .Select(x => x.index)
                    .ToList();
    }

    static List<int> FindIndicesWithUpThanAverage(int[] chips)
    {
        int average = chips.Sum() / chips.Length;
        return chips.Select((chip, index) => new { chip, index })
                    .Where(x => x.chip > average)
                    .Select(x => x.index)
                    .ToList();
    }

    static void Move(List<int> way, int[] chips)
    {
        chips[Neighbour(chips.Length, way[1] + way[0])]++;
        chips[Neighbour(chips.Length, way[1])]--;
    }
}
