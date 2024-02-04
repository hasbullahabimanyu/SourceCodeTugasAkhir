using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public Animator cutscene; 
    public string cutsceneName;
    public string sceneName;
    public bool clear;

    public float transitionTime = 11f;
    // Start is called before the first frame update
    void Start()
    {
        cutscene.Play(cutsceneName);
        LoadNextScene();
        if(clear)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadLevel());
    }

    public IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
