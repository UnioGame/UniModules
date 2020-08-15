namespace UniModules.UniGame.Algo.Runtime.KnapsackAlgo
{
    using System.Collections.Generic;
    using UnityEngine;

    public class KnapsackSolver
    {
        public static (int maxValue,List<int> selection) Solve(int capacity, int[] weight, int[] value, int itemsCount)
        {
            var selection = new List<int>();
            var selected = new int[itemsCount + 1][];
            var k = new int[itemsCount + 1][];
            
            for (var i = 0; i < k.Length; i++) {
                k[i] = new int[capacity + 1];
                selected[i] = new int[capacity+1];
                for (int d = 0; d < capacity+1; d++) {
                    selected[i][d] = -1;
                }
            }

            for (var i = 1; i <= itemsCount; ++i)
            {
                for (var w = 1; w <= capacity; ++w) {
                    if (w >= weight[i - 1]) {
                        var newValue = value[i - 1] + k[i - 1][w - weight[i - 1]];
                        var previous = k[i - 1][w];
                        var max = Mathf.Max(newValue, previous);
                        k[i][w] = max;
                        if (newValue > previous) {
                            selected[i][w] = i-1;
                        }
                        else {
                            selected[i][w] = selected[i-1][w];
                        }
                    }
                    else {
                        k[i][w] = k[i - 1][w];
                        selected[i][w] = selected[i - 1][w];
                    }
                }
            }

            var currentValue = k[itemsCount][capacity];
            var selectionWeight = capacity;

            for (var w = capacity; w >= 0 && currentValue > 0; w--) {
                for (var i = itemsCount; i >= 0 && currentValue > 0; i--) {
                    var maxValue =  k[i][w];
                    if (maxValue != currentValue) {
                        continue;
                    }

                    var index     = selected[i][w];
                    var itemValue = value[index];
                    currentValue -= itemValue;
                    selection.Add(index);
                }
            }

            return (k[itemsCount][capacity],selection);
        }
    }
}
