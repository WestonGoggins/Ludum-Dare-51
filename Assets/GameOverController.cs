using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TryAgain()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        objs[0].GetComponent<DontDestroy>().TryRound();
    }

    public void GoToMainMenu()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        if (objs.Length > 0) objs[0].GetComponent<DontDestroy>().ResetRounds();
        else SceneManager.LoadScene("Main Menu Scene");
    }
}
