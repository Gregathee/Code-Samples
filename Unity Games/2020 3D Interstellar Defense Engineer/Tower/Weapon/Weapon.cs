using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : TowerPart
{
    [SerializeField] protected Transform firePoint = null;
    [SerializeField] GameObject collisionIndicator = null;
    List<Collider> touchingWeapons = new List<Collider>();
    protected bool canFire = true;
    bool isTouchingWeapon = false;
    public Quaternion savedLocalRotation = new Quaternion();

    protected override void Update()
    {
        if (isPreview) base.Update();
        else { CompensateScale(); }
        int i = 0;
        int count = touchingWeapons.Count;
        while(i < count)
        {
            if (touchingWeapons[i] == null){touchingWeapons.RemoveAt(i);}
            else i++;
            count = touchingWeapons.Count;
        }
        if (touchingWeapons.Count > 0)
        {
            collisionIndicator.SetActive(true);
            isTouchingWeapon = true;
        }
        else
        {
            collisionIndicator.SetActive(false);
            isTouchingWeapon = false;
        }
        Light[] lights = GetComponentsInChildren<Light>();
        foreach (Light light in lights) light.gameObject.SetActive(isPreview);
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponent<Weapon>();
        if (weapon && !isPreview) {touchingWeapons.Add(other);}
    }

    private void OnTriggerExit(Collider other)
    {
        Weapon weapon = other.GetComponent<Weapon>();
        if (weapon && !isPreview){touchingWeapons.Remove(other);}
    }

    public void CompensateScale()
    {
        SetShrink(false);
        SetHide(false);
        Transform parent = transform.parent;
        TowerPart parentPart = null;
        if (parent) {parentPart = parent.parent.GetComponent<TowerPart>();}
        if (parentPart)
        {
            transform.localPosition = new Vector3(0, 0.08f, 0);
            PartSize parentSize = parentPart.GetSize();
            switch (parentSize)
            {
                case PartSize.Small:
                    switch (size)
                    {
                        case PartSize.Small: gameObject.transform.localScale = new Vector3(1, 0.5f, 1); break;
                        case PartSize.Medium: gameObject.transform.localScale = new Vector3(2, 1, 2); break;
                        case PartSize.Large: gameObject.transform.localScale = new Vector3(3, 1.5f, 3); break;
                    } break;
                case PartSize.Medium:
                    switch (size)
                    {
                        case PartSize.Small: gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); break;
                        case PartSize.Medium: gameObject.transform.localScale = new Vector3(1, 1, 1); break;
                        case PartSize.Large: gameObject.transform.localScale = new Vector3(1.5f,1.5f,1.5f); break;
                    } break;
                case PartSize.Large:
                    switch (size)
                    {
                        case PartSize.Small:
                            gameObject.transform.localScale = new Vector3(0.333f, 0.5f, 0.333f);
                            break;
                        case PartSize.Medium: gameObject.transform.localScale = new Vector3(0.666f, 1, 0.666f); break;
                        case PartSize.Large: gameObject.transform.localScale = new Vector3(1, 1.5f, 1); break;
                    }break;
            }
        } else base.Update();
    }

    public bool IsTouchingWeapon() { return isTouchingWeapon; }
    public abstract void Fire(Transform target);
    public Transform GetFirePoint() { return firePoint; }
}


