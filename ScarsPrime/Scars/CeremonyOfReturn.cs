using API = RL2.API;
using Collections = System.Collections.Generic;

public struct CeremonyOfReturn : API.IRegistrable
{
    public void Register()
    {
        API.Scars.ModifySummonRules.Event += ModifyArmada;
    }

    private void ModifyArmada(ChallengeType challenge, ref Collections.List<BaseSummonRule> rules)
    {
        if (challenge != ChallengeType.BigBattle) return;

        var count = rules.Count - 1;
        for (var i = count - 10; i > 6; i--)
        {
            rules.RemoveAt(i);
        }

        Collections.List<BaseSummonRule> newRules = [
            new API.SummonRule.DisplayObjectiveComplete() {
                HUDType = ObjectiveCompleteHUDType.Scar,
                BossLocIDOverride = "May the Ceremony of Return... begin!",
            },
            new API.SummonRule.SetEnemyLevel()
            {
                Level = 12,
                SetToRoomLevel = false
            },

            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.SpellswordBoss, EnemyRank.Basic)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },

            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.SkeletonBossA, EnemyRank.Basic)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },

            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.DancingBoss, EnemyRank.Basic)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },

            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.StudyBoss, EnemyRank.Basic)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },

            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.EyeballBoss_Bottom, EnemyRank.Basic)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },

            new API.SummonRule.SetEnemyPool()
            {
                EnemyPool = [new(EnemyType.CaveBoss, EnemyRank.Basic)],
                IsBiomeSpecific = false,
                FlyingOnly = false
            },
            new API.SummonRule.SummonEnemies() { SummonValue = 1, SummonDelay = 0, RandomizeOnce = true, SpawnFast = false, SpawnAsCommander = false },
        ];

        rules.InsertRange(6, newRules);
    }
}