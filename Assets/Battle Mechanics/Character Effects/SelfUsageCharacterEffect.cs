using UnityEngine;

[CreateAssetMenu(fileName = "SelfUsageCharacterEffect", menuName = "Scriptable Objects/SelfUsageCharacterEffect")]
public class SelfUsageCharacterEffect : BaseCharacterEffect
{
	virtual public void UseEffect(BaseCharacter _character, int _stacsOfEffect)
	{
		Debug.Log($"Used base self used effect {this.EffectName}");
	}
}
