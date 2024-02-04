using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    public bool phoneIsActive { get; private set; }
    public bool musicIsActive { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject blackPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private Animator phoneAnimator;
    [SerializeField] private GameObject headerPanel;
    [SerializeField] private GameObject appPanel;
    [SerializeField] private GameObject locPanel;
    [SerializeField] private GameObject mapPanel;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject contactPanel;
    [SerializeField] private GameObject guidePanel;
    [SerializeField] private GameObject phoneIcon;
    [SerializeField] private GameObject soundCheck;
    [SerializeField] private TextMeshProUGUI appName;
    [SerializeField] public Animator transition;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private GameObject[] locchoices;
    [SerializeField] private GameObject[] contchoices;
    [SerializeField] private GameObject[] homechoices;
    [SerializeField] private GameObject soundToggle;
    private TextMeshProUGUI[] choicesText;
    
    public float transitionTime = 1f;
    private static PhoneManager instance;
    private string contactName;
    public bool guide = false;
    private string mustatus;
    public Toggle musictoggle;
    [SerializeField] private AudioClip sound;
    [SerializeField] private AudioSource source;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Phone Manager in the scene.");
        }
        instance = this;
        source = this.gameObject.AddComponent<AudioSource>();
        mustatus = PlayerPrefs.GetString("Music");
    }    

    public static PhoneManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        blackPanel.SetActive(false);
        settingPanel.SetActive(true);
        phoneIsActive = false;
        headerPanel.SetActive(false);
        phoneAnimator.SetBool("phone", false);
        appPanel.SetActive(false);
        locPanel.SetActive(false);
        soundCheck.SetActive(false);
        mapPanel.SetActive(false);
        chatPanel.SetActive(false);
        homePanel.SetActive(false);
        contactPanel.SetActive(false);
        phoneIcon.SetActive(true);

        mustatus = PlayerPrefs.GetString("Music");
        Debug.Log(mustatus);

        if(mustatus == "on")
        {
            musicIsActive = true;
            musictoggle.GetComponent<Toggle>().isOn = false;
        }

        if(mustatus == "off")
        {
            musicIsActive = false;
            musictoggle.GetComponent<Toggle>().isOn = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if (ChatManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        if (DialogueManager.GetInstance().gameOver)
        {
            return;
        }

        if(!phoneIsActive)
        {
            if(InputManager.GetInstance().GetJumpPressed())
            {
                if(guide)
                {
                    guidePanel.SetActive(false);
                }
                phoneIsActive = true;
                blackPanel.SetActive(true);
                settingPanel.SetActive(false);
                phoneAnimator.SetBool("phone", true);
                appPanel.SetActive(true);
                headerPanel.SetActive(false);
                soundCheck.SetActive(false);
                locPanel.SetActive(false);
                chatPanel.SetActive(false);
                homePanel.SetActive(false);
                contactPanel.SetActive(false);
                phoneIcon.SetActive(false);
                appName.text = "";
                

                StartCoroutine(SelectFirstChoice());
            }
        } 

        if(phoneIsActive)
        {
            if(InputManager.GetInstance().GetJumpPressed())
            {
                phoneIsActive = false;
                if(guide)
                {
                    guidePanel.SetActive(true);
                }
                settingPanel.SetActive(true);
                blackPanel.SetActive(false);
                appPanel.SetActive(false);
                phoneAnimator.SetBool("phone", false);
                mapPanel.SetActive(false);
                locPanel.SetActive(false);
                soundCheck.SetActive(false);
                chatPanel.SetActive(false);
                homePanel.SetActive(false);
                contactPanel.SetActive(false);
                phoneIcon.SetActive(true);
            }
        }

        if(musicIsActive)
        {
            PlayerPrefs.SetString("Music", "on");
        }

        if(!musicIsActive)
        {
            PlayerPrefs.SetString("Music", "off");
        }        
    
    }

    private IEnumerator SelectFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    private IEnumerator SelectLocFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(locchoices[0].gameObject);
    }

    private IEnumerator SelectContactChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(contchoices[0].gameObject);
    }

    private IEnumerator SelectHomeChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(homechoices[0].gameObject);
    }

    private IEnumerator SelectSetChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(soundToggle.gameObject);
    }

    public void MapMenu()
    {
        appPanel.SetActive(false);
        headerPanel.SetActive(true);
        appName.text = "Map";
        mapPanel.SetActive(true);
        locPanel.SetActive(true);
        StartCoroutine(SelectLocFirstChoice());
    } 

    public void ChatMenu()
    {
        appPanel.SetActive(false);
        headerPanel.SetActive(true);
        appName.text = "Chat";
        contactPanel.SetActive(true);
        StartCoroutine(SelectContactChoice());
    } 

    public void HomeMenu()
    {
        appPanel.SetActive(false);
        headerPanel.SetActive(true);
        appName.text = "Home";
        homePanel.SetActive(true);
        StartCoroutine(SelectHomeChoice());
    } 

    public void SettingsMenu()
    {
        appPanel.SetActive(false);
        headerPanel.SetActive(true);
        appName.text = "Settings";
        soundCheck.SetActive(true);
        StartCoroutine(SelectSetChoice());
    } 

    public void MusicSettings(bool status)
    {
        if(status)
        {
            musicIsActive = false;
            Debug.Log("on");
        }
        else
        {
            musicIsActive = true;
            Debug.Log("off");
        }

    }


    public void LocationChange(string LocName)
    {
        StartCoroutine(LoadLevel(LocName));
        source.PlayOneShot(sound);
    }

    public void ChatPanel(string Name)
    {
        StartCoroutine(LoadLevel(Name));
    }  

    public void Chat(TextAsset inkJSON)
    {
        phoneIsActive = false;
        settingPanel.SetActive(true);
        blackPanel.SetActive(false);
        appPanel.SetActive(false);
        mapPanel.SetActive(false);
        locPanel.SetActive(false);
        chatPanel.SetActive(false);
        homePanel.SetActive(false);
        contactPanel.SetActive(false);
        phoneIcon.SetActive(true);
        ChatManager.GetInstance().EnterDialogueMode(inkJSON);
        phoneAnimator.SetBool("phone", false);
        contchoices[0] = contchoices[1];
        contchoices[1] = contchoices[0];
    }

    public void ChatEnable(GameObject contact)
    {
        contact.SetActive(false);
    }

    public IEnumerator LoadLevel(string areaName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(areaName);
    }

    public void BackButton()
    {
        phoneIsActive = true;
        blackPanel.SetActive(true);
        settingPanel.SetActive(false);
        appPanel.SetActive(true);
        headerPanel.SetActive(false);
        locPanel.SetActive(false);
        chatPanel.SetActive(false);
        homePanel.SetActive(false);
        contactPanel.SetActive(false);
        appName.text = "";
                

        StartCoroutine(SelectFirstChoice());        
    }
}
