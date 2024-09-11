using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelChoose : MonoBehaviour
{
    public Button[] levelButtons; 
    public Button playButton; 
    public GameObject toBeContinuedPanel; 
    public float selectedScale = 1.5f; // Scale factor for the selected button
    public float animationDuration = 0.5f; // Duration of the scaling animation

    private int selectedLevelIndex = 0; 
    private Vector3[] originalScales; // Store the original scales of the buttons

    void Start()
    {
        // Initialize the original scales array
        originalScales = new Vector3[levelButtons.Length];
        for (int i = 0; i < levelButtons.Length; i++)
        {
            originalScales[i] = levelButtons[i].transform.localScale;
            int index = i; 
            levelButtons[i].onClick.AddListener(() => OnLevelButtonClick(index));
        }

        playButton.onClick.AddListener(OnPlayButtonClick);

        HighlightSelectedLevel();
    }

    void OnLevelButtonClick(int index)
    {
        selectedLevelIndex = index;
        HighlightSelectedLevel();
    }

    void HighlightSelectedLevel()
    {
        // Reset all buttons to their original scale with animation
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].transform.DOScale(originalScales[i], animationDuration);
        }
        
        // Set the selected button to the selected scale with animation
        levelButtons[selectedLevelIndex].transform.DOScale(originalScales[selectedLevelIndex] * selectedScale, animationDuration);
    }

    void OnPlayButtonClick()
    {
        switch (selectedLevelIndex)
        {
            case 0:
                SceneManager.LoadScene("Level1");
                break;
            case 1:
                SceneManager.LoadScene("Level2");
                break;
            case 2:
                SceneManager.LoadScene("Level3");
                break;
            case 3:
                SceneManager.LoadScene("Level4");
                break;
            case 4:
                toBeContinuedPanel.SetActive(true);
                break;
        }
    }
}