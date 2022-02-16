using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformExtension
{
    public static List<Transform> GetAllChildren(this Transform parent)
    {
        return parent.Cast<Transform>().ToList();
    }
}
