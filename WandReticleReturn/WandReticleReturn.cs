using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RL2.ModLoader;
using System.Reflection;

namespace WandReticleReturn;

[ModEntrypoint]
public class WandReticleReturn
{
    public static ILHook ReticleReturnHook = new ILHook(
        typeof(MagicWand_Ability).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance),
        ReticleReturn
    );

    public WandReticleReturn() {
        ModLoader.OnLoad += ReticleReturnHook.Apply;
        ModLoader.OnUnload += ReticleReturnHook.Undo;
    }

    public static void ReticleReturn(ILContext il) {
        ILCursor cursor = new ILCursor(il);
        UnityEngine.Debug.Log("[WRR]: Applying edit successfully!");

        if (!cursor.TryGotoNext(MoveType.Before, ins => ins.MatchBrfalse(out _))) return;
        cursor.Emit(OpCodes.Ldc_I4_1);
        cursor.Emit(OpCodes.Or);

        if (!cursor.TryGotoNext(MoveType.After, ins => ins.MatchLdcI4(0))) return;

        cursor.Emit(OpCodes.Ldc_I4_1);
        cursor.Emit(OpCodes.Or);

        UnityEngine.Debug.Log("[WRR]: Edit applied successfully!");
    }
}