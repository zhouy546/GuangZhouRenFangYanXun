
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum State { DefautState,VideoPlayState, YanXunState, QAState }
public enum YanYun {
    DefaultState,
    PlayerTakePhotoState,
    PlayerSelectCharacterState,
    ZuZhangAlertState,
    FuZuZhangStandByState,
    ShuShanYinDaoState,
    SeQuJuMingState,
    ZhiAnBaoZhangState,
    HouQingBaoZhangState,
    CancelAlertState,
    EndState,
}
public class GameManager : NetworkBehaviour
{
    public static int MAXPLAYER = 15; 

    public static GameManager instance;

    private const string PLAYER_ID_PREFIX = "Player";

    public static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static Dictionary<string, PlayerInfo> PlayerName_PlayerInfo_KP = new Dictionary<string, PlayerInfo>();

    public List<GameObject> YanYunGameObjects = new List<GameObject>();

    [SyncVar]
    public State GameState = State.DefautState;



    //同步ListStructure--------------------------------
    public struct characters
    {
        public int id;
        public bool isLocked;
        public string M_name;
        public YanYun YanYunState;

        //public void SetId(int _id)
        //{
        //    id = _id;
        //}

        //public void SetisLocked(bool _isLocked)
        //{
        //    isLocked = _isLocked;
        //}

        //public void SetM_name(string _M_name)
        //{
        //    M_name = _M_name;
        //}

        //public void SetYanYunState(YanYun _YanYunState)
        //{
        //    YanYunState = _YanYunState;
        //}
    };

    public class SyncCharacter : SyncListStruct<characters> { }


    public SyncCharacter syncCharacter = new SyncCharacter();
    //同步ListStructure--------------------------------


    //初始化15个角色
    public void IniCharacter(int _id,bool _isLocked,string _name, YanYun _YanYun) {

            characters cha = new characters();
            cha.id = _id;
            cha.isLocked = _isLocked;
            cha.M_name = _name;
            cha.YanYunState = _YanYun;
            syncCharacter.Add(cha);
    }


    public characters getCharacterByName(string keyName)
    {

        characters te = new characters();
        for (int i = 0; i < syncCharacter.Count; i++)
        {
            if (syncCharacter[i].M_name == keyName)
            {
                return syncCharacter[i];
            }
        }

        Debug.Log("Search character fail");

        return te;
    }

    public int getCharacterIndex(GameManager.characters temp)
    {
        int i = 0;
        for (i = 0; i < GameManager.instance.syncCharacter.Count; i++)
        {
            //Debug.Log(i);
            //Debug.Log(GameManager.instance.syncCharacter[i].Equals(temp));
            //Debug.Log(GameManager.instance.syncCharacter[i].M_name);
            //Debug.Log(temp.M_name);
            if (GameManager.instance.syncCharacter[i].M_name == temp.M_name)
            {
             //   Debug.Log("返回值I："+i);
                return i;
            }
        }
        return i;
    }


    //        public  void SearchChaChnageState(string keyName,YanYun _yanYun)
    //{
    //    for (int i = 0; i < syncCharacter.Count; i++)
    //    {
    //        if (syncCharacter[i].M_name == keyName)
    //        {

    //            Debug.Log("玩家： " + keyName + "状态被改： " + _yanYun);
    //            syncCharacter[i].SetYanYunState(_yanYun);
    //        }
    //    }
    //}

    public void Start()
    {
        instance = this;
    }

    public void OnYanYunStateChange(YanYun _yanYun)
    {
        Debug.Log(_yanYun.ToString());
        switch (_yanYun)
        {
            case YanYun.DefaultState:
                TurnOnYanYunGui(-1);
                break;
            case YanYun.PlayerTakePhotoState:
                TurnOnYanYunGui(0);
                break;
            case YanYun.PlayerSelectCharacterState:
                TurnOnYanYunGui(1);
     
                break;
            case YanYun.ZuZhangAlertState:
                break;
            case YanYun.FuZuZhangStandByState:
                break;
            case YanYun.ShuShanYinDaoState:
                break;
            case YanYun.SeQuJuMingState:
                break;
            case YanYun.ZhiAnBaoZhangState:
                break;
            case YanYun.HouQingBaoZhangState:
                break;
            case YanYun.CancelAlertState:
                break;
            case YanYun.EndState:
                break;
            default:
                break;
        }
    }

