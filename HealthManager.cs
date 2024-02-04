using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI pointText;    
    [SerializeField] private Animator pointAnimator;
    [SerializeField] private GameObject overPanel;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject phonePanel;
    [SerializeField] private GameObject[] overchoices;
    [SerializeField] private GameObject objectivePanel;
    [SerializeField] private Animator satAnimator;
    [SerializeField] private Animator overAnimator;
    public bool notgameOver { get; private set; }
    public HealthBar healthBar;
    public int levelHealth;
    public bool levelStart = false;
    public static int currentHealth;
    public float transitionTime = 1f;
    private int healthpoint;
    public bool Rendra;
    public string levelString;
    [SerializeField] public Animator transition;
    private static HealthManager instance;

    // Start is called before the first frame update
    private void Awake() 
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;

    }    
    
    void Start()
    { 
        notgameOver = true;
        currentHealth = levelHealth;
        overPanel.SetActive(false);
        if (PlayerPrefs.GetInt("Health") != 0)
        {
            currentHealth = PlayerPrefs.GetInt("Health");
        }
        healthBar.SetHealth(currentHealth);
        healthText.text = (currentHealth +"/100");  

        if (Rendra)
        {
            if (levelStart)
            {
                currentHealth = levelHealth;
                PlayerPrefs.SetString("Level", levelString);
            } 
        }

        if (!Rendra)
        {
            if (levelStart)
            {
                levelHealth = currentHealth;
                currentHealth = levelHealth;
                PlayerPrefs.SetInt("LevelHealth", levelHealth); 
                PlayerPrefs.SetString("Level", levelString);
            }         
        }
    }

    public static HealthManager GetInstance() 
    {
        return instance;
    }    

    // Update is called once per frame
    void Update()
    {
        healthText.text = (currentHealth +"/100");
        
        if (Rendra)
        {

            if (currentHealth < 15)
            {
                satAnimator.Play("satdec");
            }

            if (currentHealth > 14)
            {
                satAnimator.Play("satinc");
            }
        }


    


        if (currentHealth == 0)
        {
            DialogueManager.GetInstance().StopDialogue();
            GameOverTrigger();

        }                   
    }

    private void GameOverTrigger()
    {
        notgameOver = false;
        DialogueManager.GetInstance().StopDialogue();
        dialoguePanel.SetActive(false);
        phonePanel.SetActive(false);
        overPanel.SetActive(true);
        objectivePanel.SetActive(false);
        StartCoroutine(SelectOverFirstChoice());
        overAnimator.Play("gameover");
        PlayerPrefs.DeleteKey("Health");
    }

    private IEnumerator SelectOverFirstChoice() 
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(overchoices[0].gameObject);
    }
    public void LoadNextArea(string areaName)
    {
        StartCoroutine(LoadLevel(areaName));
    }

    public IEnumerator LoadLevel(string areaName)
    {
        transition.SetTrigger("Start");
        currentHealth = levelHealth;
        overPanel.SetActive(false);
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(areaName);
    }

    public void GetSetHealth(int Health)
    {
        healthBar.SetHealth(currentHealth + Health);
        currentHealth += Health;
        if (currentHealth > 100)
            {
                currentHealth = 100;
            }

        if (currentHealth < 1)
            {
                currentHealth = 0;

            }                    
        healthText.text = (currentHealth +"/100");
        pointText.text = (Health.ToString());
        if (Health > 0)
            {
                pointText.text = ("+" + Health);
                pointAnimator.Play("point");
                pointAnimator.Play("point_idel");
            }
        if (Health < 0)
            {
                pointText.text = (Health.ToString());
                pointAnimator.Play("point");
                pointAnimator.Play("point_idel");
            }
        if (Health == 0)
            {
                pointText.text = (Health.ToString());
                pointAnimator.Play("point_idel");
            }
        PlayerPrefs.SetInt("Health", currentHealth); 
               

    }         
}
