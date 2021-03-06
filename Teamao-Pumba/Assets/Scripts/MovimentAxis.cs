﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentAxis : MonoBehaviour
{
    private Animator anim;
    public GameObject StunUI;
    public float movementSpeed;
    public float rotationSpeed;

    private bool stunned = false;

    //Troca de textura pro stun
    public Texture MainTexture, StunTexture;
    public GameObject corpo;
    Renderer renderer;

    public GameObject Steps;

    public ParticleSystem dust;
    //public ParticleSystem splash;

    void Start()
    {
        renderer = corpo.GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        if (!IsMoving) return;

        float translationV = 0;
        float translationH = 0;
        for (int i = 0; i < 4; i++)
        {
            if (gameObject.tag == "Player" + (i + 1).ToString())
            {
                translationV = Input.GetAxis("Vertical" + (i + 1).ToString()) * movementSpeed;
                translationH = Input.GetAxis("Horizontal" + (i + 1).ToString()) * movementSpeed;


            }
            if (((translationV > 1 || translationV < -1) || (translationH > 1 || translationH < -1)) && (gameObject.name == "tucano" || gameObject.name == "capivara" || gameObject.name == "lico" || gameObject.name == "jess"))
            {
                anim.SetBool("running", true);
            }
            else
            {
                if (gameObject.name == "tucano" || gameObject.name == "capivara" || gameObject.name == "lico" || gameObject.name == "jess")
                {
                    anim.SetBool("running", false);
                }
            }
        }
        translationV *= Time.deltaTime;
        translationH *= Time.deltaTime;

        transform.rotation = Rotation;


    }
    void Update()
    {
        anim = this.GetComponent<Animator>();
        if (IsMoving == false && (gameObject.name == "tucano" || gameObject.name == "capivara" || gameObject.name == "lico" || gameObject.name == "jess"))
        {
            anim.SetBool("running", false);
            dust.Play();
        }
        if (stunned)
        {
            StunUI.SetActive(true);
            anim.SetBool("stunned", true);
            renderer.material.SetTexture("_MainTex", StunTexture);
        }
        if (!stunned)
        {
            StunUI.SetActive(false);
            transform.position += Direction() * Time.deltaTime;
            anim.SetBool("stunned", false);
            renderer.material.SetTexture("_MainTex", MainTexture);
        }
        if (IsMoving)
        {
            Steps.SetActive(true);
        }
        else Steps.SetActive(false);
    }

    private bool IsMoving => Direction() != Vector3.zero;
    private Vector3 Direction()
    {
        for (int i = 0; i < 4; i++)
        {
            if (gameObject.tag == "Player" + (i + 1).ToString())
            {
                float h = Input.GetAxis("Horizontal" + (i + 1).ToString()) * movementSpeed;

                float v = Input.GetAxis("Vertical" + (i + 1).ToString()) * movementSpeed;
                return new Vector3(h, 0, v);

            }
        }
        return Vector3.zero;

    }
    private Quaternion Rotation => Quaternion.LookRotation(RotationDirection);

    private Vector3 RotationDirection => Vector3.RotateTowards(transform.forward, Direction(), rotationSpeed * Time.deltaTime, 0);

    public void stunSelf(float stunDuration)
    {
        if (!stunned)
        {
            StartCoroutine(stunRoutine(stunDuration));
        }

    }
    IEnumerator stunRoutine(float stunDuration)
    {
        this.stunned = true;

        yield return new WaitForSeconds(stunDuration);

        this.stunned = false;
    }


}