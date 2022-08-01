using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinesManager : MonoBehaviour
{
    public bool enableDefaultEffect;
    public Text flagDisplay;
    public int FlagCount { get; set; }
    public int SweepCount { get; set; }    

    public MineBlock[] mineBlocks;
    public int[] minesPosition;
    public int MinesCount { get; set; }

    public int Row { get; set; }
    public int Column{ get; set; }
    

    // Start is called before the first frame update
    public void Initial() {
        Timer.Start();
        PlayEvent.RoundScan += OnRoundScan;
        PlayEvent.RoundStep += OnRoundStep;
        PlayEvent.RoundView += OnRoundView;
        PlayEvent.RoundRest += OnRoundRest;
        PlayEvent.Boom += ShowAllMines;
        mineBlocks = new MineBlock[transform.childCount];        
        new PlayArea(Row, Column);
        Begin(true);
    }

    public void ResetFlag() {
        FlagCount = 0;
        flagDisplay.text = FlagCount.ToString("00");
    }

    public void Begin(bool isInitial) {
        PlayArea.Instance.SetStageBound(Row, Column);        
        minesPosition = new int[MinesCount];
        int blockCount = Row * Column;
        for (int i = 0; i < mineBlocks.Length; i++) {
            mineBlocks[i] = transform.GetChild(i).GetComponent<MineBlock>();
            if (isInitial) {                
                mineBlocks[i].Initial();
            }
            if (enableDefaultEffect)
                mineBlocks[i].GetDefaultEffect();
            if (i < blockCount) {
                mineBlocks[i].args = new MineArgs(i);
                mineBlocks[i].Enable();
            }
            else
                mineBlocks[i].Unable();
        }
        GetComponent<GridLayoutGroup>().constraintCount = Column;
        SetMinesAt(0);        
        SweepCount = 0;
        ResetFlag();
        GameBase.OnBegin(this, new EventArgs());
    }
    public void Scan(int index) {
        mineBlocks[index].Scan();
    }

    public void Step(int index) {        
        mineBlocks[index].Step();
    }
    public void View(int index) {
        mineBlocks[index].View();
    }
    public void Rest(int index) {
        mineBlocks[index].Rest();
    }

    private void OnRoundStep(object sender, int e) {
        PlayArea.Process.Invoke(e, Step);
    }

    private void OnRoundScan(object sender, int e) {
        PlayArea.Process.Invoke(e, Scan);
    }

    private void OnRoundView(object sender, int e) {
        PlayArea.Process.Invoke(e, View);
    }
    private void OnRoundRest(object sender, int e) {
        PlayArea.Process.Invoke(e, Rest);
    }

    void SetMinesAt(int i) {
        if (i == MinesCount)
            return;
        minesPosition[i] = UnityEngine.Random.Range(0, Row * Column - 1);
        if (minesPosition.TryRepeatFront(i)) {
            SetMinesAt(i);
        }
        else {
            mineBlocks[minesPosition[i]].args.hasMine = true;
            PlayArea.Process.Invoke(minesPosition[i], MinesCountRaise);
            SetMinesAt(i + 1);
        }
    }

    void MinesCountRaise(int index) {
        mineBlocks[index].args.minesCount++;
        mineBlocks[index].GetComponentInChildren<Text>().text = mineBlocks[index].args.minesCount.ToString();
        mineBlocks[index].DisplayEffect();
    }

    public void CheckEnd() {
        if (SweepCount == MinesCount) {
            GameBase.OnEnd(this, new EventArgs());
            Debug.Log("Succeed");
        }
    }

    public void ShowAllMines(object sender, int e) {
        int i_size = MinesCount;
        for(int i =0; i< i_size; i++) {
            if (mineBlocks[minesPosition[i]].args.hasMine)
                mineBlocks[minesPosition[i]].BoomEffect();
        }
    }
}
