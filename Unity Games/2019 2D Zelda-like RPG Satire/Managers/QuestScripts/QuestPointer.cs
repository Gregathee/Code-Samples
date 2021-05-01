using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestPointer : MonoBehaviour
{
    [SerializeField] Transform target;
    SpriteRenderer pointer;
    public float hideDistance;
    public bool isOnQuest;

    private void Start()
    {
        pointer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (GameManager.gameManager.QuestPointerVersion())
        {
            if (isOnQuest && target != null)
            {
                if (Vector2.Distance(transform.position, target.position) < hideDistance)
                { SetChildrenActive(false); }
                else
                {
                    SetChildrenActive(true);
                    transform.up = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, 0);
                }
            }
            else { SetChildrenActive(false); }
        }
        else SetChildrenActive(false);
    }

    void SetChildrenActive(bool value)
    {
        foreach (Transform child in transform){ child.gameObject.SetActive(value);}
    }

    public void SetTarget(Transform target) { this.target = target; }
    public Transform GetTarget() { return target; }
}