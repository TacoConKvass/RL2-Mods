using API = RL2.API;
using Collections = System.Collections.Generic;

public struct ClosedSpacePrime : API.IRegistrable
{
    public void Register()
    {
        API.Scars.ModifySummonRules.Event += ModifyClosedSpace;
    }

    private void ModifyClosedSpace(ChallengeType challenge, ref Collections.List<BaseSummonRule> rules)
    {
        if (challenge != ChallengeType.SmallChest) return;

        var count = rules.Count - 1;
        for (var i = count - 10; i > 6; i--)
        {
            rules.RemoveAt(i);
        }

        Collections.List<BaseSummonRule> newRules = [
            new API.SummonRule.SetEnemyLevel()
            {
                SetToRoomLevel = true,
                Level = 37,
            },
            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.MimicChestBoss, EnemyRank.Advanced)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1 },
        ];

        rules.InsertRange(6, newRules);
    }
}