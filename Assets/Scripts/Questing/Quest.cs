using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Quest : MonoBehaviour {
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string QuestName { get; set; }
    public string Description { get; set; }
    public int ExperienceRewarded { get; set; }
    public Item ItemReward { get; set; }
    public bool Completed { get; set; }

    public delegate void QuestEventHandler(Quest quest);
    public static event QuestEventHandler OnQuestFinished;
    public static event QuestEventHandler OnGiveQuestExperience;

    public static void QuestFinished(Quest quest)
    {
        OnQuestFinished?.Invoke(quest);
    }

    public static void GiveQuestExperience(Quest quest)
    {
        OnGiveQuestExperience?.Invoke(quest);
    }

    public void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);
        if (Completed)
        {
            Debug.Log("Quest: " + QuestName + " finished !");
            QuestFinished(this);
        }
    }

    public void GiveReward()
    {
        if(ItemReward != null)
        {
            Inventory.Instance.AddItem(ItemReward.ID);
        }
        if(ExperienceRewarded != 0)
        {
            GiveQuestExperience(this);
        }
    }
}
