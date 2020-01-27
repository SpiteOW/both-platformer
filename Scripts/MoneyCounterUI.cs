using UnityEngine;
using UnityEngine.UI;

public class MoneyCounterUI : MonoBehaviour {

    private Text moneyText;

    void Awake()
    {
        moneyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "MONEY: " + GameMaster.money.ToString();
    }
}