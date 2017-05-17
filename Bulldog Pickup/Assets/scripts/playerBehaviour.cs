using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class playerBehaviour : MonoBehaviour {
    public int startingHealth;
    public int currentHealth;
    public int bulldogsSaved;
    Renderer render; 
    Animator anim;
    CharacterController charControl;
    bool dead;
    Vector3 movement;
    Vector3 prevpos;
    Vector3 newpos;
    Vector3 fwd;
	// Use this for initialization

    void Awake()
    {
        anim = GetComponent<Animator>();
        render = GetComponent<Renderer>();
        charControl = GetComponent<CharacterController>();
    }

	void Start () {
        startingHealth = 100;
        currentHealth = 100;
        bulldogsSaved = 0;
        dead = false;
        prevpos = transform.position;
		
	}

    // Update is called once per frame
    void Update () {
        if (!dead)
        {
            //Calculate movement velocity as a 3D vector
            float _xMov = Input.GetAxis("Horizontal");
            float _zMov = Input.GetAxis("Vertical");

            Vector3 _movHorizontal = transform.right * _xMov;
            Vector3 _movVertical = transform.forward * _zMov;
            
            // Final movement vector
            Vector3 _velocity = (_movHorizontal + _movVertical);

            // Animate movement
            newpos = transform.position;
            movement = (newpos - prevpos);
    
            if (movement != new Vector3(0, 0, 0))
            {
                if (Vector3.Dot(fwd, movement) < 0)
                {
                    Debug.Log("moving backward");
                    anim.SetBool("movingbackward", true);
                    anim.SetBool("movingforward", false);
                }
                else
                {
                    Debug.Log("moving forward");
                    anim.SetBool("movingforward", true);
                    anim.SetBool("movingbackward", false);
                }
            }
            else
            {
                anim.SetBool("movingforward", false);
                anim.SetBool("movingbackward", false);
            }
            prevpos = transform.position;
            fwd = transform.forward;

            //check if fire 
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("firing", true);
            } else
            {
                anim.SetBool("firing", false);
            }


            //Apply movement
            charControl.Move(_velocity*.1f);

            //Calculate rotation as a 3D vector (turning around)
            float _yRot = Input.GetAxisRaw("Mouse X");


            //Apply rotation
            transform.Rotate(0, Input.GetAxis("Horizontal") * 90 * Time.deltaTime, 0);
           
        } else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), .1f);
        }
		
	}


    public IEnumerator getHit(int amount)
    {
        currentHealth -= amount;
        Color currentColor = render.material.color;
        render.material.color = Color.red;
        yield return new WaitForSeconds(.5f);
        render.material.color = currentColor;

        if(currentHealth <= 0)
        {
            dead = true;
        }
    }

}
