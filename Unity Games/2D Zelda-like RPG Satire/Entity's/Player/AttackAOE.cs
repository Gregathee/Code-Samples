using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to determine if entities take damange. Now used to detect and interact with NPCs
public class AttackAOE : MonoBehaviour
{
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;
    [SerializeField] public Transform entityTransform = null;
    bool NPCInBounds = false;
    NPC dialogNPC;
    BoxCollider2D NPCCollider;

    void Update() { AdjustPositionToEntity();}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "NPC") 
        { 
            dialogNPC = collision.gameObject.GetComponent <NPC>();
            dialogNPC.ShowTalkIndicator();
            NPCCollider = dialogNPC.GetComponent<BoxCollider2D>();
            if(GetComponent<Collider2D>().IsTouching(NPCCollider))NPCInBounds = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NPC npc = collision.gameObject.GetComponent<NPC>();
        if(npc != null)
        { 
            NPCInBounds = false;
            npc.HideTalkIndicator();
            NPCCollider = null;
        }
    }

    void AdjustPositionToEntity()
    {
        float x = entityTransform.position.x + xOffset;
        float y = entityTransform.position.y + yOffset;
        Vector2 pos = new Vector2(x, y);
        transform.position = pos;
    }

    public bool ActivateNPC()
    {
        if (NPCInBounds)
        {
            QuestManager.questManager.CompleteQuest(dialogNPC);
            dialogNPC.PromptDialog();
            StartCoroutine(GetQuestAfterTalk());
        }
        return NPCInBounds;
    }

    IEnumerator GetQuestAfterTalk()
    {
        while(GameManager.gameManager.NPCIsTalking())
        {
            yield return null;
        }
        
        if (dialogNPC.HasQuest())
        { QuestManager.questManager.GetQuestFromQuestGiver(dialogNPC); }
    }
}
