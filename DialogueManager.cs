using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    [Header("Params")]
    private float typingSpeed = 0.06f;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject portraitPanel;
    [SerializeField] private GameObject shadowPanel;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private GameObject phoneIcon;
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private TextMeshProUGUI pointIndicator;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Animator shadowAnimator;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private Animator personAnimator;
    [SerializeField] private Animator guideAnimator;
    [SerializeField] private TextMeshProUGUI blockedchoicesText;
    [SerializeField] private TextMeshProUGUI blockedpointIndicator;
    [SerializeField] public Animator transition;
    [SerializeField] private GameObject contactPanel;
    private Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject blockChoice;
    private TextMeshProUGUI[] choicesText;

    [Header("Audio")]
    [SerializeField] private AudioClip[] audioClip;
    [SerializeField] private AudioSource audioSource;
    

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    public bool gameOver { get; private set; }
    public bool flashback = false;
    private int healthpoint;

    private bool canContinueToNextLine = false;
    private int thresholdVal;
    private Coroutine displayLineCoroutine;
    public float transitionTime = 1f;
    public string guidename;
    public bool guide = false;

    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string HEALTH_TAG = "health";
    private const string THRESHOLD_TAG = "threshold";
    private const string AUDIO_TAG = "audio";
    private const string TEXT_TAG = "text";
    private const string PERSON_TAG = "person";
    private const string GUIDE_TAG = "guide";
    private const string SCENE_TAG = "scene";
    private const string CONTACT_TAG = "contact";
    private const string PERSONC_TAG = "personc";

    public static DialogueVariables dialogueVariables;

    private void Awake() 
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public static DialogueManager GetInstance() 
    {
        return instance;
    }

    private void Start() 
    {
        
        gameOver = false;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        blackPanel.SetActive(false);
        portraitPanel.SetActive(false);
        shadowPanel.SetActive(false);
        namePanel.SetActive(false);
        choicePanel.SetActive(false);


                // get all of the choices text 
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) 
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

    }

    private void Update() 
    {
        // return right away if dialogue isn't playing


        if (!dialogueIsPlaying) 
        {
            return;
        }     

        // handle continuing to the next line in the dialogue when submit is pressed
        // NOTE: The 'currentStory.currentChoiecs.Count == 0' part was to fix a bug after the Youtube video was made
        if (canContinueToNextLine && InputManager.GetInstance().GetInteractPressed() && HealthManager.GetInstance().notgameOver && currentStory.currentChoices.Count == 0 )
        {
            dialoguePanel.SetActive(true);
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON) 
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        blackPanel.SetActive(true);
        namePanel.SetActive(true);
        portraitPanel.SetActive(true);
        shadowPanel.SetActive(true);
        phoneIcon.SetActive(false);
        objectivePanel.SetActive(false);

        dialogueVariables.StartListening(currentStory);

        ContinueStory(); 

    }

    public void ExitDialogueMode() 
    { 
        dialogueVariables.StopListening(currentStory);
        dialogueVariables.SaveVariables();
        
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        blackPanel.SetActive(false);
        choicePanel.SetActive(false);
        portraitPanel.SetActive(false);
        namePanel.SetActive(false);
        shadowPanel.SetActive(false);
        phoneIcon.SetActive(true);
        objectivePanel.SetActive(true);
        dialogueText.text = "";
        if (guide)
        {
            guideAnimator.Play(guidename);
        }
    }

    public void StopDialogue()
    {
        dialogueIsPlaying = false;
    }

    private void ContinueStory() 
    {
        dialoguePanel.SetActive(true);       
        if (currentStory.canContinue)
        {
            choicePanel.SetActive(true);
            dialoguePanel.SetActive(true);
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
            DisplayChoices();
        }
        else
        {
            ExitDialogueMode();
        } 
    }

     private IEnumerator DisplayLine(string line) 
    {
        // set the text to the full line, but set the visible characters to 0
        dialogueText.text = "";
        //dialogueText.maxVisibleCharacters = 0;
        // hide items while text is typing
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        //bool isAddingRichTextTag = false;

        // display each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            if (InputManager.GetInstance().GetInteractPressed()) 
            {
                dialogueText.text = line;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        continueIcon.SetActive(true);
        canContinueToNextLine = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags) 
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) 
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            
            // handle the tag
            switch (tagKey) 
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    shadowAnimator.Play(tagValue);
                    break;
                case HEALTH_TAG:
                    healthpoint = int.Parse(tagValue);
                    HealthManager.GetInstance().GetSetHealth(healthpoint);
                    break;
                case THRESHOLD_TAG:
                    thresholdVal = int.Parse(tagValue);
                    Debug.Log(thresholdVal);
                    break;
                case TEXT_TAG:
                    textAnimator.Play(tagValue);
                    break;
                case PERSON_TAG:
                    personAnimator.SetBool(tagValue, true);
                    break; 
                case GUIDE_TAG:
                    guideAnimator.Play(tagValue);
                    break; 
                case SCENE_TAG:
                    LocationChange(tagValue);
                    break;
                case CONTACT_TAG:
                    if(tagValue == "true")
                    {
                        contactPanel.SetActive(true);
                    }
                    break; 
                case AUDIO_TAG:  
                    audioSource.PlayOneShot(audioClip[int.Parse(tagValue)]);
                    break; 
                case PERSONC_TAG:
                    personAnimator.SetBool(tagValue, false);
                    break;                                                                     
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private void HideChoices() 
    {
        foreach (GameObject choiceButton in choices) 
        {
            choiceButton.SetActive(false);
        }
    }    


    private void DisplayChoices() 
    {

        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to make sure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " 
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue


        foreach(Choice choice in currentChoices) 
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            if (thresholdVal > 0)
            {
                pointIndicator.text = HealthManager.currentHealth + "/" + thresholdVal;
            }
            index++;
        }

        if (thresholdVal > HealthManager.currentHealth)
        {
            if (flashback)
            {
                choices[1].gameObject.SetActive(false);
                blockChoice.gameObject.SetActive(true);
                blockedchoicesText.text = choicesText[1].text;
                blockedpointIndicator.text = HealthManager.currentHealth + "/" + thresholdVal;
            }
            else
            {
                choices[2].gameObject.SetActive(false);
                blockChoice.gameObject.SetActive(true);
                blockedchoicesText.text = choicesText[2].text;
                blockedpointIndicator.text = HealthManager.currentHealth + "/" + thresholdVal;
            }
        }

        if (index > 0)
        {
            dialoguePanel.SetActive(false);
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++) 
        {
            choices[i].gameObject.SetActive(false);
            blockChoice.gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {

        if (canContinueToNextLine)
        {
        currentStory.ChooseChoiceIndex(choiceIndex); 
        InputManager.GetInstance().GetInteractPressed(); // this is specific to my InputManager script        
        ContinueStory();    
        }
    }

    public void LocationChange(string LocName)
    {
        StartCoroutine(LoadLevel(LocName));
    }

    public IEnumerator LoadLevel(string areaName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(areaName);
    }



        //InputManager.GetInstance().RegisterSubmitPressed(); // this is specific to my InputManager script        
       // ContinueStory();

    //public void OnApplicationQuit()
    //{
        //if (dialogueVariables != null)
        //{
            //dialogueVariables.SaveVariables();
        //}
    //}
}
