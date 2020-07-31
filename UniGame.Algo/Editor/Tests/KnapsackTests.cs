using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UniModules.UniGame.Algo.Runtime.KnapsackAlgo;
using UnityEngine;

public class KnapsackTests
{
    [Test]
    public void SelectedItemsSolverTest()
    {
        int[] value    = { 10, 50, 70,100,20,30 };
        int[] weight   = { 10, 20, 30,35,40,50 };
        int   capacity = 100;

        //act
        var result = KnapsackSolver.Solve(capacity, weight, value, weight.Length);

        var selection     = result.selection;
        var message       = "";
        var selectedItems = string.Join(" ", selection);
        message += $"SELECTION: {selectedItems}";
            
        Debug.Log($"RESULT {result.maxValue} Selection:\n {message}");
            
        Assert.That(result.maxValue == 230);
        Assert.That(result.selection.Count == 4);
        Assert.That(result.selection.SequenceEqual(new List<int>() {
            3,2,1,0
        }));
    }
}
