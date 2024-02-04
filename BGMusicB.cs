using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicB : MonoBehaviour
{
    public static BGMusicB instance;
    private string status;

    AudioSource audioSource;    
 
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }       
  
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(PhoneManager.GetInstance().musicIsActive)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }



    }
    
}

