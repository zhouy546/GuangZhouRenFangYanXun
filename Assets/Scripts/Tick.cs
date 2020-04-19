using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{
    public delegate void FinishCountDown();
    public event FinishCountDown finishCountDownEvent;

    public delegate IEnumerator StartCountDown();
    public event StartCountDown startCountDownEvent;


    public delegate void StopCountDonw();
    public event StopCountDonw stopCountDonwEvent;

    public delegate void ResetTime();
    public event ResetTime resetTimeEvent;

    public float DefaultCountDonwTime = 5;
    public float CurrentCountDonwTime = 5;

    public bool enableKeyBoardDebug;

    public bool IsCountDonw = false;

    void Awake() {
        CurrentCountDonwTime = DefaultCountDonwTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        startCountDownEvent += CountDown;
        finishCountDownEvent += EndCountDown;
        stopCountDonwEvent += stopCountdonw;
        resetTimeEvent += resetCurrentCountDonwTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableKeyBoardDebug) {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Func_StartCountDonw();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Func_StopCountDonw();
            }
        }
    }

    public void Func_ResetTime() {
        resetTimeEvent.Invoke();
    }

    public void Func_StopCountDonw() {
        IsCountDonw = false;
        stopCountDonwEvent.Invoke();
    }

    public void Func_StartCountDonw() {
        if (!IsCountDonw) {
            IsCountDonw = true;
            StartCoroutine(startCountDownEvent.Invoke());
        //    Debug.Log("Invoke CountDonwEvent");
        }
    }

    public void Func_FinishCountDonw() {
        IsCountDonw = false;
        finishCountDownEvent.Invoke();
    }

    private IEnumerator CountDown() {
        CurrentCountDonwTime--;
        yield return new WaitForSeconds(1f);
        //Debug.Log(CurrentCountDonwTime);
        if (CurrentCountDonwTime <= 0)
        {
            Func_FinishCountDonw();
        }
        else {
            StartCoroutine(CountDown());
        }
    }

    private void EndCountDown() {
        Func_ResetTime();
       // Debug.Log("Count Donw End");
        //CameraCt.instance.AutoRotateCam();
    //   ValueSheet.IsAutoRotate = true;
    }

    private void resetCurrentCountDonwTime() {
        //Debug.Log("reset time");
        CurrentCountDonwTime = DefaultCountDonwTime;
    }

    private void stopCountdonw() {
   //     Debug.Log("Stop CountDonw");
        StopAllCoroutines();
        Func_ResetTime();
    }
    
}
