using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class DestructableObject : MonoBehaviour, IDamagable
{
    public Container Health { get; set; }

    public float waitTimeBetweenSpawning = 0;

    public GameObject onHitEffect;

    public void Hurt(float damageToDeal, Vector3 hitPoint, GameObject sender)
    {
        Health.ModifyValue(-damageToDeal);

        if (Health.empty)
        {
            StartCoroutine(SpawnObjects());
        }
        else
            Instantiate(onHitEffect, hitPoint, Quaternion.Euler(hitPoint - sender.transform.position));
    }

    public GameObject[] toCreate;
    public GameObject[] toDisable;

    public Transform spawnPoint;

    // Start is called before the first frame update
    private void Start()
    {
        Health = new Container(30);
    }

    private IEnumerator SpawnObjects()
    {
        foreach (var item in toDisable)
        {
            item.SetActive(false);
        }

        yield return null;

        foreach (var item in toCreate)
        {
            var forward = spawnPoint.TransformPoint(Vector3.forward);

            var go = Instantiate(item, spawnPoint.position, Quaternion.identity);
            var rigidBody = go.GetComponent<Rigidbody>();

            if (rigidBody != null)
            {
                rigidBody.AddForce(forward * Random.Range(0.01f, 0.02f), ForceMode.Impulse);
            }

            yield return new WaitForSecondsRealtime(waitTimeBetweenSpawning);
        }

        Destroy(this);
    }
}