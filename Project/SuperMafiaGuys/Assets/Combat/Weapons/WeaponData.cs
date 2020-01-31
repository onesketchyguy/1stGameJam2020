using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "New Weapon")]
    public class WeaponData : ScriptableObject
    {
        public GameObject weaponMesh;

        public AmmoType ammoType;

        [Space]
        [Range(1, 60)]
        public int fireRate = 1;
    }
}