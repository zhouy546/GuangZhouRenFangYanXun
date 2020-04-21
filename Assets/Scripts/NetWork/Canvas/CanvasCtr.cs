using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCtr : MonoBehaviour
{
    public Player Parentplayer;

    public List<GameObject> Sections = new List<GameObject>();

    // Start is called before the first frame update

    private void Awake()
    {

    }

    public void RegisterClientGuiEvent()
    {


            Debug.Log("本地Player是Client 所以添加客户端事件");
            EventCenter.AddListener(EventDefine.OnYanYunStart, funClientYanYunStart);

    }

    public void UnRegisterClientGuiEvent()
    {
        Debug.Log(Parentplayer.name);

            EventCenter.RemoveListener(EventDefine.OnYanYunStart, funClientYanYunStart);

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
                EventCenter.Broadcast(EventDefine.OnYanYunStart);
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


    public void funClientYanYunStart()
    {

        TurnOnOff(2);
        for (int i = 0; i < GameManager.instance.syncCharacter.Count; i++)
        {

            GameManager.characters temp = GameManager.instance.syncCharacter[i];

            temp.YanYunState = YanYun.PlayerTakePhotoState;

            GameManager.instance.syncCharacter[i] = temp;
        }


    }




}
