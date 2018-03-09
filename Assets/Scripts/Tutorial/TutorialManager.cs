using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    int TOTAL_TUTORIALS = 2;

    [Header("Objects")]
    public PlayerMovement player;
    public GameObject pathBar;
    public GameObject overdriveBar;
    public GameObject jumpButton;
    public GameObject attackButton;
    public GameObject attackCooldownBar;
    public GameObject stopPathButton;

    [Header("Tutorial Layouts")]
    public GameObject[] layouts;

    int currentTutorial = 1;

    float originalCooldownBPS;
    float originalRangeBPS;
    float originalOverdriveBPS;

    private void Awake()
    {
        if (layouts.Length != TOTAL_TUTORIALS) Debug.LogError("Different amount of tutorials and layouts");
        originalCooldownBPS = player.cooldownBPS;
        originalRangeBPS = player.rangeBPS;
        originalOverdriveBPS = player.overdriveBPS;
        player.cooldownBPS = 0f;
        player.rangeBPS = 0f;
        player.overdriveBPS = 0f;
    }

    private void Start()
    {
        LoadTutorial(currentTutorial);
    }

    public void PreviousTutorial()
    {
        CleanTutorial(currentTutorial);
        currentTutorial = Mathf.Clamp(currentTutorial - 1, 1, TOTAL_TUTORIALS);
        LoadTutorial(currentTutorial);
    }

    public void NextTutorial()
    {
        CleanTutorial(currentTutorial);
        currentTutorial = Mathf.Clamp(currentTutorial + 1, 1, TOTAL_TUTORIALS);
        LoadTutorial(currentTutorial);
    }

    public void RestartTutorial()
    {
        CleanTutorial(currentTutorial);
        LoadTutorial(currentTutorial);
    }

    void CleanTutorial(int tutorial)
    {
        switch (tutorial)
        {
            case 0: break;
            case 1:
                Tutorial1(true);
                break;
            case 2:
                Tutorial2(true);
                break;
            default:
                Debug.LogError("Tutorial " + tutorial + " can't be cleaned because it doesn't exists");
                break;
        }
    }

    void LoadTutorial(int tutorial)
    {
        switch (tutorial)
        {
            case 1:
                Tutorial1();
                break;
            case 2:
                Tutorial2();
                break;
            default:
                Debug.LogError("Tutorial " + tutorial + " doesn't exists");
                break;
        }
    }

    void Tutorial1(bool clean = false)
    {
        player.transform.position = layouts[0].transform.Find("StartPosition").position;
        player.ResetPath();
        jumpButton.SetActive(clean);
        attackButton.SetActive(clean);
        attackCooldownBar.SetActive(clean);
        overdriveBar.SetActive(clean);
        layouts[0].SetActive(!clean);
    }

    void Tutorial2(bool clean = false)
    {
        player.transform.position = layouts[1].transform.Find("StartPosition").position;
        player.ResetPath();
        attackButton.SetActive(clean);
        attackCooldownBar.SetActive(clean);
        overdriveBar.SetActive(clean);
        layouts[1].SetActive(!clean);
    }
}
