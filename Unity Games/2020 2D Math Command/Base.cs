using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField]
    private Sprite healthy = null, destroyed = null;

    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SpriteRenderer shieldSprite = null;

    private bool shielded = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shieldSprite.enabled = false;
    }

    public Vector2 GetPosition() => transform.position;

    public void BlowUp()
    {
        if(shielded)
        {
            shielded = false;
            shieldSprite.enabled = false;
            return;
        }

        spriteRenderer.sprite = destroyed;
        BaseManager.Inst.BaseDestroyed(this);
    }

    public void Restore()
    {
        spriteRenderer.sprite = healthy;
        // maybe restore particle effect
    }

    public void Shield()
    {
        // add shield graphic
        shielded = true;
        shieldSprite.enabled = true;
    }
}