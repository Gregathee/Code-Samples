using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour, IQuestCompleter, IQuestGiver
{
    [SerializeField] Dialog funnyDialog;
    [SerializeField] Dialog notFunnyDialog;
    [SerializeField] TextMeshProUGUI textBox = null;
    [SerializeField] GameObject dialogBox = null;
    [SerializeField] QuestGiver questGiver = null;
    [SerializeField] GameObject Shirt = null;
    [SerializeField] GameObject Hair = null;
    [SerializeField] GameObject Pants = null;
    [SerializeField] GameObject talkIndicator = null;
    [SerializeField] GameObject questIndicator = null;
    [SerializeField] Sprite[] shirts = null;
    [SerializeField] Sprite[] dresses = null;
    [SerializeField] Sprite[] pants = null;
    [SerializeField] Sprite[] boyHair = null;
    [SerializeField] Sprite[] girlHair = null;
    [SerializeField] bool isBoy = false;
    [SerializeField] int skinColorIndex = 0;
    [SerializeField] int girlhairIndex = 0;
    [SerializeField] int shirtIndex = 0;
    [SerializeField] int dressIndex = 0;
    [SerializeField] int pantsIndex = 0;
    [SerializeField] int boyHairIndex = 0;
    Color[] skinColors = new Color[4];
    float questIndicatorDistance = 1.1f;

    private void Start()
    {
        talkIndicator.SetActive(false);
        skinColors[0] = new Color(0.9f, 0.5f, 0.3f);
        skinColors[1] = new Color(0.6f, 0.25f, 0);
        skinColors[2] = new Color(1, 0.8f, 0.4f);
        skinColors[3] = new Color(1,1,1);
        if (skinColorIndex > skinColors.Length - 1) skinColorIndex = 0;
        if (girlhairIndex > girlHair.Length - 1) girlhairIndex = 0;
        if (shirtIndex > shirts.Length - 1) shirtIndex = 0;
        if (dressIndex > dresses.Length - 1) dressIndex = 0;
        if (boyHairIndex > boyHair.Length - 1) boyHairIndex = 0;
        if (pantsIndex > pants.Length - 1) pantsIndex = 0;
        transform.Find("Body").GetComponent<SpriteRenderer>().color = skinColors[skinColorIndex];
        RandomizeClothes();
    }

    //Determines if it is the quest target and activates arrow if player is within range
    private void Update()
    {
        Transform questTarget = null;
        Quest quest = QuestManager.questManager.GetPrimaryQuest();
        if (quest != null)
        {
            questTarget = QuestManager.questManager.GetPrimaryQuest().GetQuestTarget();
        }
        if (questTarget != null)
        {
            Vector2 playerPosition = GameManager.player.transform.position;
            float distanceFromPlayer = Vector2.Distance(playerPosition, transform.position);
            bool showIndicator = distanceFromPlayer < questIndicatorDistance;
            bool isTaget = questTarget == transform;
            bool pointerVersion = GameManager.gameManager.QuestPointerVersion();
            if ( isTaget&& showIndicator && pointerVersion){ questIndicator.SetActive(true);}
            else questIndicator.SetActive(false);
        }
        else questIndicator.SetActive(false);
    }

    //IQuestCompleter and IQuestGiver functions
    public Quest GiveQuest() { return questGiver.GiveQuest(); }
    public int GetQuestNumber() { return questGiver.GetQuestNumber(); }
    public int GetQuestPrereqNumber() { return questGiver.GetQuestPrereqNumber(); }
    public bool HasQuest() { return questGiver.HasQuest(); }
    public void IncrementQuestIndex() { questGiver.IncrementQuestIndex(); }
    public string GetName() { return name; }
    public string GetQuestCompleterName() { return gameObject.name; }

    public void ShowTalkIndicator() { talkIndicator.SetActive(true); }
    public void HideTalkIndicator() { talkIndicator.SetActive(false); }

    //Plays dialog based on game version
    public void PromptDialog() 
    {
        if (GameManager.gameManager.FunnyVersion())
        {
            funnyDialog.PlayDialog(ref textBox, ref dialogBox);
        }
        else 
        {
            notFunnyDialog.PlayDialog(ref textBox, ref dialogBox);
        }
    }

    void RandomizeClothes()
    {
        SpriteRenderer shirtRenderer = Shirt.GetComponent<SpriteRenderer>();
        SpriteRenderer hairRenderer = Hair.GetComponent<SpriteRenderer>();
        if (isBoy)
        {
            shirtRenderer.sprite = shirts[shirtIndex];
            hairRenderer.sprite = boyHair[boyHairIndex];
        }
        else
        {
            shirtRenderer.sprite = dresses[dressIndex];
            hairRenderer.sprite = girlHair[girlhairIndex];
        }
        Pants.GetComponent<SpriteRenderer>().sprite = pants[pantsIndex];
        if (gameObject.name == "OldMan") Hair.GetComponent<SpriteRenderer>().color = new Vector4(0, 0, 0, 0);
    }
}
