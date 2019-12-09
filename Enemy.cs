using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;

        private int _currentHealth;
        public int currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

        public void Init()
        {
            currentHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    public Transform deathParticles;

    public float shakeAmount = 0.1f;
    public float shakeLength = 0.1f;

    public string deathSoundName = "Explosion";

    public int moneyDrop = 10;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    [SerializeField]
    private PlayerStats playerStats;

    void Start()
    {
        stats.Init();

        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if(deathParticles == null)
        {
            Debug.LogError("No deathParticles referenced on Enemy");
        }
    }

    void OnUpgradeMenuToggle(bool active)
    {
        if(this.gameObject != null)
        {
            GetComponent<EnemyAI>().enabled = !active;
        }
    }

    public void DamageEnemy(int damage) {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }

    private void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(9999999);
        }
    }

    private void OnDestroy()
    {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
