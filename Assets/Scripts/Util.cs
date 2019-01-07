using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour {

    public static T Choose<T>(params T[] input)
    {

        return input[Random.Range(0, input.Length)];
    }
}
