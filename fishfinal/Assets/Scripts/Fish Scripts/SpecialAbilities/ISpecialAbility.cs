using UnityEngine;

public interface ISpecialAbility
{
    void Activate();
    void Deactivate();
    bool IsReady { get; }
    float Cooldown { get; }
}
