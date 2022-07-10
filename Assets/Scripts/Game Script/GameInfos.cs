using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfos
{
    public static int Row { get; set; }
    public static int Column { get; set; }

    public static void SetStageBound(int row, int column) {
        Row = row;
        Column = column;
    }

    public static int GetRowOf(int index) {
        return index / Column;        
    }
    public static int GetColumnOf(int index) {
        return index % Row;
    }

    public static (int,int) GetPositionOf(int index) {        
        return (index % Row, index / Column);
    }
}
