using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static FloorTileController[] floorTiles;

    void Awake()
    {

        Transform tiles = GameObject.Find("FloorTiles").transform;
        floorTiles = new FloorTileController[tiles.childCount];
        for (int i = 0; i < floorTiles.Length; i++)
        {
            floorTiles[i] = tiles.GetChild(i).GetComponent<FloorTileController>();
        }
        StartCoroutine(CheckForGameOver());
    }


    IEnumerator CheckForGameOver()
    {
        yield return new WaitUntil(() => Array.TrueForAll(floorTiles, tile => tile.isDamaged));
        GameOver();
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
