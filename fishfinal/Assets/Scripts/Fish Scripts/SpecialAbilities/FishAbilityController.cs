using UnityEngine;

public class FishAbilityController : MonoBehaviour
{
    private ISpecialAbility[] abilities;

    private void Awake()
    {
        abilities = GetComponents<ISpecialAbility>();
    }
    
    public void ActivateAbility(int index)
    {
        if (index < 0 || index >= abilities.Length)
        {
            return;
        }
        var ability = abilities[index];
        if (ability.IsReady)
        {
            ability.Activate();
        }
    }

    public void DeactivateAbility(int index)
    {
        if (index < 0 || index >= abilities.Length)
        {
            return;
        }
        abilities[index].Deactivate();
    }
}
