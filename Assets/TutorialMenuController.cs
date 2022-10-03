using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToGameScene()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        objs[0].GetComponent<DontDestroy>().NextRound();
    }

    public void GoToMainMenuScene()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("DontDestroy");
        objs[0].GetComponent<DontDestroy>().ResetRounds();
    }
}
