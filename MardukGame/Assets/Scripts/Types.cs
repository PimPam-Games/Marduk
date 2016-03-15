using UnityEngine;
using System.Collections;

public static class Types{

	public enum Element{None,Fire,Lightning,Cold,Poison};

	public enum SkillsTypes{
		Ranged,
		Melee,
		Utility,
		Aura,
		Support
	}

	public enum SkillsRequirements{
		Bow,
		Sword,
		Axe,
		Mace,
		None
	}

    public enum EnemyTypes
    {
        Common,
        Champion,
        MiniBoss,
        Boss
    }
}
