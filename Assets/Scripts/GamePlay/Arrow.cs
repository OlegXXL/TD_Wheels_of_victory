using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;  
    private float damage = 20;   
    private Vector3 targetDirection; 

    void Update()
    {
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetTargetDirection(Vector3 direction)
    {
        targetDirection = direction.normalized;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))  
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)damage);
            }
            Destroy(gameObject);  
        }
    }
}