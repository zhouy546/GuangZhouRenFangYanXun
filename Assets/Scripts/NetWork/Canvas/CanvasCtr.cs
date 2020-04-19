using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtr : MonoBehaviour
{


    public List<GameObject> Sections = new List<GameObject>();

    // Start is called before the first frame update

    private void Awake()
    {
      //  EventCenter.AddListener<State>(EventDefine.ChangeState, SwitchCanvas);

     //   Debug.Log("addListener");
    }

    private void OnDestroy()
    {
       // EventCenter.RemoveListener<State>(EventDefine.ChangeState, SwitchCanvas);
    }


    public void SwitchCanvas(State _state) {
        Debug.Log("trigger event");
        switch (_state)
        {
            case State.DefautState:
                TurnOnOff(0);
                break;
            case State.VideoPlayState:
                TurnOnOff(1);
                break;
            case State.YanXunState:
                TurnOnOff(2);
                break;
            case State.QAState:
                TurnOnOff(3);
                break;
            default:
                break;
        }
    }

    public void TurnOnOff(int index)
    {
        for (int i = 0; i < Sections.Count; i++)
        {
            if (i == index)
            {
                Sections[i].SetActive(true);
            }
            else
            {
                Sections[i].SetActive(false);
            }
        }
    }






}
