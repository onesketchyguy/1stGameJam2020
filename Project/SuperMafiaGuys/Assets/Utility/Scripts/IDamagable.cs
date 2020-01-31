using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Any damabagle object.
    /// </summary>
    public interface IDamagable
    {
        /// <summary>
        /// The health variable for a given object.
        /// </summary>
        Container Health { get; set; }

        /// <summary>
        /// Damage this object with a specified value.
        /// </summary>
        /// <param name="damageToDeal">Amount of damage to deal. Please ensure you use a posative value here!</param>
        void Hurt(float damageToDeal, UnityEngine.Vector3 hitPoint, UnityEngine.GameObject sender);
    }
}