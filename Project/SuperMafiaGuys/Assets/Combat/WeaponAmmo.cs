namespace Combat
{
    public enum AmmoType
    {
        pistol,
        shotgun,
        machinegun
    }

    [System.Serializable]
    public class WeaponAmmo
    {
        public AmmoType type = AmmoType.pistol;
        public int count = 20;

        public WeaponAmmo(AmmoType type, int count)
        {
            this.type = type;
            this.count = count;
        }
    }
}