using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AutoLoc : MonoBehaviour
{

    [Header("Scene")]
    [SerializeField] private string LocName;
    [SerializeField] public Animator transition;

    private bool playerInRange;
    public float transitionTime = 1f;
    public GameObject sound;

    private void Awake() 
    {
        playerInRange = false;
        sound.SetActive(false);
    }

    private void Update() 
    {
        if (PhoneManager.GetInstance().phoneIsActive)
        {
            return;
        }
        if (playerInRange && !PhoneManager.GetInstance().phoneIsActive) 
        {

            LoadNextArea();
                
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
