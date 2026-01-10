using UnityEngine;

public static class DamageSource
{
    public const int INITIALIZE = -2;
    public const int HEAL = -1;
    public const int UNKNOWN = 0;

    public const int FALL_DAMAGE = 1000;
    //public const int LAVA = 1001;
    //public const int SPIKES = 1002;

    public static bool IsHealing(int id) => id == HEAL;
    public static bool IsSystem(int id) => id < 0;
}
