using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public int roundCounter = 0;

    private AudioSource menuMusic;
    private AudioSource level1Music;
    private AudioSource level2Music;
    private AudioSource level3Music;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        menuMusic = transform.Find("MenuMusic").GetComponent<AudioSource>();
        level1Music = transform.Find("Level1Music").GetComponent<AudioSource>();
        level2Music = transform.Find("Level2Music").GetComponent<AudioSource>();
        level3Music = transform.Find("Level3Music").GetComponent<AudioSource>();
        menuMusic.enabled = true;
        level1Music.enabled = false;
        level2Music.enabled = false;
        level3Music.enabled = false;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        
    }

    public void NextRound()
    {
        if (roundCounter >= 3) SceneManager.LoadScene("Win Scene");
        else
        {
            roundCounter += 1;
            SceneManager.LoadScene("Game Scene");
            switch (roundCounter)
            {
                case 1:
                    menuMusic.enabled = false;
                    level2Music.enabled = false;
                    level3Music.enabled = false;
                    level1Music.enabled = true;
                    level1Music.Play();
                    break;
                case 2:
                    menuMusic.enabled = false;
                    level1Music.enabled = false;
                    level3Music.enabled = false;
                    level2Music.enabled = true;
                    level2Music.Play();
                    break;
                case 3:
                    menuMusic.enabled = false;
                    level1Music.enabled = false;
                    level2Music.enabled = false;
                    level3Music.enabled = true;
                    level3Music.Play();
                    break;
                default:
                    menuMusic.enabled = true;
                    level1Music.enabled = false;
                    level2Music.enabled = false;
                    level3Music.enabled = false;
                    menuMusic.Play();
                    break;
            }
        }
    }

    public void TryRound()
    {
        SceneManager.LoadScene("Game Scene");
        switch (roundCounter)
        {
            case 1:
                menuMusic.enabled = false;
                level2Music.enabled = false;
                level3Music.enabled = false;
                level1Music.enabled = true;
                level1Music.Play();
                break;
            case 2:
                menuMusic.enabled = false;
                level1Music.enabled = false;
                level3Music.enabled = false;
                level2Music.enabled = true;
                level2Music.Play();
                break;
            case 3:
                menuMusic.enabled = false;
                level1Music.enabled = false;
                level2Music.enabled = false;
                level3Music.enabled = true;
                level3Music.Play();
                break;
            default:
                menuMusic.enabled = true;
                level1Music.enabled = false;
                level2Music.enabled = false;
                level3Music.enabled = false;
                menuMusic.Play();
                break;
        }
    }

    public void ResetRounds()
    {
        menuMusic.enabled = true;
        level1Music.enabled = false;
        level2Music.enabled = false;
        level3Music.enabled = false;
        roundCounter = 0;
        SceneManager.LoadScene("Main Menu Scene");
    }
}