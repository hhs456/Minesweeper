using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayArea {
    public int Row { get; set; }
    public int Column { get; set; }

    public static PlayArea Instance { get; private set; }
    public static NineBlock Process { get; private set; }

    public PlayArea(int row, int column) {
        Row = row;
        Column = column;
        Instance = this;
        Process = new NineBlock();
    }

    public void SetStageBound(int row, int column) {
        Row = row;
        Column = column;
    }

    public int GetRowOf(int index) {
        return index / Column;        
    }
    public int GetColumnOf(int index) {
        return index % Row;
    }

    public (int,int) GetPositionOf(int index) {        
        return (index % Row, index / Column);
    }
}
