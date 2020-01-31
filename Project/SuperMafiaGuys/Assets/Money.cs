using UnityEngine;

public class Money : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Player") return;

        MoneyCounter.money += Random.Range(1, 100);

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }

        Destroy(gameObject);
    }
}