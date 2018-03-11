using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialConditionBeforeWinning : MonoBehaviour {

    public int currentTutorial;

    public bool checkEnemiesDead;
    public bool checkTilesHealed;

    TutorialManager tutorial;
    EnemyHealth[] enemies;
    FloorTileController[] tiles;
    GameObject endPosition;
    bool canWin;

    private void Awake()
    {
        tutorial = GameObject.Find("GameManager").GetComponent<TutorialManager>();
        endPosition = tutorial.layouts[currentTutorial - 1].GetComponentInChildren<TutorialWinOnTrigger>().gameObject;
    }

    private void OnEnable()
    {
        NewTutorial();
        StartCoroutine(Check());
    }

    private void OnDisable()
    {
        canWin = false;
        StopAllCoroutines();
    }

    private void NewTutorial()
    {
        endPosition.SetActive(false);
        enemies = tutorial.layouts[currentTutorial - 1].GetComponentsInChildren<EnemyHealth>();
        tiles = GameObject.Find("FloorTiles").GetComponentsInChildren<FloorTileController>();
    }

    IEnumerator Check()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        yield return null; // XXX: hack for waiting the tutorial to finish load (including resurrecting enemies and tiles)
        while (true)
        {
            canWin = true;
            if (checkEnemiesDead) canWin &= Array.TrueForAll(enemies, e => e.dead);
            if (checkTilesHealed) canWin &= Array.TrueForAll(tiles, t => !t.isDamaged);

            if (canWin)
            {
                endPosition.SetActive(true);
                break;
            }
            else yield return wait;


        }
    }
}
