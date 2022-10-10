using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Events
{
    public static Action BuildingActivation;
    public static Action BuildingDeactivation;
    public static Action CookingActivation;
    public static Action CookingDeactivation;
    public static Action Load;
    public static Action<Transform> AddBuilding;
}
