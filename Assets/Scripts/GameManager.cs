using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Canvas MainMenu;
    RandomSound sound;
    AudioSource music;
    static FloorTileController[] floorTiles;

    void Awake()
    {
        sound = GetComponent<RandomSound>();
        music = GameObject.Find("Audio").GetComponents<AudioSource>()[1];
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
        sound.Play();
        music.volume = 0.5f;
        MainMenu.enabled = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++) Destroy(enemies[i]);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.Find("Spawns"));
        PlayerPrefs.Save();
    }
}
