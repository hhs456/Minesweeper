using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TimerType {
    Menu, Game
}

public static class Timer {
    public static event EventHandler<EventArgs> Begin;
    public static event EventHandler<Watch> CentiSecond;    
    public static event EventHandler<bool> Pause;    
    public static Watch watch = new Watch();

    public static void OnCentiSecond(object sender, Watch e) {
        CentiSecond?.Invoke(sender, e);       
    }

    public static void Update(GameClock executor) {     
        watch.playTime = AudioSettings.dspTime - watch.startTime - watch.pauesTime;
        watch.minute = (int)(watch.playTime / 60f);
        watch.second = (int) watch.playTime - watch.minute * 60;
        watch.centiSecond = (int)(watch.playTime * 100f % 100f);        
        OnCentiSecond(executor, watch);
    }

    public static void OnPause(object sender, bool isPause) {        
        if (isPause) {
            watch.pauseBeginTime = AudioSettings.dspTime;
        }
        else {
            watch.pauseEndTime = AudioSettings.dspTime;
            watch.GetPauseTime();
        }
        Pause?.Invoke(sender, isPause);
    }

    public static void Start() {        
        watch.startTime = AudioSettings.dspTime;        
    }

    public static void Fix(float comp) {        
        watch.playTime += comp;        
    }

    public static void Reset() {        
        watch = new Watch();
        Start();
    }
}
[Serializable]
public struct Watch {
    public double startTime;
    public double playTime;
    public double pauseBeginTime;
    public double pauseEndTime;
    public double pauesTime;
    public int minute;
    public int second;
    public int centiSecond;

    public int Minute { get => minute; set => minute = value; }
    public int Second { get => second; set => second = value; }
    public int CentiSecond { get => centiSecond; set => centiSecond = value; }

    public double GetPauseTime() {
        pauesTime += pauseEndTime - pauseBeginTime;
        return pauesTime;
    }
}

public interface IWatch {

    int Minute { get; set; }
    int Second { get; set; }
    int CentiSecond { get; set; }
}
