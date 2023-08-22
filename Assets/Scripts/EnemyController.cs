// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathEffect;
    [SerializeField] private float minIdleTime = 5.0f;
    [SerializeField] private float maxIdleTime = 15.0f;
    [SerializeField] private GameObject enemyProjectilePrefab;

    private MeshRenderer _renderer;
    private float _nextFireTime;

    private void Awake()
    {
        this._renderer = gameObject.GetComponent<MeshRenderer>();
        _nextFireTime = Time.time + Random.Range(minIdleTime, maxIdleTime);
    }

    private void Update()
    {
        TryFireProjectile();
    }

    // This method listens to HealthManager "onHealthChanged" events. The actual
    // event listening is set up within the editor interface. This is purely for
    // visuals currently, and takes a fractional value between 0 and 1.
    public void UpdateHealth(float frac)
    {
        this._renderer.material.color = Color.red * frac;
    }

    // Same as above, but listens to onDeath events.
    public void Kill()
    {
        var particles = Instantiate(this.deathEffect);
        particles.transform.position = transform.position;
    }

    // Try to fire a projectile
    private void TryFireProjectile()
    {
        if (Time.time >= _nextFireTime)
        {
            FireProjectile();
            _nextFireTime = Time.time + Random.Range(minIdleTime, maxIdleTime);
        }
    }

    // Fire a projectile
    private void FireProjectile()
    {
        Vector3 playerPosition = FindObjectOfType<PlayerController>().GetPlayerPosition();
        Vector3 directionToPlayer = (playerPosition - transform.position).normalized;

        var projectile = Instantiate(enemyProjectilePrefab);
        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileController>().velocity = 25*directionToPlayer;
    }
}
