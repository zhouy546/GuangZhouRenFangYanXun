
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum State { DefautState,VideoPlayState, YanXunState, QAState }
public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    private const string PLAYER_ID_PREFIX = "Player";

    public static Dictionary<string, Player> players = new Dictionary<string, Player>();



    [SyncVar]
    public  State GameState = State.DefautState;

    //同步ListStructure--------------------------------
    public struct characters
    {
        public int id;
        public bool isLocked;
    };

    public class SyncCharacter : SyncListStruct<characters> { }


    public SyncCharacter syncCharacter = new SyncCharacter();
    //同步ListStructure--------------------------------



        //初始化15个角色
    public void IniCharacter() {
        for (int i = 0; i < 15; i++)
        {
            characters cha = new characters();
            cha.id = 0;
            cha.isLocked = false;
            syncCharacter.Add(cha);
        }
    }

    public void Start()
    {
        instance = this;
        IniCharacter();

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

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        GUILayout.TextArea(GameManager.instance.syncCharacter[0].isLocked.ToString());
        GUILayout.TextArea(GameManager.instance.syncCharacter[1].isLocked.ToString());


        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + "   -   " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
