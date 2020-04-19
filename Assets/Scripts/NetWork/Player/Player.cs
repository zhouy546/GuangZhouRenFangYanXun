using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

    public int currentID = 15;
    public bool isLocked = false;
    public List<Button> buttons = new List<Button>();

    private bool isServerIni = false;
    //同步变量还可以指定函数，使用hook;
    //当服务器改变了playerHP的值，客户端会调用ChangHP这个函数
    //这个值是以服务器为准。就算客户端改变了，服务器改变之后，客户端还是显示服务器的数据
    //[SyncVar]
    //public State _State;

    public CanvasCtr canvasCtr;

    // Start is called before the first frame update
    void Start()
    {
         StartCoroutine( initi());
    }



    public IEnumerator initi()
    {
        Debug.Log("我是不是服务器" + isServer);
        // clientSetState(State.DefautState);
        yield return new WaitForSeconds(0.5F);
        if (isServer&&isLocalPlayer)
        {
            ServerSetState(State.DefautState);
            Debug.Log("服务器设置状态");
        }
        else
        {
            Debug.Log("客户端设置状态");
            ClientSetState();
        }
    }

    //用户界面手点击选着角色后高亮并且更新当前ID
    public void  OnSelectID(int ID)
    {
        currentID = ID;
    }

    public void setLocalID_Locked(bool b,int id) {
        currentID = id;
        isLocked = b;
    }


    public void BtnSentSelectMsgToServer()
    {
        if (currentID >= 15)
        {
            return;
        }
        CmdchangeSyncList(currentID);
    }

    public void DisableBtn(int num)
    {
        buttons[num].Select();
        buttons[num].interactable = false;
    }


    //选定角色
    [Command]
    public void CmdchangeSyncList(int id)
    {
        if (!GameManager.instance.syncCharacter[id].isLocked)//未锁
        {
            if (!isLocked)
            {
                Debug.Log("未锁定");
                GameManager.characters temp = new GameManager.characters();
                temp.id = id;
                temp.isLocked = true;//一旦进入，服务器必定锁住

                GameManager.instance.syncCharacter[id] = temp;
                setLocalID_Locked(true, id);

                ServerChageSyncList(temp);
            }
        }
        else
        {
            setLocalID_Locked(false, 15);
        }
    }
    //改变服务器上按钮
    [Server]
    public void ServerChageSyncList(GameManager.characters temp)
    {
        foreach (Player player in GameManager.players.Values)
        {
            player.DisableBtn(temp.id);//关闭所有服务器上Client的按钮
        }
        ServerCanvasCtr.instance.SetBtnInteractable(temp);//关闭服务器UI上的按钮

        RpcChangeSyncList(temp);//RPC给所有Client设备
    }
    
    [ClientRpc]
    public void RpcChangeSyncList(GameManager.characters temp)
    {
        foreach (Player player in GameManager.players.Values)
        {
            player.DisableBtn(temp.id);//关闭所有服务器上Client的按钮
        }
    }

    [Client]
    void ClientSetState()
    {
        Debug.Log("客户端通知服务端修改服务端内容");
        Debug.Log(GameManager.instance.GameState + "服务端的状态");

        SyncGuiOnConnect();
    }

    void SyncGuiOnConnect()
    {
        
        canvasCtr.SwitchCanvas(GameManager.instance.GameState);//更新状态

        foreach (var temp in GameManager.instance.syncCharacter)//更新训演选人状态下UI
        {
            if (temp.isLocked)
            {
                DisableBtn(temp.id);
            }
        } 
        
    }



    //修改服务器状态值
    //并且通过RPC去修改客户端值
    //服务器内客户端UI层并未更新
    [Server]
    public void ServerSetState(State _state)
    {
        Debug.Log(_state+"服务端状态改变");
        GameManager.instance.GameState = _state;
        GameManager.ServerCanvasSwitch(_state);

        RpcServerSetState(_state);
    }

    //服务器给所有客户端广播更新
    [ClientRpc]
    public void RpcServerSetState(State _state)
    {
        foreach (Player player in GameManager.players.Values)
        {
            //if (!isLocalPlayer)//
            //{

                Debug.Log("服务器给所有客户端");
                player.canvasCtr.SwitchCanvas(_state);
           // }
        }

    }



    void Update()
    {
       
    }

    //public bool M_isServer()
    //{
    //    return m_isServer;
    //}
     

  //  public void OnCharacterLocked(bool b) {
  ////      isCharacterLocked = b;
  //  }
    
}
