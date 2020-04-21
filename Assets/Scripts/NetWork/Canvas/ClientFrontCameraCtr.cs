using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ClientFrontCameraCtr : NetworkBehaviour
{

    private bool camAvailable;
    private WebCamTexture frontcam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    public Button OpenCameraBtn;
    public Button CloseCameraBtn;
        // Start is called before the first frame update
        [Client]
    private void Start()
    {
        Debug.Log("初始化摄像机按钮");

        if (!isServer && isLocalPlayer)
        {
            Debug.Log("添加摄像机按钮事件");
            OpenCameraBtn.onClick.AddListener(initializedCam);
            CloseCameraBtn.onClick.AddListener(TurnOffCam);
        }
    }

    void OnEnable()
    {

    }

    [Client]
    public void initializedCam() {
        Debug.Log("尝试初始化摄像头");

        Debug.Log("我是不是服务器:" + isServer + "   我是不是本地玩家：" + isLocalPlayer);
        if (!isServer && isLocalPlayer)
        {
            Debug.Log("初始化摄像头");
            defaultBackground = background.texture;
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                camAvailable = false;
                return;
            }

            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].isFrontFacing)
                {
                    frontcam = new WebCamTexture(devices[i].name, Screen.width /10 , Screen.height / 10);
                 
                }
            }

            if (frontcam == null)
            {
                Debug.Log("找不到前置摄像头");
            }

            frontcam.Play();
            background.texture = frontcam;

            camAvailable = true;
        }
    }

    [Client]
    public void TurnOffCam()
    {
        if (!isServer && isLocalPlayer)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            if (devices.Length == 0)
            {
                return;
            }
                frontcam.Stop();
            camAvailable = false;



            byte[] textureData = GameManager.TextureToTexture2D(background.texture).EncodeToJPG();
            //NetworkWriter writer = new NetworkWriter();
            //writer.WriteBytesFull(textureData);
            Debug.Log("给服务器发送图片Bytes");
          CmdUpdateCharacterPhoto(textureData);


            GameManager.characters temp = GameManager.instance.getCharacterByName(this.name);

            temp.YanYunState = YanYun.PlayerSelectCharacterState;

            int index = GameManager.instance.getCharacterIndex(temp);

            Debug.Log("index is : "+index);
            Debug.Log("character长度" + GameManager.instance.syncCharacter.Count);


            //改变Client的YANXUN状态到选人
            GameManager.instance.syncCharacter[index] = temp;

            Debug.Log(GameManager.instance.syncCharacter[index].YanYunState);

            GameManager.instance.OnYanYunStateChange(YanYun.PlayerSelectCharacterState);
        }
    }




    [Command(channel = 0)]
    public void CmdUpdateCharacterPhoto(byte[] readerBytes)
    {

        //改变Command的YANXUN状态
        Debug.Log("服务器接收BYTES并转换成TEXTURE2D");
        Texture2D texture2D = GameManager.GetTexture2d(readerBytes);

        //foreach (var item in GameManager.PlayerName_PlayerInfo_KP)
        //{
        //    Debug.Log(item);
        //}
        //Debug.Log(GameManager.PlayerName_PlayerInfo_KP.ContainsKey(transform.name));

        GameManager.PlayerName_PlayerInfo_KP[transform.name].setTexture2d(texture2D);

         //ServerCanvasCtr.instance.UpdateServerPhoto();

        RpcSetClientIDPhot(readerBytes);

        //一个人进入不代表所有人进入
        //GameManager.instance.YanYunState = YanYun.PlayerSelectCharacterState;

    }

    [ClientRpc]
    public void RpcSetClientIDPhot(byte[] texture2DByteRates)
    {
            Debug.Log("给所有Client同步图片");
        Texture2D texture2D = GameManager.GetTexture2d(texture2DByteRates);
        GameManager.PlayerName_PlayerInfo_KP[transform.name].setTexture2d(texture2D);

        //不用修改状态RPC给所有CLIENT 否则就是当一个人拍照连入后所有人一起进入下一个状态
        //GameManager.instance.YanYunState = YanYun.PlayerSelectCharacterState;
    }

    // Update is called once per frame
    [Client]
    void Update()
    {
        if (!camAvailable)
            return;


        if (isServer && isLocalPlayer)
        {
            return;
        }

        if (!isServer && isLocalPlayer)
        {
            float ratio = (float)frontcam.width / (float)frontcam.height;
            fit.aspectRatio = ratio;

            float scaleY = frontcam.videoVerticallyMirrored ? -1 : 1;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
            //Debug.Log("更新摄像头");
            int orient = frontcam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
        }

    }



}
