using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Size {
    Row, Column, Mine
}

public class SizeSender : MonoBehaviour
{
    public Size type;
    [Range(1,99)]
    public int Limit;
    private MinesManager manager;
    private InputField inputField;

    void Awake() {
        inputField = GetComponent<InputField>();
        inputField.onValueChanged.AddListener(SendSize);
        manager = FindObjectOfType<MinesManager>();
        SendSize(inputField.text);
    }

    void SendSize(string str) {
        int num = int.Parse(str);
        
        if (num > Limit) {
            num = Limit;
            inputField.text = num.ToString();
        }
        switch (type) {
            case Size.Row:
                manager.Row = num;
                break;
            case Size.Column:
                manager.Column = num;
                break;
            case Size.Mine:
                manager.MinesCount = num;
                break;                
            default:
                break;
        }
    }
}
