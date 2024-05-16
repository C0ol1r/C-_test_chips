using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    static void Main(string[] args)
    {
        int[][] setsOfChips = new int[][] {
            new int[] { 6, 2, 4, 10, 3 },
            new int[] { 1, 5, 9, 10, 5 },
            new int[] { 1, 2, 3 },
            new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 2 },
            new int[] { 0, 1, 1, 1, 2, 1, 1, 1, 1, 1 }
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
            List<int> minIndexes = FindMinIndex(chips);
            bool flag_ = true;
            foreach (int minIndex in minIndexes)
            {
                // Проверяем условия перемещения фишек
                bool shouldMoveRight = chips[neighbour(chips.Length, minIndex + 1)] + chips[neighbour(chips.Length, minIndex + 2)] > averageChips * 2;
                bool shouldMoveLeft = chips[neighbour(chips.Length, minIndex - 1)] + chips[neighbour(chips.Length, minIndex - 2)] > averageChips * 2;
                bool checkleft = chips[neighbour(chips.Length, minIndex - 1)] > chips[neighbour(chips.Length, minIndex - 2)];
                bool checkright = chips[neighbour(chips.Length, minIndex + 1)] > chips[neighbour(chips.Length, minIndex + 2)];
                bool rightIsGreater = chips[neighbour(chips.Length, minIndex + 1)] >= chips[neighbour(chips.Length, minIndex - 1)];
                // Перемещаем
                if (minIndexes.Count() > 1)
                {
                    if (shouldMoveRight && checkright)
                    {
                        chips[neighbour(chips.Length, minIndex + 1)]--;
                        chips[minIndex]++;
                        moves++;
                        flag_ = false;
                        break;
                        
                    }
                    else if (shouldMoveLeft && checkleft)
                    {
                        chips[neighbour(chips.Length, minIndex - 1)]--;
                        chips[minIndex]++;
                        moves++;
                        flag_ = false;
                        break;
                    }
                }
                if (minIndex == minIndexes[minIndexes.Count-1] || minIndexes.Count() == 1)
                {
                    if (shouldMoveRight && shouldMoveLeft && rightIsGreater)
                    {
                        chips[neighbour(chips.Length, minIndex + 1)]--;
                        chips[minIndex]++;
                        moves++;
                        flag_ = false;
                    }
                    else if (shouldMoveRight && shouldMoveLeft && !rightIsGreater)
                    {
                        chips[neighbour(chips.Length, minIndex - 1)]--;
                        chips[minIndex]++;
                        moves++;
                        flag_ = false;
                    }
                    else if (shouldMoveRight && !shouldMoveLeft)
                    {
                        chips[neighbour(chips.Length, minIndex + 1)]--;
                        chips[minIndex]++;
                        moves++;
                        flag_ = false;
                    }
                    else if (!shouldMoveRight && shouldMoveLeft)
                    {
                        chips[neighbour(chips.Length, minIndex - 1)]--;
                        chips[minIndex]++;
                        moves++;
                        flag_ = false;
                    }
                }
                
            }
            if (flag_)
            {
                List<int> maxIndexes = FindMaxIndex(chips);
                
                int minimal_distance = int.MaxValue;
                int side = -1;
                int index_of_elem = -1;
                foreach (int maxIndex in maxIndexes)
                {
                    foreach (int minIndex in minIndexes)
                    {
                        int distance;
                        int direction;

                        // Вычисляем расстояние между индексами, учитывая круговую структуру массива
                        if (minIndex > maxIndex)
                        {
                            if (minIndex - maxIndex< chips.Length - minIndex + maxIndex)
                            {
                                distance = minIndex - maxIndex;
                                direction = -1;
                            }
                            else
                            {
                                distance = chips.Length - minIndex + maxIndex;
                                direction = 1;
                            }
                        }
                        else
                        {
                            if (maxIndex - minIndex < chips.Length - maxIndex + minIndex)
                            {
                                distance = maxIndex - minIndex;
                                direction = -1;
                            }
                            else
                            {
                                distance = chips.Length - maxIndex + minIndex;
                                direction = 1;
                            }

                        }

                        // Обновляем минимальное расстояние, если найдено меньшее
                        if (distance < minimal_distance)
                        {
                            minimal_distance = distance;
                            side = direction;
                            index_of_elem = maxIndex;
                        }
                    }
                }

                chips[neighbour(chips.Length, index_of_elem)]--;
                    chips[neighbour(chips.Length, index_of_elem + side)]++;
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

    static List<int> FindMinIndex(int[] chips)
        {
            //int minIndex = 0;
            int minValue = chips[0];
            List<int> minIndex = new List<int>();
            for (int i = 0; i < chips.Length; i++)
            {
                if (chips[i] < minValue)
                {
                    minIndex.Clear();
                    minValue = chips[i];
                }
                if (chips[i] == minValue)
                {
                    minIndex.Add(i);
                }
            }
            return minIndex;
        }
    static List<int> FindMaxIndex(int[] chips)
    {

        int maxValue = chips[0];
        List<int> maxIndex = new List<int>();
        for (int i = 0; i < chips.Length; i++)
        {
            if (chips[i] > maxValue)
            {
                maxIndex.Clear();
                maxValue = chips[i];
            }
            if (chips[i] == maxValue)
            {
                maxIndex.Add(i);
            }
        }
        return maxIndex;
    }
}
