using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    int TOTAL_TUTORIALS = 3;

    [Header("Objects")]
    public Text tutorialTitle;
    public TutorialDialog dialog;
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

    float originalMaxPathDistance;

    private void Awake()
    {
        if (layouts.Length != TOTAL_TUTORIALS) Debug.LogError("Different amount of tutorials and layouts");
        originalCooldownBPS = player.cooldownBPS;
        originalRangeBPS = player.rangeBPS;
        originalOverdriveBPS = player.overdriveBPS;
        originalMaxPathDistance = player.maxPathDistance;

        player.cooldownBPS = 0f; // By default this stats are deactivated, should be activated on tutorial method, and reseted to 0
        player.rangeBPS = 0f;
        player.overdriveBPS = 0f;
    }

    private void Start()
    {
        LoadTutorial(currentTutorial);
    }

    public Vector3 StartPosition()
    {
        return layouts[currentTutorial - 1].transform.Find("StartPosition").position;
    }

    public Vector3 EndPosition()
    {
        return layouts[currentTutorial - 1].transform.Find("EndPosition").position;
    }

    public void ExitTutorial()
    {
        SceneManager.LoadScene("MainMenu");
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
        FloorTileController[] tiles = GameObject.Find("FloorTiles").transform.GetComponentsInChildren<FloorTileController>();
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].Heal();
        }
        switch (tutorial)
        {
            case 0: break;
            case 1:
                Tutorial1(true);
                break;
            case 2:
                Tutorial2(true);
                break;
            case 3:
                Tutorial3(true);
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
            case 3:
                Tutorial3();
                break;
            default:
                Debug.LogError("Tutorial " + tutorial + " doesn't exists");
                break;
        }
    }

    void Tutorial1(bool clean = false)
    {
        dialog.SetSlides(new string[] {
            "You're a monk and this is your garden",
            "You must keep it intact",
            "Let's learn the controls for that",
            "Draw your way to the objective",
            "You can draw until the top pink bar depletes",
            "Avoid damaging your garden by falling off",
            "You can skip through the tutorials with the top controls"
        });
        tutorialTitle.text = "1/" + TOTAL_TUTORIALS + " RUNNING";
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
        dialog.SetSlides(new string[] {
            "You can jump with the jump button in the bottom left corner",
            "Try to reach your objective by (double) jumping while running",
            "Avoid at all costs damaging your garden"
        });
        tutorialTitle.text = "2/" + TOTAL_TUTORIALS + " JUMPING";
        player.transform.position = layouts[1].transform.Find("StartPosition").position;
        player.ResetPath();
        attackButton.SetActive(clean);
        attackCooldownBar.SetActive(clean);
        overdriveBar.SetActive(clean);
        layouts[1].SetActive(!clean);
        player.maxPathDistance = clean ? originalMaxPathDistance : 50;
    }

    void Tutorial3(bool clean = false)
    {
        dialog.SetSlides(new string[] {
            "This frogs are trying to destroy your garden",
            "If they destroy all of your garden tiles, you loose",
            "You can attack them with the bottom right button",
            "When you attack, you also cure the garden tile below you",
            "Every time you kill a frog, your pink bar increases",
        });
        tutorialTitle.text = "3/" + TOTAL_TUTORIALS + " ATTACKING";
        player.transform.position = layouts[2].transform.Find("StartPosition").position;
        player.ResetPath();
        overdriveBar.SetActive(clean);
        layouts[2].SetActive(!clean);

        if (!clean) // Resurrect enemies
        {
            EnemyHealth[] enemiesHealth = layouts[2].GetComponentsInChildren<EnemyHealth>();
            TutorialEnemy[] enemiesMovement = layouts[2].GetComponentsInChildren<TutorialEnemy>();
            for (int i = 0; i < enemiesHealth.Length; i++)
            {
                enemiesHealth[i].dead = false;
                enemiesMovement[i].Continue();
            }
        }
        //Destroy some tiles
        FloorTileController[] tiles = GameObject.Find("FloorTiles").transform.GetComponentsInChildren<FloorTileController>();
        for (int i = 1; i <= 5; i++) {
            tiles[i].Damage();
        }
    }
}
