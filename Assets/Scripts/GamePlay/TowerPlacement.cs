using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public GameObject magePrefab;
    public GameObject knightPrefab;
    public Button mageButton;
    public Button knightButton;
    public GameObject selectionPanel;
    public Transform towerSpot;
    public SpriteRenderer towerPlacementSprite;

    private GameObject currentTower;
    private int towerCost = 100; // Initial cost

    void Start()
    {
        selectionPanel.SetActive(false);
        mageButton.onClick.AddListener(() => PlaceTower(magePrefab));
        knightButton.onClick.AddListener(() => PlaceTower(knightPrefab));
    }

    void OnMouseDown()
    {
        if (currentTower == null)
        {
            selectionPanel.SetActive(!selectionPanel.activeSelf);
        }
        else
        {
            selectionPanel.SetActive(false);
        }
    }

    void PlaceTower(GameObject towerPrefab)
    {
        if (currentTower == null && LevelManager.Instance.SpendCoins(towerCost))
        {
            currentTower = Instantiate(towerPrefab, towerSpot.position, Quaternion.identity);
            selectionPanel.SetActive(false);
            towerPlacementSprite.enabled = false;
            towerCost *= 2; // Double the cost for the next tower
        }
        else
        {
            Debug.Log("Not enough coins to place tower!");
        }
    }
}