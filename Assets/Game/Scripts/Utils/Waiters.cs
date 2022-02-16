using UnityEngine;

public static class Waiters
{
    public readonly static WaitForSeconds second = new WaitForSeconds(1);
    public readonly static WaitForSeconds halfSecond = new WaitForSeconds(0.5f);
    public readonly static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
}
