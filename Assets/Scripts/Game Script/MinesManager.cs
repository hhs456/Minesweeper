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
    public NineBlock RoundScan { get; private set; }
    public NineBlock RoundStep { get; private set; }
    public NineBlock RoundView { get; private set; }
    public NineBlock RoundRest { get; private set; }

    public MineBlock[] mineBlocks;
    public int[] minesPosition;
    public int MinesCount { get; set; }

    public int Row { get; set; }
    public int Column{ get; set; }

    NineBlock minesCountRaise;

    // Start is called before the first frame update
    public void Initial() {
        Timer.Start();
        GameEvent.RoundScan += OnRoundScan;
        GameEvent.RoundStep += OnRoundStep;
        GameEvent.RoundView += OnRoundView;
        GameEvent.RoundRest += OnRoundRest;
        GameEvent.Boom += ShowAllMines;
        RoundScan = new NineBlock(Scan);
        RoundStep = new NineBlock(Step);
        RoundView = new NineBlock(View);
        RoundRest = new NineBlock(Rest);
        mineBlocks = new MineBlock[transform.childCount];
        Begin(true);
    }

    public void SetRow(int row) {
        Row = row;
        GameInfos.SetStageBound(row, Column);
    }

    public void SetColumn(int column) {
        Column = column;
        GameInfos.SetStageBound(Row, column);
    }

    public void ResetFlag() {
        FlagCount = 0;
        flagDisplay.text = FlagCount.ToString("00");
    }

    public void Begin(bool isInitial) {
        GameInfos.SetStageBound(Row, Column);
        minesCountRaise = new NineBlock(Row, Column, MinesCountRaise);
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
        RoundStep.Invoke(e);
    }

    private void OnRoundScan(object sender, int e) {
        RoundScan.Invoke(e);
    }

    private void OnRoundView(object sender, int e) {
        RoundView.Invoke(e);
    }
    private void OnRoundRest(object sender, int e) {
        RoundRest.Invoke(e);
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
            minesCountRaise.Invoke(minesPosition[i]);
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
