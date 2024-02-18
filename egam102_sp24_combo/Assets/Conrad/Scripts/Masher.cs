using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Masher : MonoBehaviour
{
    public float mashLevel;
    public bool mashingDone = false;
    public float timeLeftCheck = 14;
    public AudioSource punchInputSFX;
    public Vector2 targetDest = new Vector2(0,0);
    public Vector2 targetSize = new Vector2(0,0);
    public Animator mashAnimate;
    public float windupTime = 3;
    public EgamMicrogameInstance microgameInstance;
    public GameObject Screambo;
    public AudioSource ScreamboA;
    public GameObject Pow;
    public AudioSource PowA;
    public GameObject Death;
    public AudioSource DeathA;
    public bool screamed = false;
    public bool slammed = false;
    public bool slamped = false;
    public bool slamComplete;
    public GameObject bunchCrags;
    public Vector2 cragLength;
    public float realTime;
    // Start is called before the first frame update
    void Start()
    {
        punchInputSFX = this.GetComponent<AudioSource>();
        mashAnimate = this.GetComponent<Animator>();
        microgameInstance = FindObjectOfType<EgamMicrogameInstance>();
        Screambo = GameObject.Find("Screambo");
        ScreamboA = Screambo.GetComponent<AudioSource>();
        Pow = GameObject.Find("Pow");
        PowA = Pow.GetComponent<AudioSource>();
        Death = GameObject.Find("Death");
        DeathA = Death.GetComponent<AudioSource>();
        slamComplete = mashAnimate.GetBool("Windup");
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeftCheck < 0)
        {
            mashingDone = true;
        }

        if (mashingDone == false)
        {
            if (Input.anyKeyDown == true)
            {
                mashLevel = mashLevel + (Time.deltaTime * 1000);
                punchInputSFX.Play();
                if ((mashLevel > 0) && (mashLevel < 25))
                {
                    this.transform.position = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                    this.transform.localScale = new Vector2(Random.Range(1.1f, 0.9f), Random.Range(1.1f, 0.9f));
                }
                if ((mashLevel > 25) && (mashLevel < 50))
                {
                    this.transform.position = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                    this.transform.localScale = new Vector2(Random.Range(1.15f, 1f), Random.Range(1.15f, 1f));
                }
                if ((mashLevel > 75) && (mashLevel < 100))
                {
                    this.transform.position = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
                    this.transform.localScale = new Vector2(Random.Range(1.2f, 1.1f), Random.Range(1.2f, 1.1f));
                }
                if (mashLevel > 100)
                {
                    this.transform.position = new Vector2(Random.Range(-0.6f, 0.6f), Random.Range(-0.6f, 0.6f));
                    this.transform.localScale = new Vector2(Random.Range(1.4f, 1.1f), Random.Range(1.4f, 1.1f));
                }
            }
            if ((mashLevel > 0) && (mashLevel < 25))
            {
                mashAnimate.SetInteger("LevelUp", 1);
            }
            if ((mashLevel > 25) && (mashLevel < 50))
            {
                mashAnimate.SetInteger("LevelUp", 2);
            }
            if ((mashLevel > 75) && (mashLevel < 100))
            {
                mashAnimate.SetInteger("LevelUp", 3);
            }
            if (mashLevel > 100)
            {
                mashAnimate.SetInteger("LevelUp", 4);
            }
        }
        targetDest = new Vector2(0, 0);
        targetSize = new Vector2(1, 1);
        transform.position = Vector2.Lerp(transform.position, targetDest, 0.1f);
        transform.position = Vector2.Lerp(transform.position, targetSize, 0.1f);
        
        
    }
    private void FixedUpdate()
    {
        if (timeLeftCheck > 0)
        {
            mashLevel = mashLevel - (Time.deltaTime * 10f);
            timeLeftCheck = timeLeftCheck - Time.deltaTime;
        }
        if (mashingDone == true)
        {
            if (mashLevel > 100)
            {
                this.transform.localScale = new Vector2(1, 1);
                if (screamed == false)
                {
                    ScreamboA.Play();
                    screamed = true;
                }
                mashAnimate.SetBool("Victory", true);
                while (windupTime > 0)
                {
                    windupTime = windupTime - Time.deltaTime;
                }
                if (windupTime <= 0)
                {
                    if (slammed == false)
                    {
                        PowA.Play();
                        slammed = true;
                    }
                    mashAnimate.SetBool("Windup", true);
                    while (windupTime > -5)
                    {
                        windupTime -= Time.deltaTime;
                    }
                    if (windupTime <= -5)
                    {
                        microgameInstance.WinGame();
                    }
                }
                realTime = realTime + Time.deltaTime;
                if ((realTime >= 1.7f) && (realTime <= 1.75f))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        cragLength = new Vector2(i + 4, 0);
                        float scale = Random.Range(0.8f, 1.2f);
                        bunchCrags.transform.localScale = new Vector2((float)scale, (float)scale);
                        Instantiate(bunchCrags, cragLength, Quaternion.identity);
                        bunchCrags.transform.localScale = new Vector2((float)scale, (float)scale);
                    }
                } 
                if (realTime >= 3.2f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else
            {
                realTime = realTime + Time.deltaTime;
                if (realTime >= 3.6f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                mashAnimate.SetBool("Death", true);
                while (windupTime > 0)
                {
                    windupTime -= Time.deltaTime;
                }
                if (windupTime <= -0)
                {
                    microgameInstance.LoseGame();
                }
                if (slamped == false)
                {
                    DeathA.Play();
                    slamped = true;
                }
            }
        }
    }
}
