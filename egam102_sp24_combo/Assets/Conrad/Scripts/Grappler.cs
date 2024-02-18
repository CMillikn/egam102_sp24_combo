using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Grappler : MonoBehaviour
{
    public bool GrapplerDead = false;
    public bool Started = true;
    public int StatusNumber;
    public int successNumber = 0;
    public bool successZero = false;
    public bool successOne = false;
    public bool successTwo = false;
    public bool successThree = false;
    public bool fullSuccess = false;
    public bool aniGrapplerGrab = false;
    public bool aniGrapplerFly = false;
    public bool aniGrapplerDie = false;
    public float anticipationTimer = 1f;
    public float TimeRemaining = 15f;
    public float EndTimer = 2f;
    public float grapplerFlying = 0;
    public bool bigJump = false;
    public Animator GrapplerAnimator;
    public GameObject FireballO;
    public SpriteRenderer FireballV;
    public GameObject ShoryuO;
    public SpriteRenderer ShoryuV;
    public GameObject SPDO;
    public SpriteRenderer SPDV;
    public GameObject DoubleFireballO;
    public SpriteRenderer DoubleFireballV;
    public GameObject JSRGraf1;
    public AudioSource JSRGraf1A;
    public GameObject JSRGraf2;
    public AudioSource JSRGraf2A;
    public GameObject JSRGraf3;
    public AudioSource JSRGraf3A;
    public GameObject JSRGraf4;
    public AudioSource JSRGraf4A;
    public GameObject JSRGrafWin;
    public AudioSource JSRGrafWinA;
    public GameObject ManHit;
    public AudioSource ManHitA;
    public GameObject DBZJump;
    public AudioSource DBZJumpA;
    public GameObject DBZGrab;
    public AudioSource DBZGrabA;
    public enum Status
    {
        Fireball, 
        Shoryu, 
        SPD, 
        DoubleFireball 
    }
    public Status currentState;


    void Start()
    {
        //find all relevant game objects
        GrapplerAnimator = GetComponent<Animator>();
        int Seed = ((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        UnityEngine.Random.InitState(Seed);
        int num = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Status)).Length - 1);
        this.currentState = (Status)num;
        FireballO = GameObject.Find("Fireball");
        FireballV = FireballO.GetComponent<SpriteRenderer>();
        FireballV.enabled = false;
        ShoryuO = GameObject.Find("Shoryu");
        ShoryuV = ShoryuO.GetComponent<SpriteRenderer>();
        ShoryuV.enabled = false;
        SPDO = GameObject.Find("SPD");
        SPDV = SPDO.GetComponent<SpriteRenderer>();
        SPDV.enabled = false;
        DoubleFireballO = GameObject.Find("DoubleFireball");
        DoubleFireballV = DoubleFireballO.GetComponent<SpriteRenderer>();
        DoubleFireballV.enabled = false;
        JSRGraf1 = GameObject.Find("JSRGraf1");
        JSRGraf1A = JSRGraf1.GetComponent<AudioSource>();

        JSRGraf2 = GameObject.Find("JSRGraf2");
        JSRGraf2A = JSRGraf2.GetComponent<AudioSource>();

        JSRGraf3 = GameObject.Find("JSRGraf3");
        JSRGraf3A = JSRGraf3.GetComponent<AudioSource>();

        JSRGraf4 = GameObject.Find("JSRGraf4");
        JSRGraf4A = JSRGraf4.GetComponent<AudioSource>();

        JSRGrafWin = GameObject.Find("JSRGrafWin");
        JSRGrafWinA = JSRGrafWin.GetComponent<AudioSource>();

        ManHit = GameObject.Find("ManHit");
        ManHitA = ManHit.GetComponent<AudioSource>();

        DBZGrab = GameObject.Find("DBZGrab");
        DBZGrabA = DBZGrab.GetComponent<AudioSource>();

        DBZJump = GameObject.Find("DBZJump");
        DBZJumpA = DBZJump.GetComponent<AudioSource>();

        GrapplerAnimator.SetBool("aniGrapplerDie", false);
        GrapplerAnimator.SetBool("aniGrapplerGrab", false);
        GrapplerAnimator.SetBool("aniGrapplerFly", false);
    }

    void Update()
    {
        //currentState = 0-3; //for statetesting, comment this out as needed
        if (GrapplerDead == false)
        {
            switch (currentState)
            {
                case Status.Fireball:
                    UpdateFireball();
                    break;

                case Status.Shoryu:
                    UpdateShoryu();
                    break;

                case Status.SPD:
                    UpdateSPD();
                    break;

                case Status.DoubleFireball:
                    UpdateDoubleFireball();
                    break;
            }
        }
        else
        {
            UpdateDead();
        }
    }
    private void FixedUpdate()
    {
        TimeRemaining = TimeRemaining - Time.deltaTime;
        if ((TimeRemaining <= 0) && (fullSuccess == false))
        {
            GrapplerDead = true;
        }
        if (fullSuccess == true)
        {
            anticipationTimer = anticipationTimer - Time.deltaTime;
            if (anticipationTimer <= 0)
            {
                this.transform.position = new Vector2(-1, grapplerFlying);
                grapplerFlying = (grapplerFlying + (Time.deltaTime * 30));
                aniGrapplerFly = true;
                GrapplerAnimator.SetBool("aniGrapplerFly", true);
            }
            if ((anticipationTimer <= 0) && (bigJump == false))
            {
                DBZJumpA.Play();
                bigJump = true;
            }
            EndTimer = EndTimer - Time.deltaTime;
            if (EndTimer <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    void UpdateFireball()
    {
        FireballV.enabled = true;
        if ((Input.anyKeyDown) && (fullSuccess == false)) 
        {
            if ((Input.GetKey(KeyCode.DownArrow)) && (successZero == false))
            {
                JSRGraf1A.Play();
                successZero = true;
                Debug.Log("One Down");
            }
            else if ((Input.GetKey(KeyCode.RightArrow)) && (successZero == true) && (successOne == false))
            {
                JSRGraf2A.Play();
                successOne = true;
                Debug.Log("Two Down");
            }
            else if ((Input.GetKey(KeyCode.Space)) && (successOne == true) && (successTwo == false))
            {
                JSRGrafWinA.Play();
                fullSuccess = true;
                Debug.Log("Nice");
                aniGrapplerGrab = true;
                GrapplerAnimator.SetBool("aniGrapplerGrab", true);
                DBZGrabA.Play();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                //Fodder statement to prevent game loss through clicking to refocus
            }
            else
            {
                GrapplerDead = true;
                ManHitA.Play();
                Debug.Log("Something Went Wrong");
            }
        }
    }
    void UpdateShoryu()
    {
        ShoryuV.enabled = true;
        if ((Input.anyKeyDown) && (fullSuccess == false))
        {
            if ((Input.GetKey(KeyCode.RightArrow)) && (successZero == false))
            {
                JSRGraf1A.Play();
                successZero = true;
                Debug.Log("One Down");
            }
            else if ((Input.GetKey(KeyCode.DownArrow)) && (successZero == true) && (successOne == false))
            {
                JSRGraf2A.Play();
                successOne = true;
                Debug.Log("Two Down");
            }
            else if ((Input.GetKey(KeyCode.RightArrow)) && (successOne == true) && (successTwo == false))
            {
                JSRGraf3A.Play();
                successTwo = true;
                Debug.Log("Three Down");
            }
            else if ((Input.GetKey(KeyCode.Space)) && (successTwo == true) && (fullSuccess == false))
            {
                JSRGrafWinA.Play();
                fullSuccess = true;
                Debug.Log("Nice");
                aniGrapplerGrab = true;
                GrapplerAnimator.SetBool("aniGrapplerGrab", true);
                DBZGrabA.Play();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                //Fodder statement to prevent game loss through clicking to refocus
            }
            else
            {
                GrapplerDead = true;
                ManHitA.Play();
                Debug.Log("Something Went Wrong");
            }
        }
    }
    void UpdateSPD()
    {
        SPDV.enabled = true;
        if ((Input.anyKeyDown) && (fullSuccess == false))
        {
            if ((Input.GetKey(KeyCode.LeftArrow)) && (successZero == false))
            {
                JSRGraf1A.Play();
                successZero = true;
                Debug.Log("One Down");
            }
            else if ((Input.GetKey(KeyCode.DownArrow)) && (successZero == true) && (successOne == false))
            {
                JSRGraf2A.Play();
                successOne = true;
                Debug.Log("Two Down");
            }
            else if ((Input.GetKey(KeyCode.RightArrow)) && (successOne == true) && (successTwo == false))
            {
                JSRGraf3A.Play();
                successTwo = true;
                Debug.Log("Three Down");
            }
            else if ((Input.GetKey(KeyCode.UpArrow)) && (successTwo == true) && (successThree == false))
            {
                JSRGraf4A.Play();
                successThree = true;
                Debug.Log("Four Down");
            }
            else if ((Input.GetKey(KeyCode.Space)) && (successThree == true) && (fullSuccess == false))
            {
                JSRGrafWinA.Play();
                fullSuccess = true;
                Debug.Log("Nice");
                aniGrapplerGrab = true;
                GrapplerAnimator.SetBool("aniGrapplerGrab", true);
                DBZGrabA.Play();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                //Fodder statement to prevent game loss through clicking to refocus
            }
            else
            {
                GrapplerDead = true;
                ManHitA.Play();
                Debug.Log("Something Went Wrong");
            }
        }
    }
    void UpdateDoubleFireball()
    {
        DoubleFireballV.enabled = true;
        if ((Input.anyKeyDown) && (fullSuccess == false))
        {
            if ((Input.GetKey(KeyCode.DownArrow)) && (successZero == false))
            {
                JSRGraf1A.Play();
                successZero = true;
                Debug.Log("One Down");
            }
            else if ((Input.GetKey(KeyCode.RightArrow)) && (successZero == true) && (successOne == false))
            {
                JSRGraf2A.Play();
                successOne = true;
                Debug.Log("Two Down");
            }
            else if ((Input.GetKey(KeyCode.DownArrow)) && (successOne == true) && (successTwo == false))
            {
                JSRGraf3A.Play();
                successTwo = true;
                Debug.Log("Three Down");
            }
            else if ((Input.GetKey(KeyCode.RightArrow)) && (successTwo == true) && (successThree == false))
            {
                JSRGraf4A.Play();
                successThree = true;
                Debug.Log("Four Down");
            }
            else if ((Input.GetKey(KeyCode.Space)) && (successThree == true) && (fullSuccess == false))
            {
                JSRGrafWinA.Play();
                fullSuccess = true;
                Debug.Log("Nice");
                aniGrapplerGrab = true;
                GrapplerAnimator.SetBool("aniGrapplerGrab", true);
                DBZGrabA.Play();
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                //Fodder statement to prevent game loss through clicking to refocus
            }
            else
            {
                GrapplerDead = true;
                ManHitA.Play();
                Debug.Log("Something Went Wrong");
            }
        }
    }
    void UpdateDead()
    {
        aniGrapplerDie = true;
        GrapplerAnimator.SetBool("aniGrapplerDie", true);
        EndTimer = EndTimer - Time.deltaTime; 
        if (EndTimer <=0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
