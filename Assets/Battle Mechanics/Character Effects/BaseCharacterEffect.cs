using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "BaseCharacterEffect", menuName = "Scriptable Objects/BaseCharacterEffect")]
public class BaseCharacterEffect : ScriptableObject
{
    [SerializeField]
    public readonly int MaxCountOfStacs;

    [SerializeField]
    public readonly Image EffectIcon;

    [SerializeField]
    public readonly string EffectName; 

    virtual public void OnEffectAddad(BaseCharacter _character)
    {
        Debug.Log($"Added effect {EffectName}");
    }
}
