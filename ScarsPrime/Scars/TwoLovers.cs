using API = RL2.API;
using Collections = System.Collections.Generic;

public struct TwoLoversPrime : API.IRegistrable
{
    public void Register()
    {
        API.Scars.ModifySummonRules.Event += ModifyTwoLovers;
    }

    private void ModifyTwoLovers(ChallengeType challenge, ref Collections.List<BaseSummonRule> rules)
    {
        if (challenge != ChallengeType.TwoLovers) return;

        var count = rules.Count - 1;
        for (var i = count - 9; i > 6; i--)
        {
            rules.RemoveAt(i);
        }

        Collections.List<BaseSummonRule> newRules = [
            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.DancingBoss, EnemyRank.Advanced)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1 },
            new API.SummonRule.SetSpawnPoints() { SpawnPoints = [3] },
            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.StudyBoss, EnemyRank.Advanced)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1 },
        ];

        rules.InsertRange(6, newRules);
    }
}