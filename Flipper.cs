using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    [SerializeField] private Animator Orient;
    public bool flipStart { get; private set; }
    private float speed;
    private static Flipper instance;
    public float horizontal;
    public bool hasRun;
    public bool Rendra;
    public bool Dhya;

    private void Awake()
    {
        instance = this;
    }

    public static Flipper GetInstance()
    {
        return instance;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        flipStart = true;
        if (Rendra)
        {
            Orient.Play("rendra_startr");
        }
        
        if (Dhya)
        {
            Orient.Play("dhya_startr");
        }
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        hasRun = false; 
              
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasRun)
        {
            Move();
        }
    }

    void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); 
        if(!PhoneManager.GetInstance().phoneIsActive)
        {
            if (horizontal > 0f || horizontal < 0f)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                hasRun = true;  
            }
        }
    }
}
