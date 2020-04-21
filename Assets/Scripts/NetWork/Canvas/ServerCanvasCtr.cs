using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ServerCanvasCtr :MonoBehaviour 
{
    public static List<GameObject> Sections = new List<GameObject>();
    public GameObject G_defalut;
    public GameObject G_VideoPlayState;
    public GameObject G_YanXunState;
    public GameObject G_QAState;

    public static ServerCanvasCtr instance;
    public List<Button> Characterbuttons = new List<Button>();

    public static YanYun ServerYanYunState = YanYun.DefaultState;

    public void Awake()
    {
        instance = this;

        Sections.Add(G_defalut);
        Sections.Add(G_VideoPlayState);
        Sections.Add(G_YanXunState);
        Sections.Add(G_QAState);
    }


    public  void RegisterServerGuiEvent()
    {


            Debug.Log("本地Player是服务器 所以添加服务器事件");
            EventCenter.AddListener(EventDefine.OnYanYunStart, funYanYunStart);

    }

    public  void UnRegisterServerGuiEvent()
    {

        Debug.Log("本地Player是服务器 所以移除服务器事件");

        EventCenter.RemoveListener(EventDefine.OnYanYunStart, funYanYunStart);

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
                EventCenter.Broadcast(EventDefine.OnYanYunStart);

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

        Debug.Log(cha.M_name);
        Characterbuttons[cha.id].interactable = !cha.isLocked;
        Texture2D tempTexture2D = GameManager.PlayerName_PlayerInfo_KP[cha.M_name].TextureIDPhoto;
        Characterbuttons[cha.id].GetComponent<Image>().sprite = GameManager.Texture2DtoSprite(tempTexture2D);

    }

    public  void funYanYunStart() {
        Debug.Log("服务器上显示训演开始UI");
        TurnOnOff(2);
        ServerYanYunState = YanYun.PlayerTakePhotoState;
    }





    //public void UpdateServerPhoto()
    //{
    //    Debug.Log("服务器更新图片");
    //    int i = 0;

    //    foreach (var key in GameManager.PlayerName_PlayerInfo_KP.Keys)
    //    {
    //        foreach (var img in testRawIamge)
    //        {
    //            if(img.name == key)
    //            {
    //                img.texture = GameManager.PlayerName_PlayerInfo_KP[key].TextureIDPhoto;
    //            }
    //        }
    //    }

    //}

}
