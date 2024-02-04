using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CharSelect : MonoBehaviour
{

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] public Animator transition;

    public float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
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
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }    

    public void ChooseCharacter(string charName)
    {
        LoadNextArea(charName);
    }

    public void LoadNextArea(string charName)
    {
        StartCoroutine(LoadLevel(charName));
    }

    public IEnumerator LoadLevel(string areaName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(areaName);
    }
}
