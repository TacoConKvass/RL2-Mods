using System;

namespace Arsenal.Config;

[Serializable]
public class ArsenalConfig
{
    public ModeConfig WeaponsOnly;
	public ModeConfig SpellsOnly;
	public ModeConfig TalentsOnly;
}

[Serializable]
public class ModeConfig
{
    public bool AppliesToAllClasses;
    public ClassType[] AppliesToClasses;
}