using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public int roundCounter = 0;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        
    }

    public void NextRound()
    {
        roundCounter += 1;
        SceneManager.LoadScene("Game Scene");
    }

    public void ResetRounds()
    {
        roundCounter = 0;
        SceneManager.LoadScene("Main Menu Scene");
    }
}