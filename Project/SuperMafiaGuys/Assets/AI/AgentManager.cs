using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameAI
{
    public class AgentManager : MonoBehaviour
    {
        public List<SimpleAI> agents = new List<SimpleAI>();

        private List<Coin> coins = new List<Coin>();

        [SerializeField]
        private int combatCoins = 1;

        public Coin GetCoin()
        {
            if (coins.Count > 0)
            {
                return coins.FirstOrDefault();
            }

            return null;
        }

        public void ReturnCoin(Coin coinToReturn)
        {
            coins.Add(coinToReturn);
        }

        private void Start()
        {
            for (int i = 0; i < combatCoins; i++)
            {
                coins.Add(new Coin());
            }

            var _agents = FindObjectsOfType<SimpleAI>();

            foreach (var item in _agents)
            {
                this.agents.Add(item);
            }
        }

        public void HearGunShot(Vector3 aroundPoint, float range = 10)
        {
            var toRemove = new List<SimpleAI>();

            foreach (var item in agents)
            {
                if (item == null)
                {
                    toRemove.Add(item);
                    continue;
                }

                if (Vector3.Distance(item.transform.position, aroundPoint) <= range)
                {
                    item.HeardGunShot(aroundPoint);
                }
            }

            foreach (var item in toRemove) agents.Remove(item);
        }
    }

    public class Coin
    {
        public SimpleAI lastAI;
    }
}