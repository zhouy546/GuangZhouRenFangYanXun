using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerCanvasCtr : MonoBehaviour
{
    public static List<GameObject> Sections = new List<GameObject>();
    public GameObject G_defalut;
    public GameObject G_VideoPlayState;
    public GameObject G_YanXunState;
    public GameObject G_QAState;

    public static ServerCanvasCtr instance;
    public List<Button> Characterbuttons = new List<Button>();
    public void Awake()
    {
        instance = this;

        Sections.Add(G_defalut);
        Sections.Add(G_VideoPlayState);
        Sections.Add(G_YanXunState);
        Sections.Add(G_QAState);
    }

    public static void SwitchCanvas(State _state)
    {
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

    public static void TurnOnOff(int index)
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

    public void SetBtnInteractable(GameManager.characters cha)
    {
        Characterbuttons[cha.id].interactable = !cha.isLocked;
    }
}
