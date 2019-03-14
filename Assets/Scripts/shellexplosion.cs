using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellexplosion : MonoBehaviour
{
    public string ennemieTag;
    public LayerMask ennemieMask;
    public ParticleSystem explosionParticule;
    public AudioSource explosionAudio;
    public float maxDamage = 100f;
    public float explosionForce = 1000f;
    public float maxLifeTime = 2f;
    public float explosionRadius = 5f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, maxLifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, ennemieMask);
        Vector3 Hitpoint = new Vector3(1,1,1);

        // Go through all the colliders...
        for (int i = 0; i < colliders.Length; i++)
        {
            // ... and find their rigidbody.
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            // If they don't have a rigidbody, go on to the next collider.
            if (!targetRigidbody)
                continue;

            // Add an explosion force.
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // Find the ennemyHealth script associated with the rigidbody.
            EnemyHealth targetHealth = targetRigidbody.GetComponent<EnemyHealth>();

            // If there is no TankHealth script attached to the gameobject, go on to the next collider.
            if (!targetHealth)
                continue;

            if (targetRigidbody.GetComponent<Collider>().gameObject.tag != ennemieTag)
                continue;
 
            // Calculate the amount of damage the target should take based on it's distance from the shell.
              int damage = CalculateDamage(targetRigidbody.position);

            // Deal this damage to the ennemy.
            targetHealth.TakeDamage(damage, Hitpoint);
        }

        // Unparent the particles from the shell.
        explosionParticule.transform.parent = null;

        // Play the particle system.
        explosionParticule.Play();

        // Play the explosion sound effect.
        explosionAudio.Play();

        // Once the particles have finished, destroy the gameobject they are on.
        Destroy(explosionParticule.gameObject, explosionParticule.main.duration);

        // Destroy the shell.
        Destroy(gameObject);
    }

    private int CalculateDamage(Vector3 targetPosition)
    {
        // Create a vector from the shell to the target.
        Vector3 explosionToTarget = targetPosition - transform.position;

        // Calculate the distance from the shell to the target.
        float explosionDistance = explosionToTarget.magnitude;

        // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
        float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;

        // Calculate damage as this proportion of the maximum possible damage.
        float damage = relativeDistance * maxDamage;

        // Make sure that the minimum damage is always 0.
        damage = Mathf.Max(0f, damage);

        return (int)damage;
    }
}
