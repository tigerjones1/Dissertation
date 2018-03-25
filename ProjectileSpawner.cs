using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {

    public Transform Destination;
    public PathedProjectile Projectile;

    public GameObject SpawnEffect;
    public float Speed;
    public float FireRate;

    private float nextShotInSeconds;

	// Use this for initialization
	public void Start () {
        nextShotInSeconds = FireRate;
	}
	
	// Update is called once per frame
	public void Update () {
	    if((nextShotInSeconds -= Time.deltaTime) > 0)
            return;

        nextShotInSeconds = FireRate;
        var projectile = (PathedProjectile)Instantiate(Projectile, transform.position, transform.rotation);
        projectile.Initialise(Destination, Speed);

        if(SpawnEffect != null)
            Instantiate(SpawnEffect, transform.position, transform.rotation);
	}

    public void OnDrawGizmos()
    {
        if (Destination == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
