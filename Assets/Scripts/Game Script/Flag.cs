using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    void Awake() {
        Timer.CentiSecond += OnUpdate;
    }

    private void OnUpdate(object sender, Watch e) {
        
    }
}
