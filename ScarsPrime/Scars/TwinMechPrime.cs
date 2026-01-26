using API = RL2.API;
using Collections = System.Collections.Generic;

public struct TwinMechPrime : API.IRegistrable
{
    public void Register()
    {
        API.Scars.ModifySummonRules.Event += ModifyTwinMech;
    }

    private void ModifyTwinMech(ChallengeType challenge, ref Collections.List<BaseSummonRule> rules)
    {
        if (challenge != ChallengeType.TwinMech) return;

        var count = rules.Count - 1;
        for (var i = count - 10; i > 7; i--)
        {
            rules.RemoveAt(i);
        }

        Collections.List<BaseSummonRule> newRules = [
            new API.SummonRule.SetEnemyLevel() {
                Level = 20,
            },
            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.SpellswordBoss, EnemyRank.Advanced)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },
        ];

        rules.InsertRange(6, newRules);
    }
}