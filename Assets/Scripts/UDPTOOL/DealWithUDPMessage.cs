
//*********************❤*********************
// 
// 文件名（File Name）：	DealWithUDPMessage.cs
// 
// 作者（Author）：			LoveNeon
// 
// 创建时间（CreateTime）：	Don't Care
// 
// 说明（Description）：	接受到消息之后会传给我，然后我进行处理
// 
//*********************❤*********************

using System.Collections;
using UnityEngine;

public class DealWithUDPMessage : MonoBehaviour
{



    public static DealWithUDPMessage instance;
    // public GameObject wellMesh;
    private string dataTest;


    //private static bool isInScreenProtect=true;


    //public LogoWellCtr logoWellCtr;
    //private bool enterTrigger, exitTrigger;
    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="_data"></param>
    public void MessageManage(string _data)
    {

    //    Debug.Log("DEALWITH MSG " + _data);
        if (_data != "")
        {

            dataTest = _data;

            if (dataTest == "1000")
            {
                PlayerChangeState(State.VideoPlayState);
            }
            else if (dataTest == "1001")
            {
                PlayerChangeState(State.YanXunState);
            }
            else if (dataTest == "1002")
            {
                PlayerChangeState(State.QAState);
            }
            else if (dataTest == "1003")
            {
                PlayerChangeState(State.DefautState);
            }
        }

    }
 
    public void PlayerChangeState(State _num)
    {
        GameManager.GetServerPlayer().ServerSetState(_num);
    }

    private void Awake()
    {

    }

    public IEnumerator Initialization() {
        if (instance == null)
        {
            instance = this;
        }
        yield return new  WaitForSeconds(0.01f);
    }

    public void Start()
    {

    }


    private void Update()
    {


        //Debug.Log("数据：" + dataTest);  
    }

    

}
