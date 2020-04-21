using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField ]
    Behaviour[] componentsToDisable;


    Camera sceneCamera;
    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)//关闭非LOCALPLAYER的摄像机和控制
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }

           // GameManager.instance.IniCharacter(0, false, this.name, YanYun.DefaultState);

        }
        else
        {
            if (isServer)//打开场景内摄像机
            {



                for (int i = 0; i < componentsToDisable.Length; i++)
                {
                    componentsToDisable[i].enabled = false;
                }

                ServerCanvasCtr.instance.RegisterServerGuiEvent();
            }
            else//如果是客户端关闭场景摄像机打开自己Player下的摄像机，
            {
                sceneCamera = Camera.main;
                if (sceneCamera != null)
                {
                    sceneCamera.gameObject.SetActive(false);
                }


                Player _player = GetComponent<Player>();
                Debug.Log(_player.isLocalPlayer);
                if (_player.isLocalPlayer)
                {
                    AddClientEvent();
                }

            }
        }




    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();



        GameManager.RegisterPlayer(_netID, _player);

        //初始化信息
        GameManager.RegisterPhotoID(_netID);


    }

    void AddClientEvent()
    {

        Debug.Log(GameManager.GetCurrentLocalPlayer().name);

       GameManager.GetCurrentLocalPlayer().canvasCtr.RegisterClientGuiEvent();

    }

    void RemoveClientListener()
    {
        GameManager.GetCurrentLocalPlayer().canvasCtr.UnRegisterClientGuiEvent();
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("服务开始");
    }

    public override void OnNetworkDestroy()
    {
        base.OnNetworkDestroy();
        

    }


    //void RegisterPlayer() {
    //    string _ID = "Player" + GetComponent<NetworkIdentity>().netId;
    //    transform.name = _ID;
    //}


    private void OnDisable()
    {
        Player _player = GetComponent<Player>();
        Debug.Log(_player.isLocalPlayer);
        if (_player.isLocalPlayer)
        {
            RemoveClientListener();
        }


        if (_player.isLocalPlayer && _player.isServerIni)
        {
            ServerCanvasCtr.instance.UnRegisterServerGuiEvent();
        }


        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);

        }



        GameManager.UnRegisterPlayer(transform.name);
        GameManager.UnRegisterPhotoID(transform.name);



    }
}
