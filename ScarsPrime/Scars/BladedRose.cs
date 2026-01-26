using API = RL2.API;
using Collections = System.Collections.Generic;

public struct BladedRosePrime : API.IRegistrable
{
    public void Register()
    {
        API.Scars.ModifySummonRules.Event += ModifyBladedRose;
    }

    private void ModifyBladedRose(ChallengeType challenge, ref Collections.List<BaseSummonRule> rules)
    {
        if (challenge != ChallengeType.BrotherAndSister) return;

        var count = rules.Count - 1;
        for (var i = count - 10; i > 6; i--)
        {
            rules.RemoveAt(i);
        }

        Collections.List<BaseSummonRule> newRules = [
            new API.SummonRule.SetEnemyLevel()
            {
                SetToRoomLevel = true,
                Level = 24,
            },
            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.SpellswordBoss, EnemyRank.Advanced)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1 },
            new API.SummonRule.SetSpawnPoints() { SpawnPoints = [ 3 ] },
            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.DancingBoss, EnemyRank.Advanced)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1 },
        ];

        rules.InsertRange(6, newRules);
    }
}