    public void TurnOnYanYunGui(int index)
    {
        
        //---------------------------------服务器UI控制---------------------------------------
        if (isServer)
        {
            //服务器逻辑
            
            //if (index == -1)//关闭Server所有Gui
            //{
            //    foreach (var item in YanYunGameObjects)
            //    {
            //        item.SetActive(false);
            //    }
            //}
            //else
            //{
            //    foreach (var item in YanYunGameObjects)//打开服务器单一UI
            //    {
            //        if (index == YanYunGameObjects.IndexOf(item))
            //        {
            //            item.SetActive(true);
            //        }
            //        else
            //        {
            //            item.SetActive(false);
            //        }
            //    }
            //}
        }
        //---------------------------------服务器UI控制------------------------------------

        //---------------------------------客户端UI控制--------------------------------------------
        else
        {
            if (index == -1)//关闭Client所有Gui
            {
                foreach (var item in GetCurrentLocalPlayer().YanXungameObjects)
                {
                    item.SetActive(false);
                }
            }
            else//打开单一UI
            {
                foreach (var item in GetCurrentLocalPlayer().YanXungameObjects)//打开单一UI
                {
                    if (index == GetCurrentLocalPlayer().YanXungameObjects.IndexOf(item))
                    {
                        item.SetActive(true);
                    }
                    else
                    {
                        item.SetActive(false);
                    }
                }
            }




        }
    }

    public static void ServerYanYunStateSwitch(YanYun _yanYun)
    {
        //服务器UI改变
    }

    public static void ServerCanvasSwitch(State state)
    {
        ServerCanvasCtr.SwitchCanvas(state);
    }

    public static void RegisterPlayer(string _netID,Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;

    }

    public static void UnRegisterPlayer(string _PlayerID) {
        players.Remove(_PlayerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID]; 
    }

    public static Player GetServerPlayer()
    {
        foreach (Player player in players.Values)
        {
 //           Debug.Log(player.M_isServer());

            if (player.isServer)
            {
                return player;
            }
        }
        return null;
    }

    public static Player GetCurrentLocalPlayer()
    {
        foreach (Player player in players.Values)
        {
            //           Debug.Log(player.M_isServer());

            if (player.isLocalPlayer)
            {
                return player;
            }
        }
        return null;

    }

    public static void RegisterPhotoID(string _netID)
    {
        PlayerInfo playerInfo = new PlayerInfo(new Texture2D(10, 10), MAXPLAYER);

        string _playerID = PLAYER_ID_PREFIX + _netID;

        PlayerName_PlayerInfo_KP.Add(_playerID, playerInfo);
    }

    public static void UnRegisterPhotoID(string id)
    {
        PlayerName_PlayerInfo_KP.Remove(id);
    }


    public static Sprite Texture2DtoSprite(Texture2D tex2)
    {
        Sprite s = Sprite.Create(tex2, new Rect(0, 0, tex2.width, tex2.height), Vector2.zero);
        return s;
    }

    public static Texture2D GetTexture2d(byte[] bytes)
    {
        //先创建一个Texture2D对象，用于把流数据转成Texture2D
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(bytes);//流数据转换成Texture2D
        //创建一个Sprite,以Texture2D对象为基础
        return texture;
    }
    public static Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }


    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();

       // Debug.Log(GameManager.instance.syncCharacter.Count);
        for (int i = 0; i < GameManager.instance.syncCharacter.Count; i++)
        {
            GUILayout.TextArea(GameManager.instance.syncCharacter[i].isLocked.ToString());

        }


        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + "   -   " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

[System.Serializable]
public class PlayerInfo
{

    public Texture2D TextureIDPhoto;
    public int JobID;

    

    public PlayerInfo()
    {

    }

    public PlayerInfo(Texture2D _texture2D, int _JobID)
    {
        TextureIDPhoto = _texture2D;
        JobID = _JobID;
    }

    public void setTexture2d(Texture2D tex)
    {
        TextureIDPhoto = tex;
    }

    public Texture2D GetTextureIDPhoto()
    {
        return TextureIDPhoto;
    }

    public void SetJobID(int _jobid) {
        JobID = _jobid;
    }

    public int GetJobID() {
        return JobID;
    }
}