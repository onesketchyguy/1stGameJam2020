using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCounter : MonoBehaviour
{
    public static int money = 0;
    private int lastMoney;

    public Text textObject;

    private void Update()
    {
        int displayAmount = lastMoney;

        if (lastMoney != money)
        {
            lastMoney += Mathf.Clamp(money - lastMoney, -1, 1);
        }

        if (lastMoney == 0)
        {
            textObject.text = "";
        }
        else

            textObject.text = $"{GetMoneyCharacter(lastMoney)}{displayAmount}" + (money > lastMoney ? $" {GetMoneyCharacter(money - lastMoney)}{money - lastMoney}" : "");
    }

    private char GetMoneyCharacter(int val)
    {
        if (val <= 5)
            return 'A';
        if (val <= 10)
            return 'J';
        if (val <= 25)
            return 'K';
        if (val <= 50)
            return 'R';
        if (val <= 100)
            return 'm';
        if (val <= 500)
            return 'M';
        if (val <= 1000)
            return 'V';

        return 'N';
    }
}