﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public int damage = 10;
	public LayerMask whatToHit;

	public Transform bulletTrailPrefab;
    public Transform hitPrefab;
    public Transform muzzleFlashPrefab;
    private float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    // Handle camera shaking
    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    private CameraShake camShake;

    public string weaponShootSound = "DefaultShot";

	private float timeToFire = 0;
	private Transform firePoint;

    private PlayerStats playerStats;

    // Caching
    AudioManager audioManager;

	// Use this for initialization
	void Awake () {
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("No firepoint? WHAT?!");
		}
	}

    void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if (camShake == null)
            Debug.LogError("No CameraShake found on GM object");

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("FREAK OUT! No audioManager found in scene");
        }
    }

    // Update is called once per frame
    void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
				timeToFire = Time.time + 1 / fireRate;
				Shoot ();
			}
		}
	}

    void Shoot () {
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);

	    Debug.DrawLine (firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                //Debug.Log("We hit " + hit.collider.name + " and did " + damage + " damage.");
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null) {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }																										

	void Effect (Vector3 hitPos, Vector3 hitNormal) {
        Transform trail = Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.04f);

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Transform hitParticle = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }

		Transform clone = Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (1.2f, 1.5f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy(clone.gameObject, 0.02f);

        // Shake the camera
        camShake.Shake(camShakeAmount, camShakeLength);

        // Play shoot sound
        audioManager.PlaySound(weaponShootSound);
	}
}