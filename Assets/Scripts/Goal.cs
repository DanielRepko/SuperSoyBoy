﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioClip goalClip;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            var audioSource = GetComponent<AudioSource>();
            if(audioSource != null && goalClip != null)
            {
                audioSource.PlayOneShot(goalClip);
            }

            var timer = FindObjectOfType<Timer>();

            GameManager.instance.SaveTime(timer.time);

            GameManager.instance.RestartLevel(0.5f);
        }
    }
}
