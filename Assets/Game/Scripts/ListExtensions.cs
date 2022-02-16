using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ListExtensions
{
    private static readonly System.Random Random = new System.Random();

    public static T GetNextItem<T>(this List<T> list, int currentIndex)
    {
        return currentIndex + 1 >= list.Count - 1 ? list[currentIndex + 1] : list[0];
    }

    public static int GetNextIndex<T>(this List<T> list, int currentIndex)
    {
        return currentIndex + 1 < list.Count ? currentIndex + 1 : 0;
    }

    public static bool TryGetNextItem<T>(this List<T> list, T item, out T element)
    {
        if (list.Count == 0)
        {
            element = default;
            return false;
        }

        var index = list.IndexOf(item);
        element = index == list.Count - 1 ? list[0] : list[index + 1];
        return index != list.Count - 1;
    }

    public static bool TryGetPreviousItem<T>(this List<T> list, T item, out T element)
    {
        if (list.Count == 0)
        {
            element = default;
            return false;
        }

        var index = list.IndexOf(item);
        element = index == 0 ? list[0] : list[index - 1];
        return index != 0;
    }

    public static T GetLastElement<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    private static bool IsIndexExist<T>(this List<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[Random.Next(0, list.Count)];
    }

    public static T GetRandomElement<T>(this IEnumerable<T> mass)
    {
        var enumerable = mass as T[] ?? mass.ToArray();
        return enumerable[Random.Next(0, enumerable.Length)];
    }

    public static T GetClosestElementOnZ<T>(this List<T> list, float minDistance, float z) where T : MonoBehaviour
    {
        return list.FirstOrDefault(element => Mathf.Abs(z - element.transform.position.z) <= minDistance);
    }

    public static T GetClosestElement<T>(this IEnumerable<T> list, Vector3 point) where T : MonoBehaviour
    {
        var minDistance = Mathf.Infinity;
        T minPoint = null;
        foreach (var element in list)
        {
            var distance = Vector3.Distance(element.transform.position, point);
            if (distance > minDistance) continue;
            minDistance = distance;
            minPoint = element;
        }

        return minPoint;
    }

    public static Vector3 GetClosest(this IEnumerable<Vector3> list, Vector3 point)
    {
        var minDistance = Mathf.Infinity;
        var minPoint = Vector3.one;
        foreach (var element in list)
        {
            var distance = Vector3.Distance(element, point);
            if (distance > minDistance) continue;
            minDistance = distance;
            minPoint = element;
        }

        return minPoint;
    }

    public static void Reverse(this ref Vector2 vector)
    {
        var delta = vector;
        vector.x = delta.y;
        vector.y = delta.x;
    }
}