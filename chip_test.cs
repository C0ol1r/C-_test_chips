using System;
using System.Linq;
class MainClass
{
    public static void Main(string[] args)
    {
        int[][] setsOfChips = new int[][] {
            new int[] { 6, 2, 4, 10, 3 },
            new int[] { 1, 5, 9, 10, 5 },
            new int[] { 1, 2, 3 },
            new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            new int[] { 0, 1, 1, 1, 2, 1, 1, 1, 1, 1 },
            new int[] { 0, 10,0 ,8, 3, 10, 7, 0, 9, 3 },
            new int[] { 13, 8, 28, 21, 30, 6, 13, 27, 23, 1}
        };

        foreach (int[] chips in setsOfChips)
        {
            foreach (int i in chips)
            {
                Console.Write(i);
            }
            Console.WriteLine();
            Console.WriteLine("Fewest number of moves:" + MinMovesToEqualize(chips));
        }
    }

    // Функция для нахождения минимального количества перемещений фишек
    static int MinMovesToEqualize(int[] chips)
    {
        List<int> indices_small = new List<int>();
        List<int> indices_up = new List<int>();
        List<int> way = new List<int>();
        int moves = 0;

        while (!CheckAllEqual(chips))
        {
            //foreach (int i in chips)
            //{
            //  Console.Write(i);
            //}
            //Console.WriteLine();
            indices_small = FindIndicesWithLessThanAverage(chips);
            indices_up = FindIndicesWithUpThanAverage(chips);
            way = FindTheMostWantedMove(indices_small, indices_up, chips);
            Move(way, chips);
            moves++;
        }

        return moves;
    }

    static List<int> FindTheMostWantedMove(List<int> indices_small, List<int> indices_up, int[] chips)
    {
        List<List<int>> listOfLists = new List<List<int>>();
        foreach (int minIndex in indices_small)
        {
            foreach (int maxIndex in indices_up)
            {
                listOfLists.Add(CalculateDistance(minIndex, maxIndex, chips.Length));
                //Console.WriteLine("" + maxIndex + "" + minIndex);
            }
        }

        return [listOfLists[0][0], listOfLists[0][2]];
    }
    static int Neighbour(int L, int index)
    {
        if (index >= L)
        {
            return (index % L);
        }
        else if (index < 0)
        {
            return (L + index);
        }
        else return index;

    }
    static bool CheckAllEqual(int[] array)
    {
        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] != array[0])
            {
                return false;
            }
        }
        return true;
    }
    static List<int> FindIndicesWithLessThanAverage(int[] chips)
    {
        int n = chips.Length;
        int totalChips = chips.Sum();
        int average = totalChips / n;

        List<int> indices = new List<int>();

        for (int i = 0; i < n; i++)
        {
            if (chips[i] < average)
            {
                indices.Add(i);
            }
        }

        return indices;
    }
    static List<int> FindIndicesWithUpThanAverage(int[] chips)
    {
        int n = chips.Length;
        int totalChips = chips.Sum();
        int average = totalChips / n;

        List<int> indices = new List<int>();

        for (int i = 0; i < n; i++)
        {
            if (chips[i] > average)
            {
                indices.Add(i);
            }
        }

        return indices;
    }
    static List<int> CalculateDistance(int minIndex, int maxIndex, int length)
    {
        int direction = 0;
        int directDistance = Math.Abs(maxIndex - minIndex);
        int wrapAroundDistance = length - directDistance;

        if (directDistance < wrapAroundDistance)
        {
            direction = (maxIndex > minIndex) ? -1 : 1;
            return [direction, directDistance, maxIndex, minIndex];
        }
        else
        {
            direction = (maxIndex > minIndex) ? 1 : -1;
            return [direction, wrapAroundDistance, maxIndex, minIndex];
        }
    }
    static int Move(List<int> way, int[] chips)
    {
        chips[Neighbour(chips.Length, way[1] + way[0])]++;
        chips[Neighbour(chips.Length, way[1])]--;
        return 0;
    }
}