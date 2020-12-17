/*
 * SlotReel.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/12/2020 (en-US)
 * Description: "Rotates" reels and adjusts them to snap to a position when stopped
 */

using NaughtyAttributes;
using System.Collections;
using UnityEngine;



public class SlotReel : MonoBehaviour
{
    [SerializeField] RectTransform upperHalfReel;
    [SerializeField] RectTransform lowerHalfReel;
    [Tooltip("Transforms who's position is used to snap a reel to a value when stopped")]
    [SerializeField] Transform[] slots;
    [SerializeField] int smallBetAmount = 1;
    [SerializeField] int mediumBetAmount = 5;
    [SerializeField] int largeBetAmount = 10;
    [Tooltip("Y value for lower half reel to move to be top half reel.")]
    [SerializeField] float adjustPositionThreshold = -300;
    [Tooltip("Distance slot must be from focus before snaping in place.")]
    [SerializeField] float snapDistanceThreshold = 0.75f;
    Transform selectedSlot;
    Slot slot;
    float reelSpeed;
    float spinAfterStopTime;
    float x;
    bool spin = false;
    bool stopped = false;
    bool gameStarted;

    void Start()
    {
        x = upperHalfReel.anchoredPosition.x;
    }
    void Update()
    {
        if(spin)Spin();
        AdjustPosition();
    }

    public bool Spinning() { return spin; }
    public Slot GetSlot() { return slot; }
    public void SetSpeed(float speed) { reelSpeed = speed; }
    public void SetSpinAfterStopTime(float time) { spinAfterStopTime = time; }

    /// <summary>
    /// "Rotates" reels
    /// <para>direction: 1 to "Rotate" down. -1 to "Rotate" up</para>
    /// </summary>
    /// <param name="speedAdjustment"></param>
    /// <param name="direction"></param>
    void Spin(float speedAdjustment = 1, int direction = 1 )
	{
        if(direction > 1) { direction = 1; }
        if(direction < -1) { direction = -1; }
        if(direction == 0) { direction = 1; }
        float lowerY = lowerHalfReel.anchoredPosition.y;
        float upperY = upperHalfReel.anchoredPosition.y;
        Vector2 upperPos = new Vector2(x, upperY - (direction * speedAdjustment * reelSpeed * Time.deltaTime));
        Vector2 lowerPos = new Vector2(x, lowerY - (direction * speedAdjustment * reelSpeed * Time.deltaTime));
        upperHalfReel.anchoredPosition = upperPos;
        lowerHalfReel.anchoredPosition = lowerPos;
    }

    void AdjustPosition()
	{
        if(upperHalfReel.anchoredPosition.y < adjustPositionThreshold)
		{
            float y = upperHalfReel.anchoredPosition.y;
            y += lowerHalfReel.rect.height * 2;
            upperHalfReel.anchoredPosition = new Vector2(x, y);
		}
        if (lowerHalfReel.anchoredPosition.y < adjustPositionThreshold)
        {
            float y = lowerHalfReel.anchoredPosition.y;
            y += (upperHalfReel.rect.height * 2);
            lowerHalfReel.anchoredPosition = new Vector2(x, y);
        }
    }

    public void StartSpining() 
    { 
        spin = true; 
        gameStarted = true;
    }

    public void StopSpinning()
	{
        if (!stopped && gameStarted)
        {
            stopped = true;
            StartCoroutine(SlowSpinningToHalt());
        }
	}

    IEnumerator SlowSpinningToHalt()
	{
        float originalSpeed = reelSpeed;
        float speedMultiplier = 1f;
        for(int i = 0; i < 10; i++)
		{
            speedMultiplier -= 0.05f;
            reelSpeed = reelSpeed * speedMultiplier;
            yield return new WaitForSeconds(spinAfterStopTime/10);
        }
        reelSpeed = originalSpeed;
        spin = false;
        float distance = Vector3.Distance(transform.position, slots[0].position);
        selectedSlot = slots[0];
        foreach(Transform slot in slots)
		{
            float newDistance = Vector3.Distance(transform.position, slot.position);
            if (newDistance < distance)
			{
                selectedSlot = slot;
                distance = newDistance;
			}
		}
        slot = selectedSlot.GetComponent<Slot>();
        bool inPosition = false;
        while(!inPosition)
		{
            distance = Vector3.Distance(transform.position, selectedSlot.position);
            if (distance < snapDistanceThreshold)
            {
                inPosition = true;
                transform.position = selectedSlot.position;
            }
            else
            {
                //Adjust reel up or down at a slower speed to get closer to snap point
                if (selectedSlot.position.y > transform.position.y) { Spin(0.4f); }
                else { Spin(0.4f, -1); }
            }
            yield return new WaitForEndOfFrame();
		}
	}

}