using UnityEngine;
public interface IDamageable
{
    void TakeDamage(float amount, int sourceID);
    void Heal(float amount);
}