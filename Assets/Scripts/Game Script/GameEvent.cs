using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public static event EventHandler<int> RoundStep;
    public static event EventHandler<int> RoundScan;
    public static event EventHandler<int> RoundView;
    public static event EventHandler<int> RoundRest;
    public static event EventHandler<int> Boom;

    public static void OnRoundScan(object sender, int e) {
        RoundScan?.Invoke(sender, e);
    }

    public static void OnRoundStep(object sender, int e) {
        RoundStep?.Invoke(sender, e);
    }
    public static void OnRoundView(object sender, int e) {
        RoundView?.Invoke(sender, e);
    }
    public static void OnRoundRest(object sender, int e) {
        RoundRest?.Invoke(sender, e);
    }
    public static void OnBoom(object sender, int e) {
        Boom?.Invoke(sender, e);
    }

}
