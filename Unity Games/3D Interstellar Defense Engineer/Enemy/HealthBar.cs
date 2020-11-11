using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]GameObject frontPlate;
    [SerializeField]SpriteRenderer sprite;

    void Update()
    {
        transform.LookAt(GameObject.Find("Main Camera").transform.position);
    }

    public void SetStatus(float percent)
    {
        frontPlate.transform.localScale = new Vector3(percent * 2, transform.localScale.y, transform.localScale.z);
        if(percent >= 0.5f) 
        {
            float numurator = (percent * 100) - 50;
            float newPercent = numurator / 50;
            sprite.color = new Color(1-newPercent, 1, 0, 1); 
        }
        else
        {
            float numurator = (percent * 100) + 50;
            float newPercent = numurator / 100;
            sprite.color = new Color(1, newPercent, 0, 1);
        }
    }
}
