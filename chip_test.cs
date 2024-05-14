using System;

class Program
{
    static void Main(string[] args)
    {
        int[][] setsOfChips = new int[][] {
            new int[] { 1, 5, 9, 10, 5 },
            new int[] { 1, 2, 3 },
            new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 }
        };

        foreach (int[] chips in setsOfChips)
        {
            int moves = BalanceChips(chips);
            Console.WriteLine("Minimum number of movements: " + moves);
            Console.WriteLine("Chips after balancing: " + string.Join(", ", chips));
        }
    }

    static int BalanceChips(int[] chips)
    {
        int totalChips = 0;
        foreach (int chip in chips)
        {
            totalChips += chip;
        }

        int averageChips = totalChips / chips.Length; // среднее количество фишек на место
        int moves = 0;
        
        // Пока не уравняли ставки
        while (!CheckAllEqual(chips))
        {
            // Индекс позиции с минимальным количеством фишек
            int minIndex = FindMinIndex(chips);  
            
            // Проверяем условия перемещения фишек
            bool shouldMoveRight = chips[neighbour(chips.Length, minIndex + 1)] + chips[neighbour(chips.Length, minIndex + 2)] > averageChips * 2;
            bool shouldMoveLeft = chips[neighbour(chips.Length, minIndex - 1)] + chips[neighbour(chips.Length, minIndex - 2)] > averageChips * 2;
            bool rightIsGreater = chips[neighbour(chips.Length, minIndex + 1)] >= chips[neighbour(chips.Length, minIndex - 1)];
            
            // Перемещаем
            if (shouldMoveRight && shouldMoveLeft && rightIsGreater)
            {
                chips[neighbour(chips.Length, minIndex + 1)]--;
                chips[minIndex]++;
                moves++;
            }
            else if (shouldMoveRight && shouldMoveLeft && !rightIsGreater)
            {
                chips[neighbour(chips.Length, minIndex - 1)]--;
                chips[minIndex]++;
                moves++;
            }
            else if (shouldMoveRight && !shouldMoveLeft)
            {
                chips[neighbour(chips.Length, minIndex + 1)]--;
                chips[minIndex]++;
                moves++;
            }
            else if (!shouldMoveRight && shouldMoveLeft)
            {
                chips[neighbour(chips.Length, minIndex - 1)]--;
                chips[minIndex]++;
                moves++;
            }
            
            // Вывод текущего состояния массива
            Console.WriteLine("Current state of affairs: " + string.Join(", ", chips));
        };

        return moves;
    }
    
    static int neighbour(int L, int index)
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
    
    static int FindMinIndex(int[] chips)
    {
        int minIndex = 0;
        int minValue = chips[0];

        for (int i = 1; i < chips.Length; i++)
        {
            if (chips[i] < minValue)
            {
                minIndex = i;
                minValue = chips[i];
            }
        }

        return minIndex;
    }
}
