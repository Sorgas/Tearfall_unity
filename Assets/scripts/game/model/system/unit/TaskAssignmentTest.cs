using System;
using System.Collections.Generic;
using MoreLinq;
using UnityEngine;
using util.geometry;
using Random = UnityEngine.Random;

namespace game.model.system.unit {
public  class TaskAssignmentTest {
    public void Main() {
        List<Vector3Int> units = new();
        List<Vector3Int> tasks = new();
        string unitsString = "";
        string tasksString = "";
        for (int i = 0; i < 5; i++) {
            Vector3Int unit = new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            Vector3Int task = new Vector3Int(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            units.Add(unit);
            tasks.Add(task);
            unitsString += unit + " ";
            tasksString += task + " ";
        }
        Console.WriteLine(unitsString);
        Console.WriteLine(tasksString);
        List<KeyValuePair<Vector3Int, Vector3Int>> pairs = findPairs(units, tasks);
    }

    private static List<KeyValuePair<Vector3Int, Vector3Int>> findPairs(List<Vector3Int> units, List<Vector3Int> tasks) {
        Vector3Int currentUnit = units[0];
        List<KeyValuePair<Vector3Int, Vector3Int>> pairs = new();
        while (units.Count > 0) {
            KeyValuePair<Vector3Int, Vector3Int> pair = selectTaskForUnit(currentUnit, units, tasks);
            units.Remove(pair.Key);
            tasks.Remove(pair.Value);
            pairs.Add(pair);
        }
        return pairs;
    }

    private static KeyValuePair<Vector3Int, Vector3Int> selectTaskForUnit(Vector3Int unit, List<Vector3Int> units, List<Vector3Int> tasks) {
        Vector3Int nearestTask = selectNearest(unit, tasks);
        Vector3Int nearestUnit = selectNearest(nearestTask, units);
        if (nearestUnit == unit) {
            return new KeyValuePair<Vector3Int, Vector3Int>(unit, nearestTask);
        } else {
            return selectUnitForTask(nearestTask, units, tasks);
        }
    }

    private static KeyValuePair<Vector3Int, Vector3Int> selectUnitForTask(Vector3Int task, List<Vector3Int> units, List<Vector3Int> tasks) {
        Vector3Int nearestUnit = selectNearest(task, units);
        Vector3Int nearestTask = selectNearest(nearestUnit, tasks);
        if (nearestTask == task) {
            return new KeyValuePair<Vector3Int, Vector3Int>(nearestUnit, task);
        } else {
            return selectTaskForUnit(nearestUnit, units, tasks);
        }
    }

    private static Vector3Int selectNearest(Vector3Int reference, List<Vector3Int> vectors) {
        return vectors
            .MinBy(vector => reference.fastDistance(vector));
    }
}
}