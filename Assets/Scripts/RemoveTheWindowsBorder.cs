using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class RemoveTheWindowsBorder : MonoBehaviour
{

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;  //边框用的
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;

    int offset = 2;

    int _posX = -1;
    int _posY = -1;
    public  int _Txtwith;
    public int _Txtheight;
    void Start()
    {


#if !UNITY_EDITOR

        Screen.SetResolution(_Txtwith, _Txtheight, false);
        StartCoroutine("Setposition");
        Cursor.visible = false;
#endif
    }

    private void Update()
    {
        
    }

    IEnumerator Setposition()
    {
        yield return new WaitForSeconds(0.1f);		//不知道为什么发布于行后，设置位置的不会生效，我延迟0.1秒就可以
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);      //无边框
        bool result = SetWindowPos(GetForegroundWindow(), 0, _posX, _posY, _Txtwith+offset, _Txtheight+offset, SWP_SHOWWINDOW);       //设置屏幕大小和位置
    }

    IEnumerator ReSetposition()
    {
        yield return new WaitForSeconds(1f);		//不知道为什么发布于行后，设置位置的不会生效，我延迟0.1秒就可以
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);      //无边框
        bool result = SetWindowPos(GetForegroundWindow(), 0, _posX, _posY, _Txtwith, _Txtheight, SWP_SHOWWINDOW);       //设置屏幕大小和位置
    }

    [System.Runtime.InteropServices.DllImport("user32")]
    private static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    //移动鼠标 
    const int MOUSEEVENTF_MOVE = 0x0001;
    //模拟鼠标左键按下 
    const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    //模拟鼠标左键抬起 
    const int MOUSEEVENTF_LEFTUP = 0x0004;
    //模拟鼠标右键按下 
    const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
    //模拟鼠标右键抬起 
    const int MOUSEEVENTF_RIGHTUP = 0x0010;
    //模拟鼠标中键按下 
    const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    //模拟鼠标中键抬起 
    const int MOUSEEVENTF_MIDDLEUP = 0x0040;
    //标示是否采用绝对坐标 
    const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    //模拟鼠标滚轮滚动操作，必须配合dwData参数
    const int MOUSEEVENTF_WHEEL = 0x0800;


    public static void TestMoveMouse()
    {
        Console.WriteLine("模拟鼠标移动5个像素点。");
        //mouse_event(MOUSEEVENTF_MOVE, 50, 50, 0, 0);//相对当前鼠标位置x轴和y轴分别移动50像素
        mouse_event(MOUSEEVENTF_WHEEL, 0, 0, -20, 0);//鼠标滚动，使界面向下滚动20的高度
    }

}
