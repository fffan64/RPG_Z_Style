using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSlayer : Quest {

    private void Start()
    {
        QuestName = "Ultimate Slayer";
        Description = "Kill a bunch of stuff";
        ItemReward = ItemDatabase.Instance.FetchItemBySlug("sword_ordinary");
        ExperienceRewarded = 100;

        Goals.Add(new KillGoal(this, 0, "Kill 2 Evil Tree", false, 0, 2));
        Goals.Add(new CollectionGoal(this, "power_potion", "Find a power potion", false, 0, 1));

        Goals.ForEach(g => g.Init());
    }
}
