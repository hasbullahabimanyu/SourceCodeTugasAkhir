using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LocChoices : MonoBehaviour
{
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;

    [SerializeField] private GameObject choicePanel;

    private bool playerInRange;

    [SerializeField] public Animator transition;

    public float transitionTime = 1f;

    private void Awake() 
    {
        playerInRange = false;
        choicePanel.SetActive(false);
    }
    // Start is called before the first frame update
    void Update()
    {
        if (PhoneManager.GetInstance().phoneIsActive)
        {
            return;
        }
        if (playerInRange && !PhoneManager.GetInstance().phoneIsActive) 
        {
            choicePanel.SetActive(true);
            StartCoroutine(SelectFirstChoice());
        }
        else 
        {
            choicePanel.SetActive(false);
        }
    }

    public void LoadNextArea(string LocName)
    {
        StartCoroutine(LoadLevel(LocName));
    }

    public IEnumerator LoadLevel(string areaName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(areaName);
    }

    private IEnumerator SelectFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }   

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }     

    
}
