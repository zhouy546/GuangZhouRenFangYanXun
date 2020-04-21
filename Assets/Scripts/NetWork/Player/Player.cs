using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{

    public List<GameObject> YanXungameObjects = new List<GameObject>();


//isServer获得的是这个软件当前是否是服务器,
    public bool AmILocalPlayer = false;

    [SyncVar]
    public int currentID = 15;
    [SyncVar]
    public bool isLocked = false;

    public List<Button> buttons = new List<Button>();

    public bool isServerIni = false;

    //[SyncVar]
    //public State _State;

    public CanvasCtr canvasCtr;

    // Start is called before the first frame update
    void Start()
    {
        AmILocalPlayer = isLocalPlayer;
         StartCoroutine( initi());
    }



    public IEnumerator initi()
    {

        // clientSetState(State.DefautState);

        yield return new WaitForSeconds(0.5F);
        Debug.Log("我是不是服务器" + isServer);
        if (isServer&&isLocalPlayer)
        {
            ServerSetState(State.DefautState);
            Debug.Log("服务器设置状态");
            isServerIni = true;
        }
        else
        {
            Debug.Log("客户端设置状态");
            ClientSetState();
        }
        //registerCha
        if (isServer) {
            GameManager.instance.IniCharacter(0, false, this.name, YanYun.DefaultState);
            Debug.Log("-------------------添加玩家进入syncCharacter---------------------");

        }

    }

    private void OnDestroy()
    {
        GameManager.instance.syncCharacter.Remove(GameManager.instance.getCharacterByName(this.name));
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
        if (currentID >=GameManager.MAXPLAYER)
        {
            return;
        }
        Debug.Log("客户端按钮ID" + currentID);
        CmdchangeSyncList(currentID,this.name);
    }

    public void DisableBtn(int num)
    {
        buttons[num].Select();
        buttons[num].interactable = false;
    }


    //选定角色
    [Command]
    public void CmdchangeSyncList(int id,string _PlayerName)
    {
        if (!GameManager.instance.getCharacterByName(_PlayerName).isLocked)//未锁
        {
            if (!isLocked)
            {
                Debug.Log("未锁定");
                GameManager.characters temp = new GameManager.characters();
                temp.id = id;
                temp.isLocked = true;//一旦进入，服务器必定锁住
                temp.M_name = _PlayerName;
                //设定角色进入到某个状态
                temp.YanYunState = YanYun.ZuZhangAlertState;
                int index = GameManager.instance.syncCharacter.IndexOf(GameManager.instance.getCharacterByName(_PlayerName));
                GameManager.instance.syncCharacter[index] = temp;

                setLocalID_Locked(true, id);
                Debug.Log("传入的数值"+id);
                Debug.Log(_PlayerName + "按按钮玩家名字---------------");
                Debug.Log(GameManager.instance.getCharacterByName(_PlayerName).M_name + "Structure中名字---------------");
                Debug.Log(GameManager.instance.getCharacterByName(_PlayerName).YanYunState + "Structure中状态---------------");
                Debug.Log(GameManager.instance.getCharacterByName(_PlayerName).id+"按钮后玩家选中按钮ID---------------");
                ServerChageSyncList( GameManager.instance.getCharacterByName(_PlayerName));
            }
        }
        else
        {
            setLocalID_Locked(false, GameManager.MAXPLAYER);
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

        //找到本地玩家，更新本地玩家UI; 不更新远程玩家UI

        Player tempPlayer = GameManager.GetCurrentLocalPlayer();
        foreach (Player player in GameManager.players.Values)
        {
            player.DisableBtn(temp.id);//关闭所有服务器上Client的按钮
        }
        Texture2D tempTexture2D = GameManager.PlayerName_PlayerInfo_KP[temp.M_name].TextureIDPhoto;
        tempPlayer.buttons[temp.id].GetComponent<Image>().sprite = GameManager.Texture2DtoSprite(tempTexture2D);

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
