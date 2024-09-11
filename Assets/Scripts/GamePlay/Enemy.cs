using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int maxHP = 5;
    public int damage = 10;
    private int currentHP;
    private int currentWaypointIndex = 0;
    public Transform[] waypoints;
    public int coinValue = 50;

    public delegate void EnemyKilled();
    public event EnemyKilled OnEnemyKilled;

    void Start()
    {
        currentHP = maxHP;
    }

    void Update()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            MoveToNextWaypoint();
        }
        else
        {
            if (MyGameManager.Instance != null)
            {
                MyGameManager.Instance.ReduceGameHeart();
            }
            Destroy(gameObject);
        }
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    void MoveToNextWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypointIndex++;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (OnEnemyKilled != null)
        {
            OnEnemyKilled();
        }
        Destroy(gameObject);
        LevelManager.Instance.AddCoins(coinValue); // Add coins to the player's total
    }
}