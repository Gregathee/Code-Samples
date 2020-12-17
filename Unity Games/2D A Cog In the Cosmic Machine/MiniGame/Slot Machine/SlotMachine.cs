/*
 * SlotMachine.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/12/2020 (en-US)
 * Description: Manages slot machine mini game
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Structs used to create collapsable inspector sections
[System.Serializable]
public struct Payouts
{
    public int basePayout111;
    public int basePayout222;
    public int basePayout333;
    public int basePayout444;
    public int basePayout555;
    public int basePayout666;
    public int basePayout11;
    public int basePayout22;
    public int basePayout33;
    public int basePayout44;
    public int basePayout55;
    public int basePayout66;
}

//Structs used to create collapsable inspector sections
[System.Serializable]
public struct PayoutTexts
{
    public TMP_Text basePayout111;
    public TMP_Text basePayout222;
    public TMP_Text basePayout333;
    public TMP_Text basePayout444;
    public TMP_Text basePayout555;
    public TMP_Text basePayout666;
    public TMP_Text basePayout11;
    public TMP_Text basePayout22;
    public TMP_Text basePayout33;
    public TMP_Text basePayout44;
    public TMP_Text basePayout55;
    public TMP_Text basePayout66;
}

//Structs used to create collapsable inspector sections
[System.Serializable]
public struct PayoutMultipliers
{
    public float freePayoutMultiplier;
    public float smallPayoutMultiplier;
    public float mediumPayoutMultiplier;
    public float largePayoutMultiplier;
}

public enum BetAmount { Free, Small, Medium, Large}

public class SlotMachine : MiniGame
{
    [SerializeField] SlotReel[] reels;
    [SerializeField] float spinAfterStopTime = 1;
    [SerializeField] float reelSpeed = 1000;
    [Tooltip("Multiplies the original speed of the remaining two reels after the first one is stopped.")]
    [SerializeField] float firstStopSpeedMultiplier = 1.5f;
    [Tooltip("Multiplies the original speed of the last reel after the first two are stopped.")]
    [SerializeField] float secondStopSpeedMultiplier = 2f;
    [SerializeField] GameObject bettingPanel;
    [SerializeField] Slider crank;
    [SerializeField] Button smallBetButton;
    [SerializeField] Button mediumBetButton;
    [SerializeField] Button largeBetButton;
    [SerializeField] float crankReturnSpeed = 1;
    [SerializeField] int smallBet = 1;
    [SerializeField] int mediumBet = 5;
    [SerializeField] int largeBet = 10;
    [SerializeField] Payouts payouts;
    [SerializeField] PayoutTexts payoutTexts;
    [SerializeField] PayoutMultipliers payoutMultipliers;
    [SerializeField] float winDelay = 1;
    [SerializeField] Button[] buttons;

    bool spinning = false;
    bool gameStarted = false;
    bool gameFinished = false;
    ShipStats shipStats;
    int payout = 0;
    BetAmount betAmount = BetAmount.Free;
    List<int> slotValues = new List<int>();
    bool sound = false;

    void Start() 
    {
        sound = false;
        shipStats = OverclockController.instance.ShipStats();
        foreach (SlotReel reel in reels) { reel.SetSpeed(reelSpeed); }
        foreach (SlotReel reel in reels) { reel.SetSpinAfterStopTime(spinAfterStopTime); }
    }

    void Update()
    {
        if (!bettingPanel.activeInHierarchy) { DetectCrank(); }
        AdjustReelSpeed();
        DetectEndOfGame();
        EnableDisableButtons();
    }

    void EnableDisableButtons()
	{
        smallBetButton.enabled = (shipStats.Credits >= smallBet);
        mediumBetButton.enabled = (shipStats.Credits >= mediumBet);
        largeBetButton.enabled = (shipStats.Credits >= largeBet);
    }

    void DetectCrank()
	{
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != crank.gameObject || !Input.GetKey(KeyCode.Mouse0))
        {
            AudioManager.instance.PlaySFX("Grab Crank");
            crank.value += crankReturnSpeed * Time.deltaTime;
        }
        if (crank.value == 0 && !gameStarted) 
        {
            AudioManager.instance.PlaySFX("Pull Crank");
            StartCoroutine(SpinSound());
            foreach (Button button in buttons) { button.interactable = true; }
            gameStarted = true;
            switch(betAmount)
			{
                case BetAmount.Small: shipStats.UpdateCreditsAmount(-smallBet);  break;
                case BetAmount.Medium: shipStats.UpdateCreditsAmount(-mediumBet);  break;
                case BetAmount.Large: shipStats.UpdateCreditsAmount(-largeBet);  break;
			}
            StartCoroutine(Spin()); 
        }
    }

    void DetectEndOfGame()
	{
        if (!spinning && gameStarted && !gameFinished)
        {
            gameFinished = true;
            foreach (SlotReel reel in reels) { slotValues.Add(reel.GetSlot().GetValue()); }
            PayUp();
            statModification = payout;
            winMessage = "You win " + payout + " credits!";
            StartCoroutine(EndGame());
        }
    }

    void AdjustReelSpeed()
	{
        spinning = false;
        int spinningCount = 0;
        foreach (SlotReel reel in reels) { if (reel.Spinning()) { spinning = true; spinningCount++; } }
        if (spinningCount < reels.Length)
        {
            if (spinningCount == 2) foreach (SlotReel reel in reels) { if (reel.Spinning()) { reel.SetSpeed(reelSpeed * firstStopSpeedMultiplier); } }
            if (spinningCount == 1) foreach (SlotReel reel in reels) { if (reel.Spinning()) { reel.SetSpeed(reelSpeed * secondStopSpeedMultiplier); } }
        }
    }

    void PayUp()
	{
        float payoutMultiplier = 0f;
        int oneCount = 0;
        int twoCount = 0;
        int threeCount = 0;
        int fourCount = 0;
        int fiveCount = 0;
        int sixCount = 0;

        GetMultiplier(ref payoutMultiplier);
        CountResults(ref oneCount, ref twoCount, ref threeCount, ref fourCount, ref fiveCount, ref sixCount);
        float basePayout = GetBasePayout(ref oneCount, ref twoCount, ref threeCount, ref fourCount, ref fiveCount, ref sixCount);
        payout =  Mathf.RoundToInt(basePayout * payoutMultiplier);
	}

    void GetMultiplier(ref float payoutMultiplier)
    {
        switch (betAmount)
        {
            case BetAmount.Free: payoutMultiplier = payoutMultipliers.freePayoutMultiplier; break;
            case BetAmount.Small: payoutMultiplier = payoutMultipliers.smallPayoutMultiplier; break;
            case BetAmount.Medium: payoutMultiplier = payoutMultipliers.mediumPayoutMultiplier; break;
            case BetAmount.Large: payoutMultiplier = payoutMultipliers.largePayoutMultiplier; break;
        }
    }

    void DisplayPayouts()
	{
        float multiplier = 0;
        GetMultiplier(ref multiplier);
        payoutTexts.basePayout11.text = (payouts.basePayout11 * multiplier).ToString();
        payoutTexts.basePayout111.text = (payouts.basePayout111 * multiplier).ToString();
        payoutTexts.basePayout22.text = (payouts.basePayout22 * multiplier).ToString();
        payoutTexts.basePayout222.text = (payouts.basePayout222 * multiplier).ToString();
        payoutTexts.basePayout33.text = (payouts.basePayout33 * multiplier).ToString();
        payoutTexts.basePayout333.text = (payouts.basePayout333 * multiplier).ToString();
        payoutTexts.basePayout44.text = (payouts.basePayout44 * multiplier).ToString();
        payoutTexts.basePayout444.text = (payouts.basePayout444 * multiplier).ToString();
        payoutTexts.basePayout55.text = (payouts.basePayout55 * multiplier).ToString();
        payoutTexts.basePayout555.text = (payouts.basePayout555 * multiplier).ToString();
        payoutTexts.basePayout66.text = (payouts.basePayout66 * multiplier).ToString();
        payoutTexts.basePayout666.text = (payouts.basePayout666 * multiplier).ToString();
    }

    void CountResults(ref int oneCount, ref int twoCount, ref int threeCount, ref int fourCount, ref int fiveCount, ref int sixCount)
	{
        foreach (SlotReel reel in reels)
        {
            switch (reel.GetSlot().GetValue())
            {
                case 1: oneCount++; break;
                case 2: twoCount++; break;
                case 3: threeCount++; break;
                case 4: fourCount++; break;
                case 5: fiveCount++; break;
                case 6: sixCount++; break;
            }
        }
    }
    
    int GetBasePayout(ref int oneCount, ref int twoCount, ref int threeCount, ref int fourCount, ref int fiveCount, ref int sixCount)
	{
        if (oneCount > 1) { if (oneCount > 2) { AudioManager.instance.PlaySFX("Pay111"); return payouts.basePayout111; } else { AudioManager.instance.PlaySFX("Pay11"); return payouts.basePayout11; } }
        if (twoCount > 1) { if (twoCount > 2) { AudioManager.instance.PlaySFX("Pay222"); return payouts.basePayout222; } else { AudioManager.instance.PlaySFX("Pay22"); return payouts.basePayout22; } }
        if (threeCount > 1) { if (threeCount > 2) { AudioManager.instance.PlaySFX("Pay333"); return payouts.basePayout333; } else { AudioManager.instance.PlaySFX("Pay33"); return payouts.basePayout33; } }
        if (fourCount > 1) { if (fourCount > 2) { AudioManager.instance.PlaySFX("Pay444"); return payouts.basePayout444; } else { AudioManager.instance.PlaySFX("Pay44"); return payouts.basePayout22; } }
        if (fiveCount > 1) { if (fiveCount > 2) { AudioManager.instance.PlaySFX("Pay555"); return payouts.basePayout555; } else { AudioManager.instance.PlaySFX("Pay55"); return payouts.basePayout55; } }
        if (sixCount > 1) { if (sixCount > 2) { AudioManager.instance.PlaySFX("Pay666"); return payouts.basePayout666; } else { AudioManager.instance.PlaySFX("Pay66"); return payouts.basePayout66; } }
        AudioManager.instance.PlaySFX("Pay0"); return 0;
	}

    public void Option1()
    {
        bettingPanel.SetActive(false);
        betAmount = BetAmount.Free;
        DisplayPayouts();
        AudioManager.instance.PlaySFX("Free Bet");
    }

    public void Option2()
    {
        bettingPanel.SetActive(false);
        betAmount = BetAmount.Small;
        DisplayPayouts();
        AudioManager.instance.PlaySFX("Small Bet");
    }

    public void Option3()
    {
        bettingPanel.SetActive(false);
        betAmount = BetAmount.Medium;
        DisplayPayouts();
        AudioManager.instance.PlaySFX("Medium Bet");
    }

    public void Option4()
    {
        bettingPanel.SetActive(false);
        betAmount = BetAmount.Large;
        DisplayPayouts();
        AudioManager.instance.PlaySFX("Large Bet");
    }

    IEnumerator Spin()
	{
        foreach (SlotReel reel in reels) 
        {
            reel.StartSpining();
            yield return new WaitForSeconds(0.3f);
        }
	}

    IEnumerator EndGame()
	{
        sound = true;
        yield return new WaitForSeconds(winDelay);
        EndMiniGameSuccess();
    }

    IEnumerator SpinSound()
    {
        if (sound == false)
        {
            AudioManager.instance.PlaySFX("Slot Reel");
            yield return new WaitForSeconds(0.499f);
            StartCoroutine(SpinSound());
        }
    }
}