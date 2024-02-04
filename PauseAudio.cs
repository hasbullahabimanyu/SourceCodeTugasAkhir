using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 
public class PauseAudio : MonoBehaviour
{

    public string scene;

    void Update()
    {
 
        if ((SceneManager.GetActiveScene().name == scene) || (SceneManager.GetActiveScene().name == "Main menu"))
        {
            Destroy(BGmusic.instance.gameObject);
        }
        
 
    }
}
