using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1.0f;
    public float range = 5.0f;

    private float fireCountdown = 0.0f;
    private Transform target;

    void Update()
    {
        FindTarget();

        if (target != null)
        {
            if (fireCountdown <= 0.0f)
            {
                Shoot();
                fireCountdown = 1.0f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }
}