using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueStarter : MonoBehaviour
{
    public float time = 0.5f;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    void Start()
    {
       StartCoroutine(StartDialogue()); 
    }

    void Update()
    {

    }

    public IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(time);
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
    }
}
