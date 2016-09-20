using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

    public int hitPoints = 2;
    public Sprite damagedSprite;
    public float damageImpactSpeed;

    private int currentHitPoints;
    private float damageImpactSpeedSquare;
    private SpriteRenderer spriteRenderer;

	void Start () {
        currentHitPoints = hitPoints;
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageImpactSpeedSquare = damageImpactSpeed * damageImpactSpeed;
	}
	
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Damager"))
            return;

        if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSquare)
            return;

        spriteRenderer.sprite = damagedSprite;
        currentHitPoints--;

        if (currentHitPoints <= 0)
            Kill();
    }

    void Kill()
    {
        spriteRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<ParticleSystem>().Play();
    }
}
