using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainSelect : MonoBehaviour
{

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] public Animator transition;
    [SerializeField] private GameObject Continue;
    [SerializeField] private GameObject blockContinue;

    public float transitionTime = 1f;    
    // Start is called before the first frame update
    void Start()
    {
        string str = PlayerPrefs.GetString("Level", null);
        if(string.IsNullOrEmpty(str) == true)
        { 
            Continue.SetActive(false);
        }
        else
        {
            blockContinue.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator SelectFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[1].gameObject);
    }    

    public void Mulai()
    {
        StartCoroutine(LoadLevel());
        PlayerPrefs.DeleteAll();
    }

    public IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("start");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        StartCoroutine(ContLevel());
    }

    public IEnumerator ContLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(PlayerPrefs.GetString("Level"));
    }
}
