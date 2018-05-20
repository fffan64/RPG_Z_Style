using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RPGTalkArea))]
public class QuestGiver : MonoBehaviour {

    private RPGTalkArea rPGTalkArea;

    public bool AssignedQuest { get; set; }
    public bool Helped { get; set; }

    [SerializeField]
    private GameObject quests;

    [SerializeField]
    private string questType;
    private Quest Quest { get; set; }


    private void Start()
    {
        rPGTalkArea = GetComponent<RPGTalkArea>();
        rPGTalkArea.txtToParse = Resources.Load<TextAsset>("Dialogs/Quests/" + questType + "_Before");
        rPGTalkArea.rpgtalkTarget.OnNewTalk += Interact;
    }

    public void Interact()
    {
        if(!AssignedQuest && !Helped)
        {
            AssignQuest();
            rPGTalkArea.txtToParse = Resources.Load<TextAsset>("Dialogs/Quests/" + questType + "_While");
        } else if(AssignedQuest && !Helped)
        {
            CheckQuest();
        } else
        {
            Debug.Log("Thanks for that stuff that one time.");
        }
    }

    void AssignQuest()
    {
        AssignedQuest = true;
        Quest = (Quest)quests.AddComponent(System.Type.GetType(questType));
        Quest.OnQuestFinished += Quest_OnQuestFinished;
    }

    private void Quest_OnQuestFinished(Quest quest)
    {
        rPGTalkArea.txtToParse = Resources.Load<TextAsset>("Dialogs/Quests/" + questType + "_After");
    }

    void CheckQuest()
    {
        if (Quest.Completed)
        {
            Quest.GiveReward();
            Helped = true;
            AssignedQuest = false;
            Debug.Log("Thanks for that! Here's your reward.");
        }
        else
        {
            Debug.Log("You're still in the middle of helping me. Get back at it!");
        }
    }
}
