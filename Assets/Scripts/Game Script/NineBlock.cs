using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void IntProccess(int i); 

public struct NineBlock {

    private event IntProccess Behaviour;
    public NineBlock(IntProccess behaviour) {
        Behaviour = behaviour;
    }

    public NineBlock(int row, int column, IntProccess behaviour) {
        GameInfos.Row = row;
        GameInfos.Column = column;
        Behaviour = behaviour;
    }

    public void Invoke(int center) {
        ScanHorizontal(center);
        ScanVertical(center);
    }

    void ScanHorizontal(int center) {
        if (center % GameInfos.Column != 0) {
            Behaviour.Invoke(center - 1);
            ScanVertical(center - 1);
        }
        if (center % GameInfos.Column != GameInfos.Column - 1) {
            Behaviour.Invoke(center + 1);
            ScanVertical(center + 1);
        }
    }
    void ScanVertical(int center) {
        if (center / GameInfos.Column > 0) {
            Behaviour.Invoke(center - GameInfos.Column);
        }
        if (center / GameInfos.Column < GameInfos.Row - 1) {
            Behaviour?.Invoke(center + GameInfos.Column);
        }
    }
}
