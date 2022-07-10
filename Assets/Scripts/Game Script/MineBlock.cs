using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct MineArgs {
    public int index;
    public bool hasSweep;
    public bool hasMine;
    public bool hasFlag;
    public int minesCount;
    public MineArgs(int position) {
        index = position;
        hasSweep = false;
        hasMine = false;
        hasFlag = false;
        minesCount = 0;
    }
}
public class MineBlock : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

    private bool isRightDown;
    private bool isLeftDown;

    private MinesManager manager;    
    private Image image;
    private Text text;
    private Color origin;

    public CallMethod ResetEffect;
    public CallMethod SweepEffect;
    public CallMethod HoverEffect;
    public CallMethod OriginEffect;
    public CallMethod DisplayEffect;
    public CallMethod SetFlagEffect;
    public CallMethod DelFlagEffect;
    public CallMethod BoomEffect;
    public CallMethod FailEffect;
    public CallMethod WrongEffect;
    public MineArgs args;

    #region # Initial Block

    public void Initial() {
        manager = GetComponentInParent<MinesManager>();
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        GameBase.End += End;
    }

    public void Enable() {
        gameObject.SetActive(true);
        ResetEffect();
    }

    public void GetDefaultEffect() {
        origin = image.color;
        ResetEffect = () => {
            image.color = Color.white;
            text.enabled = false;
            origin = image.color;
        };
        SweepEffect = () => {
            image.color = Color.gray;
            origin = image.color;
        };
        HoverEffect = () => {
            origin = image.color;
            if (!args.hasSweep && !args.hasFlag)
                image.color = new Color(0.8f, 0.8f, 0.8f, 1);
            else
                image.color = image.color / 2;
        };        
        OriginEffect = () => {
            image.color = origin;
        };
        DisplayEffect = () => {
            Color[] colors = {
                    Color.gray, Color.blue, Color.green, Color.yellow, Color.red,
                    Color.cyan, Color.black, Color.white, new Color(1, 0, 1, 1)
                };
            text.color = colors[args.minesCount];
        };
        SetFlagEffect = () => {
            image.color = Color.yellow;
            origin = image.color;
        };
        DelFlagEffect = () => {
            image.color = Color.white;
            origin = image.color;
        };
        BoomEffect = () => {
            image.color = Color.red;
            origin = image.color;
        };
        FailEffect = () => {
            text.color = Color.red;
            text.text = "@";
            text.enabled = true;
            origin = image.color;
        };
        WrongEffect = () => {
            text.text = "X";
            text.enabled = true;
            text.color = Color.red;
        };
    }

    public void Unable() {        
        gameObject.SetActive(false);        
    }
    #endregion

    public void Scan() {
        if (args.hasSweep)
            return;
        if (args.hasMine)
            return;
        if (args.hasFlag)
            return;
        args.hasSweep = true;
        SweepEffect();
        if (args.minesCount > 0)
            text.enabled = true;
        else
            GameEvent.OnRoundScan(this, args.index);        
    }

    public void Step() {        
        if (args.hasSweep)
            return;        
        if (args.hasFlag)
            return;
        if (args.hasMine) {
            BoomEffect();
            args.hasSweep = true;            
            GameBase.OnEnd(this, new System.EventArgs());
        }
        else {
            Scan();
        }
    }

    public void View() {
        if (args.hasFlag)
            return;
        if (args.hasSweep)
            return;
        HoverEffect();
    }
    public void Rest() {
        OriginEffect();
    }

    void SetFlag(bool setup) {
        Debug.Log("Flag " + GameInfos.GetPositionOf(args.index));
        if (args.hasSweep)
            return;
        args.hasFlag = setup;
        if (setup) {
            SetFlagEffect();
            manager.FlagCount++;
            manager.flagDisplay.text = manager.FlagCount.ToString("00");
            if (args.hasMine)
                manager.SweepCount++;            
        }
        else {
            DelFlagEffect();
            manager.FlagCount--;
            manager.flagDisplay.text = manager.FlagCount.ToString("00");
            if (args.hasMine)
                manager.SweepCount--;            
        }
        manager.CheckEnd();
    }

    void End(object sender, System.EventArgs e) {
        if (args.hasMine) {
            if(!args.hasFlag)
                FailEffect();
        }
        else if (args.hasFlag)
            WrongEffect();
    }

    public void Wrong() {
        WrongEffect();
    }

    #region # Mouse Event
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Trigger " + GameInfos.GetPositionOf(args.index));
        if (Input.GetMouseButtonUp(1)) {
            isRightDown = false;
            SetFlag(!args.hasFlag);
        }
        if (Input.GetMouseButtonUp(0)) {
            isLeftDown = false;
            Step();
            if (isRightDown)
                GameEvent.OnRoundStep(this, args.index);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (Input.GetMouseButtonDown(0)) {            
            isLeftDown = true;
        }
        if (Input.GetMouseButtonDown(1)) {            
            isRightDown = true;
        }
        if (isLeftDown && isRightDown)
            GameEvent.OnRoundView(this, args.index);
    }
    public void OnPointerUp(PointerEventData eventData) {
        if (Input.GetMouseButtonUp(0)) {            
            isLeftDown = false;
        }
        if (Input.GetMouseButtonUp(1)) {            
            isRightDown = false;
        }
        if (!(isLeftDown && isRightDown))
            GameEvent.OnRoundRest(this, args.index);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        HoverEffect();
    }

    public void OnPointerExit(PointerEventData eventData) {
        OriginEffect();
        GameEvent.OnRoundRest(this, args.index);
    }
    #endregion
}
