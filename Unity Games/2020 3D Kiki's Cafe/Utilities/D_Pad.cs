using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Used to convert gamepad D_Pad axes into key presses
 */

public class D_Pad : MonoBehaviour
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
    /// Equivalent of Input.GetKeyDown(Left D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool LeftPress() { return leftPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Left D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool Left(){ return left;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Left D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool LeftRelease(){ return leftRelease;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyDown(Right D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool RightPress(){ return rightPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Right D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool Right(){ return right;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Right D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool RightRelease(){ return rightRelease;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyDown(Up D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool UpPress(){ return upPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Up D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool Up(){ return up;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Up D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool UpRelease(){ return upRelease;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyDown(Down D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool DownPress(){ return downPress;}
    
    /// <summary>
    /// Equivalent of Input.GetKey(Down D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool Down(){ return down;}
    
    /// <summary>
    /// Equivalent of Input.GetKeyUp(Down D_Pad)
    /// </summary>
    /// <returns></returns>
    public static bool DownRelease(){ return downRelease;}

    bool releaseRight = true;
    bool releaseLeft = true;
    bool releaseUp = true;
    bool releaseDown = true;

    void Update()
    {
        float horizontalInput = Input.GetAxis("DPadHorizontal");
        float verticalInput = Input.GetAxis("DPadVertical");
        Left(ref horizontalInput);
        Right(ref horizontalInput);
        Down(ref verticalInput);
        Up(ref verticalInput);
    }

    void Left(ref float input)
    {
        if (input < -0.75f)
        {
            left = true;
            if (releaseLeft) { leftPress = true; }
            else { leftPress = false; }
            releaseLeft = false;
        }
        else
        {
            left = false;
            if (leftRelease) { leftRelease = false; }
            if (!releaseLeft) { releaseLeft = true; leftRelease = true; }
        }
    }

    void Right(ref float input)
    {
        if (input > 0.75f)
        {
            right = true;
            if (releaseRight) { rightPress = true; }
            else { rightPress = false; }
            releaseRight = false;
        }
        else
        {
            right = false;
            if (rightRelease) { rightRelease = false; }
            if (!releaseRight) { releaseRight = true; rightRelease = true; }
        }
    }

    void Up(ref float input)
    {
        if (input > 0.75f)
        {
            up = true;
            if (releaseUp) { upPress = true; }
            else { upPress = false; }
            releaseUp = false;
        }
        else
        {
            up = false;
            if (upRelease) { upRelease = false; }
            if (!releaseUp) { releaseUp = true; upRelease = true; }
        }
    }

    void Down(ref float input)
    {
        if (input < -0.75f)
        {
            down = true;
            if (releaseDown) { downPress = true; }
            else { downPress = false; }
            releaseDown = false;
        }
        else
        {
            down = false;
            if (downRelease) { downRelease = false; }
            if (!releaseDown) { releaseDown = true; downRelease = true; }
        }
    }
}
