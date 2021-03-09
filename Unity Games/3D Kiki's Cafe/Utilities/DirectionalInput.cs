using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Used to combine D_pad and LeftStickAsButton
public class DirectionalInput : MonoBehaviour
{
    static bool leftPress = false;
    static bool left = false;
    static bool leftRelease = false;
    static bool rightPress = false;
    static bool right = false;
    static bool rightRelease = false;
    static bool upPress = false;
    static bool up = false;
    static bool upRelease = false;
    static bool downPress = false;
    static bool down = false;
    static bool downRelease = false;

    /// <summary>
    /// Equivalent of Input.GetKeyDown(Left Input)
    /// </summary>
    /// <returns></returns>
    public static bool LeftPress() { return leftPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Left Input)
    /// </summary>
    /// <returns></returns>
    public static bool Left(){ return left;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Left Input)
    /// </summary>
    /// <returns></returns>
    public static bool LeftRelease(){ return leftRelease;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyDown(Right Input)
    /// </summary>
    /// <returns></returns>
    public static bool RightPress(){ return rightPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Right Input)
    /// </summary>
    /// <returns></returns>
    public static bool Right(){ return right;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Right Input)
    /// </summary>
    /// <returns></returns>
    public static bool RightRelease(){ return rightRelease;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyDown(Up Input)
    /// </summary>
    /// <returns></returns>
    public static bool UpPress(){ return upPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Up Input)
    /// </summary>
    /// <returns></returns>
    public static bool Up(){ return up;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Up Input)
    /// </summary>
    /// <returns></returns>
    public static bool UpRelease(){ return upRelease;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyDown(Down Input)
    /// </summary>
    /// <returns></returns>
    public static bool DownPress(){ return downPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Down Input)
    /// </summary>
    /// <returns></returns>
    public static bool Down(){ return down;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Down Input)
    /// </summary>
    /// <returns></returns>
    public static bool DownRelease(){ return downRelease;}

    void Update()
    {
        leftPress = D_Pad.LeftPress() || LeftStickAsButton.LeftPress();
        left = D_Pad.Left() || LeftStickAsButton.Left();
        leftRelease = D_Pad.LeftRelease() || LeftStickAsButton.LeftRelease();
        rightPress = D_Pad.RightPress() || LeftStickAsButton.RightPress();
        right = D_Pad.Right() || LeftStickAsButton.Right();
        rightRelease = D_Pad.RightRelease() || LeftStickAsButton.RightRelease();
        upPress = D_Pad.UpPress() || LeftStickAsButton.UpPress();
        up = D_Pad.Up() || LeftStickAsButton.Up();
        upRelease = D_Pad.UpRelease() || LeftStickAsButton.UpRelease();
        downPress = D_Pad.DownPress() || LeftStickAsButton.DownPress();
        down = D_Pad.Down() || LeftStickAsButton.Down();
        downRelease = D_Pad.DownRelease() || LeftStickAsButton.DownRelease();
    }
}
