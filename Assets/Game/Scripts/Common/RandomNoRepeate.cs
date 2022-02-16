using System.Collections.Generic;
using UnityEngine;

public class RandomNoRepeate
{
    private readonly List<int> available = new List<int>();
    private int count;
    private int last;
    private int iteration = 0;
    private int requestNum = 0;

    private void Reset()
    {
        available.Clear();
        requestNum = 0;
        for (var i = 0; i < count; i++) available.Add(i);
        //Remove last, so it won't be repeated
        if (++iteration > 1 && count > 1)
        {
            available.Remove(last);
        }
        //Debug.Log("Reset");
    }

    public void Init(int value)
    {
        iteration = 0;
        count = value;
        Reset();
    }

    public int GetAvailable()
    {
        CheckAvailableIds();

        return GetAvailableAtId(Random.Range(0, available.Count));
    }

    private int GetAvailableAtId(int availableId)
    {
        if (availableId < 0 || availableId >= count) return -1;

        CheckAvailableIds();

        if (availableId >= available.Count)
            Reset();

        var id = available[availableId];
        available.RemoveAt(availableId);

        //Adding back removed index after first request
        if (++requestNum == 1 && iteration > 1 && count > 1) available.Add(last);
        last = id;

        return id;
    }

    private void CheckAvailableIds()
    {
        if (available.Count == 0) Reset();
    }
}