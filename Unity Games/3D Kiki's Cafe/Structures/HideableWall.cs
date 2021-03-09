using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableWall : MonoBehaviour
{
    public bool IsVisiable = false;
    //MeshRenderer renderer = null;
    [SerializeField] Material solidMat = null;
    [SerializeField] Material transMat = null;
    void Start()
    {
       // renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (IsVisiable) { renderer.material = solidMat; }
        //else { renderer.material = transMat;}
        IsVisiable = true;
    }
}
