using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Text healthRegenRateText;

    [SerializeField]
    private float healthAddition = 20f;

    [SerializeField]
    private float movementSpeedAddition = 1f;

    [SerializeField]
    private float healthRegenAddition = 0.2f;

    [SerializeField]
    private int upgradeCost = 50;

    private PlayerStats stats;

    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues ()
    {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
        healthRegenRateText.text = "HEALTH REGENERATION RATE: " + stats.healthRegenRate.ToString();
    }

    public void UpgradeHealth ()
    {
        if (GameMaster.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.maxHealth = (int)(stats.maxHealth + healthAddition);

        stats.currentHealth = stats.maxHealth;

        GameMaster.money -= upgradeCost;

        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        if (GameMaster.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.movementSpeed = Mathf.Round(stats.movementSpeed + movementSpeedAddition);

        GameMaster.money -= upgradeCost;

        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }

    public void UpgradeHealthRegen()
    {
        if (GameMaster.money < upgradeCost)
        {
            AudioManager.instance.PlaySound("NoMoney");
            return;
        }

        stats.healthRegenRate = (stats.healthRegenRate + healthRegenAddition);

        GameMaster.money -= upgradeCost;

        AudioManager.instance.PlaySound("Money");

        UpdateValues();
    }
}
