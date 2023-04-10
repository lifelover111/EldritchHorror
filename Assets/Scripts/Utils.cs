using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
    }
}
