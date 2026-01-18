using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter: MonoBehaviour
{
    [SerializeField]
    private int p_maxHp;

    [SerializeField]
    public readonly string CharacterName;

	[SerializeField]
	public GameObject CharacterModel;

    private int p_currentHp;

    private Dictionary<SelfUsageCharacterEffect, int> p_selfUsageEffectOnCharacter = new();
    private Dictionary<BaseCharacterEffect, int> p_normalEffectsOnCharacter = new();

    void Start()
    {
        p_currentHp = p_maxHp;
    }

    public bool AddSelfUsageEffectStacs(SelfUsageCharacterEffect _effect, int _countOfStacs)
    {
		return AddEffectStacs(p_selfUsageEffectOnCharacter, _effect, _countOfStacs);
	}

	public bool AddCharacterEffectStacs(SelfUsageCharacterEffect _effect, int _countOfStacs)
	{
		return AddEffectStacs(p_normalEffectsOnCharacter, _effect, _countOfStacs);
	}

	public bool RemoveSelfUsageEffectStacs(SelfUsageCharacterEffect _effect, int _countOfStacs)
	{
		return RemoveEffectStacs(p_selfUsageEffectOnCharacter, _effect, _countOfStacs);
	}

	public bool RemoveCharacterEffectStacs(SelfUsageCharacterEffect _effect, int _countOfStacs)
	{
		return RemoveEffectStacs(p_normalEffectsOnCharacter, _effect, _countOfStacs);
	}

	private bool AddEffectStacs<T> (Dictionary<T, int> _effects, T _currentEffect, int _countOfStacs) where T : BaseCharacterEffect
    {
		if (_countOfStacs <= 0)
			Debug.LogWarning($"You try to add wront number {_countOfStacs} of stacs of {_currentEffect.EffectName} to {CharacterName}");

		if (_countOfStacs > _currentEffect.MaxCountOfStacs)
		{
			Debug.LogError($"Too many stacs {_countOfStacs} of effect {_currentEffect.EffectName} with max count of stacs is {_currentEffect.MaxCountOfStacs}");
			return false;
		}

		if (_effects.TryGetValue(_currentEffect, out var effectCurrentCount))
		{
			if (effectCurrentCount + _countOfStacs > _currentEffect.MaxCountOfStacs)
			{
				Debug.LogError($"Too many stacs current({effectCurrentCount}) + added({_countOfStacs}) of effect {_currentEffect.EffectName} with max count of stacs is {_currentEffect.MaxCountOfStacs}");
				return false;
			}
			_effects[_currentEffect] = effectCurrentCount + _countOfStacs;
			return true;
		}

		_effects.Add(_currentEffect, _countOfStacs);
		return true;
	}

	private bool RemoveEffectStacs<T>(Dictionary<T, int> _effects, T _currentEffect, int _countOfStacs) where T : BaseCharacterEffect
	{
		if (_countOfStacs <= 0)
			Debug.LogWarning($"You try to add wront number {_countOfStacs} of stacs of {_currentEffect.EffectName} to {CharacterName}");

		if (_effects.TryGetValue(_currentEffect, out var effectCurrentCount))
		{
			if (effectCurrentCount - _countOfStacs < 0)
				Debug.LogWarning($"Too many stacs current({effectCurrentCount}) - remover({_countOfStacs}) " +
						$"of effect {_currentEffect.EffectName} has been removed with current number of stacs {effectCurrentCount}");

			if(effectCurrentCount - _countOfStacs == 0)
				_effects.Remove(_currentEffect);
			else
				_effects[_currentEffect] = effectCurrentCount - _countOfStacs;
			return true;
		}

		Debug.LogError($"There is no {_currentEffect.EffectName} effect");
		return false;
	}
}
