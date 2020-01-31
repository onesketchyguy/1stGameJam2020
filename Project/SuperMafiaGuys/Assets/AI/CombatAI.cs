using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class CombatAI : SimpleAI
    {
        public Combat.GunManager gun;
        private AgentManager AgentManager;

        private float waitUntilNextShot;

        private Coin combatCoin;

        private Transform GetParent(Transform go)
        {
            return (go.parent == null ? go : GetParent(go.parent));
        }

        private GameObject GetShooter(Vector3 fromPoint)
        {
            var objects = FindObjectsOfType<Combat.GunManager>();

            var closest = objects[0].gameObject;
            var distToClosest = 1000f;

            foreach (var item in objects)
            {
                var obj = GetParent(item.transform);
                if (obj == transform) continue;

                float dist = Vector3.Distance(obj.transform.position, fromPoint);
                if (dist < distToClosest)
                {
                    closest = obj.gameObject;
                    distToClosest = dist;
                }
            }

            return closest.gameObject;
        }

        public override void HeardGunShot(Vector3 point)
        {
            // find a target with a weapon near the shot point
            following = GetShooter(point).transform;
            moveState = MoveState.follow;
            agent.speed = runSpeed;

            UpdateMoveToTarget();
        }

        protected override void Start()
        {
            AgentManager = FindObjectOfType<AgentManager>();

            base.Start();
        }

        protected override void Update()
        {
            if (Health.empty == true)
            {
                Die();

                return;
            }

            base.Update();

            if (agent.velocity == Vector3.zero && following != null)
            {
                transform.LookAt(new Vector3(following.position.x, 0, following.position.z));
            }
        }

        internal override void UpdateMoveToTarget()
        {
            // Attempt to shoot first

            if (following == null)
            {
                base.UpdateMoveToTarget();
                return;
            }

            if (waitUntilNextShot >= Time.time || Health.empty) return;

            // try to get a coin from the manager
            if (combatCoin == null)
            {
                combatCoin = AgentManager.GetCoin();
            }

            if (gun == null || gun.CurrentWeapon == null || combatCoin == null) return;

            RaycastHit objectHit;
            var rayDist = 10;

            Vector3 fwd = gun.CurrentWeapon.transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(gun.CurrentWeapon.transform.position, fwd, out objectHit, rayDist))
            {
                if (objectHit.transform == following.transform)
                {
                    waitUntilNextShot = Time.time + (1 + timeBeforeUpdating);

                    anim.SetTrigger("Shoot");
                    gun.Invoke(nameof(gun.ShootWeapon), 0.1f);
                }
            }

            base.UpdateMoveToTarget();
        }

        internal override void Die()
        {
            gun.fire = false;

            if (combatCoin != null)
            {
                combatCoin.lastAI = this;

                AgentManager.ReturnCoin(combatCoin);
                combatCoin = null;
            }

            base.Die();
        }
    }
}