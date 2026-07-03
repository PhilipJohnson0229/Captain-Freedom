using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we can only inherit from abstract classes they cannot be attached to gameobjects
//this is basically an interface
//an abstract class can still hold state and define data where an interface cannot
//you can only inherit from one abstract class
//but you can inherit from as many interfaces as needed
public abstract class Actions : MonoBehaviour
{
    public abstract void Act();
}