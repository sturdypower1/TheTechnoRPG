using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// used to represent all the things that can be obtained in the game
/// </summary>
public abstract class IObtainable : ScriptableObject
{
    public abstract void Obtain();
}


