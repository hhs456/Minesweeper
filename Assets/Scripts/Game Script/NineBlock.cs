using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NineBlock {

    private Action<int> Behaviour;

    public void Invoke(int center, Action<int> behaviour) {
        Behaviour = behaviour;
        ScanHorizontal(center);
        ScanVertical(center);
    }

    void ScanHorizontal(int center) {
        if (center % PlayArea.Instance.Column != 0) {
            Behaviour.Invoke(center - 1);            
        }
        if (center % PlayArea.Instance.Column != PlayArea.Instance.Column - 1) {
            Behaviour.Invoke(center + 1);            
        }
    }
    void ScanVertical(int center) {
        if (center / PlayArea.Instance.Column > 0) {
            int target = center - PlayArea.Instance.Column;
            Behaviour.Invoke(target);
            ScanHorizontal(target);
        }
        if (center / PlayArea.Instance.Column < PlayArea.Instance.Row - 1) {
            int target = center + PlayArea.Instance.Column;
            Behaviour?.Invoke(target);
            ScanHorizontal(target);
        }
    }
}
