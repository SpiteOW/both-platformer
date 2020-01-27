using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{

    public int fallBoundry = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats stats;

    void Start()
    {
        stats = PlayerStats.instance;

        stats.currentHealth = stats.maxHealth;

        if (statusIndicator == null)
        {
            Debug.LogError("No StatusIndicator referenced on player!");
        }
        else
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("PANIC! No audioManager in scene");
        }

        InvokeRepeating("RegenHealth", 1f/stats.healthRegenRate, 1f/stats.healthRegenRate);
    }

    void RegenHealth ()
    {
        stats.currentHealth += 1;
        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }

    void Update()
    {
        if (transform.position.y <= -20)
        {
            DamagePlayer(9999999);
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

public void DamagePlayer(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            // Play death sound
            audioManager.PlaySound(deathSoundName);

            // Kill player
            GameMaster.KillPlayer(this);
        }
        else
        {
            // Play damage sound
            audioManager.PlaySound(damageSoundName);
        }

        statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
    }
}
