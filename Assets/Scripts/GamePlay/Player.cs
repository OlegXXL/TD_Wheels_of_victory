using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public int maxHP = 100;   
    private int currentHP;     
    public float moveSpeed = 5f;      
    public float swordDamage = 20f;   
    public float attackSpeed = 1f;      
    public Transform swordTransform; 
    public Joystick joystick;    
    private bool isSwordEquipped = true;  
    private float nextAttackTime = 0f; 
    public float swordAttackRange = 1.5f;
    public float bowAttackRange = 2000f; // Increased range for bow
    public LayerMask enemyLayer;
    public int lives = 3; 
    public Image weaponSet;
    // Adjusted player borders
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    private SpriteRenderer spriteRenderer;
    public SpriteRenderer weaponSpriteRenderer; 

    public GameObject arrowPrefab; 
    public Transform arrowSpawnPoint; 
    public float arrowDamage = 15f;

    public ItemDataList itemDataList; 

    // References for UI elements
    public Slider hpSlider;
    public Image[] blueHearts;

    private float nextDamageTime = 0f; // Timer for controlling damage frequency

    void Start()
    {
        currentHP = maxHP;   
        enemyLayer = LayerMask.GetMask("Default"); 
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateWeapon();
        UpdateHPSlider();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        float moveX = joystick.Horizontal; 
        float moveY = joystick.Vertical;   

        Vector3 movement = new Vector3(moveX, moveY, 0f) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;

        if (moveX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (moveX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void HandleAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (isSwordEquipped)
            {
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, swordAttackRange, enemyLayer);
                foreach (Collider2D enemy in hitEnemies)
                {
                    SwordAttack(enemy.gameObject);
                }
            }
            else
            {
                BowAttack();
            }
            nextAttackTime = Time.time + 1f / attackSpeed;
        }
    }

    void SwordAttack(GameObject enemy)
    {
        Vector3 originalPosition = swordTransform.localPosition;
        Vector3 originalRotation = swordTransform.localEulerAngles;

        Sequence swordSequence = DOTween.Sequence();
        swordSequence.Append(swordTransform.DOLocalMove(new Vector3(0.5f, 0, 0), 0.1f)) // Move sword forward
                     .Join(swordTransform.DOLocalRotate(new Vector3(0, 0, -45), 0.1f)) // Rotate sword
                     .Append(swordTransform.DOLocalMove(new Vector3(-0.5f, 0, 0), 0.1f)) // Move sword backward
                     .Join(swordTransform.DOLocalRotate(new Vector3(0, 0, 45), 0.1f)) // Rotate sword back
                     .Append(swordTransform.DOLocalMove(originalPosition, 0.1f)) // Smoothly reset sword position
                     .Join(swordTransform.DOLocalRotate(originalRotation, 0.1f)); // Smoothly reset sword rotation

        // Reduce enemy health
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.TakeDamage((int)swordDamage);

            // Check if the enemy is dead and add coins
            if (enemyScript.IsDead())
            {
                LevelManager.Instance.AddCoins(enemyScript.coinValue);
            }
        }
    }

    void BowAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= bowAttackRange)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.SetDamage(arrowDamage);
                Vector3 directionToEnemy = nearestEnemy.transform.position - arrowSpawnPoint.position;
                arrowScript.SetTargetDirection(directionToEnemy);
            }
        }
    }

    public void SetWeapon(bool swordEquipped)
    {
        isSwordEquipped = swordEquipped;
        Debug.Log(isSwordEquipped ? "Sword equipped" : "Bow equipped");
        UpdateWeapon();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHPSlider();
        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            lives--;
            UpdateBlueHearts();
            Debug.Log("Player lives: " + lives);
            if (lives > 0)
            {
                currentHP = maxHP; 
                UpdateHPSlider();
            }
            else
            {
                currentHP = 0; // Ensure HP is zero when lives are exhausted
                UpdateHPSlider();
            }
        }
    }

    void Die()
    {
        Debug.Log("Player died. Game Over.");
        LevelManager.Instance.ResetGame();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, isSwordEquipped ? swordAttackRange : bowAttackRange);
    }

    void UpdateWeapon()
    {
        foreach (ItemData item in itemDataList.items)
        {
            if (item.isSelected)
            {
                isSwordEquipped = !item.isBow;
                weaponSpriteRenderer.sprite = item.weaponSprite; // Update weapon sprite
                weaponSet.sprite = item.weaponSprite; // Update weapon sprite
                Debug.Log(isSwordEquipped ? "Sword equipped" : "Bow equipped");
                break;
            }
        }
    }

    void UpdateHPSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHP / maxHP;
        }
    }

    void UpdateBlueHearts()
    {
        for (int i = 0; i < blueHearts.Length; i++)
        {
            if (i < lives)
            {
                blueHearts[i].enabled = true;
            }
            else
            {
                blueHearts[i].enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && Time.time >= nextDamageTime)
            {
                TakeDamage(enemy.damage);
                nextDamageTime = Time.time + 0.5f; // Set the next damage time
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && Time.time >= nextDamageTime)
            {
                TakeDamage(enemy.damage);
                nextDamageTime = Time.time + 0.5f; // Set the next damage time
            }
        }
    }
}