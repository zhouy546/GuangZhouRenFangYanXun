using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class YanXunBtnCtrClient : NetworkBehaviour
{
    public List<Button> buttons = new List<Button>();

  [SyncVar]
    public int CharacterID =15;

    [SyncVar]
    public bool Locked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBtnClick(int id)
    {
        CharacterID = id;
    }

    public void Selected()
    {
    }

    public void CmdSelected() {
    }

    public void RpcClient() {

    }

}
