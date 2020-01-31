using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace GameAI
{
    public class SimpleAI : MonoBehaviour, IDamagable
    {
        public GameObject SpawnOnDeath;

        public NavMeshAgent agent;

        public Animator anim;

        public MoveState moveState = MoveState.wander;

        public Transform following;

        private Vector3 dangerPoint;
        private Vector3 targetPos;
        public float timeBeforeUpdating = 1f;

        public float runSpeed = 5;
        public float walkSpeed = 2;

        public float followDistance = 10;

        private float currentSpeed
        {
            get
            {
                var maxSpeed = runSpeed;

                return (agent.velocity.magnitude / maxSpeed);
            }
        }

        public int startingHealth = 25;

        public Container Health { get; set; }

        public enum MoveState
        {
            idle,
            wander,
            follow,
            fleeing
        }

        protected virtual void Start()
        {
            Health = new Container(startingHealth);

            targetPos = transform.position;
            InvokeRepeating(nameof(UpdateMoveToTarget), timeBeforeUpdating, timeBeforeUpdating);
        }

        protected virtual void Update()
        {
            anim.SetFloat("Speed", currentSpeed);
        }

        public virtual void HeardGunShot(Vector3 point)
        {
            dangerPoint = point;

            moveState = MoveState.fleeing;

            CancelInvoke(nameof(UpdateMoveToTarget));
            InvokeRepeating(nameof(UpdateMoveToTarget), 0f, 0f);
        }

        internal virtual void UpdateMoveToTarget()
        {
            if (agent == null) return;

            if (moveState == MoveState.fleeing)
            {
                while (Vector3.Distance(targetPos, dangerPoint) < 10)
                {
                    var ran = Random.insideUnitCircle * (15 + Vector3.Distance(transform.position, dangerPoint));

                    targetPos = transform.position + new Vector3(ran.x, 0, ran.y);
                }

                agent.speed = runSpeed;
                agent.SetDestination(targetPos);

                return;
            }

            if (Vector3.Distance(transform.position, targetPos) <= agent.stoppingDistance + 0.1f || currentSpeed == 0)
            {
                bool wander = (following == null ?
                    (Random.value > 0.5f)
                    : false);

                switch (moveState)
                {
                    case MoveState.idle:
                        // stand still
                        agent.speed = walkSpeed;

                        targetPos = transform.position;

                        if (wander)
                        {
                            moveState = MoveState.wander;
                        }

                        break;

                    case MoveState.wander:
                        // find a random pos
                        agent.speed = walkSpeed;

                        var ran = Random.insideUnitCircle * 10;

                        targetPos = transform.position + new Vector3(ran.x, 0, ran.y);

                        if (wander)
                        {
                            moveState = MoveState.idle;
                        }

                        break;

                    case MoveState.follow:
                        // follow a target
                        targetPos = Vector3.MoveTowards(following.transform.position, transform.position, followDistance);
                        break;

                    default:
                        break;
                }

                if (agent.isOnNavMesh == false)
                {
                    return;
                }

                if (agent.CalculatePath(targetPos, agent.path))
                    agent.SetDestination(targetPos);
            }
        }

        public GameObject onHitEffect;

        public void Hurt(float damageToDeal, Vector3 hitPoint, GameObject sender)
        {
            Health.ModifyValue(-damageToDeal);

            if (Health.empty)
            {
                Die();
            }

            Instantiate(onHitEffect, hitPoint, Quaternion.Euler(hitPoint - sender.transform.position));
        }

        internal virtual void Die()
        {
            CancelInvoke(nameof(UpdateMoveToTarget));

            if (agent.isOnNavMesh == true)
                agent.isStopped = true;

            anim.SetTrigger("Die");

            if (SpawnOnDeath != null)
            {
                if (Random.value > 0.5f)
                    Instantiate(SpawnOnDeath, transform.TransformPoint(Vector3.up), Quaternion.identity);
            }

            Destroy(GetComponent<Collider>());

            Destroy(this);
        }
    }
}