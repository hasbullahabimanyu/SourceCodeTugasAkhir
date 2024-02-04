using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LocTrigger : MonoBehaviour
{
    [Header("visualcue")]
    [SerializeField] private GameObject visualCue;

    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Scene")]
    [SerializeField] private string LocName;
    [SerializeField] public Animator transition;

    private bool playerInRange;
    
    public GameObject sound;

    private static LocTrigger instance;

    public float transitionTime = 1f;

    private void Awake() 
    {
        playerInRange = false;
        visualCue.SetActive(false);
        sound.SetActive(false);
    }

    public static LocTrigger GetInstance()
    {
        return instance;
    }

    private void Update() 
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
                
        if (PhoneManager.GetInstance().phoneIsActive)
        {
            return;
        }
        if (playerInRange && !PhoneManager.GetInstance().phoneIsActive) 
        {
            visualCue.SetActive(true);
                if (InputManager.GetInstance().GetInteractPressed()) 
                {
                    LoadNextArea();
                }
        }
        else 
        {
            visualCue.SetActive(false);
        }
    }

    public void LoadNextArea()
    {
        StartCoroutine(LoadLevel(LocName));
        sound.SetActive(true);
    }

    public IEnumerator LoadLevel(string areaName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(areaName);
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
