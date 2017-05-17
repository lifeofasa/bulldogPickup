using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerMoveRotate))]
public class playerBehaviour : MonoBehaviour {
    public int startingHealth;
    public int currentHealth;
    public int bulldogsSaved;
    Renderer render; 
    Animator anim;
    CharacterController charControl;
    Camera cam;
    bool dead;
    Vector3 movement;
    Vector3 prevpos;
    Vector3 newpos;
    Vector3 fwd;
    public Texture2D crosshairImage;
    PlayerBehaviourTwo otherScript;
    // Use this for initialization
    private PlayerMoveRotate motor;

    /*
    private PlayerMoveRotate motor;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float lookSensitivity = 3f;
    */
    void Awake()
    {
        anim = GetComponent<Animator>();
        render = GetComponent<Renderer>();
        charControl = GetComponent<CharacterController>();
        cam = GameObject.Find("Player One Camera").GetComponent<Camera>();
        motor = GetComponent<PlayerMoveRotate>();

    }


    void Start () {
        startingHealth = 100;
        currentHealth = 100;
        bulldogsSaved = 0;
        dead = false;
        prevpos = transform.position;
        
        otherScript = GameObject.Find("ArmyPilotTwo").GetComponent<PlayerBehaviourTwo>();
    }

    void OnGUI()
    {
        float xMin = (Screen.width / 4) - (crosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
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

            /*
            //MOVEMENT STUFF//
            //Calculate movement velocity as a 3D vector
            float _xMov = Input.GetAxis("Horizontal");
            float _zMov = Input.GetAxis("Vertical");

            Vector3 _movHorizontal = transform.right * _xMov;
            Vector3 _movVertical = transform.forward * _zMov;

            // Final movement vector
            Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

            //ROTATION STUFF//
            //Apply movement
            motor.Move(_velocity);

            //Calculate rotation as a 3D vector (turning around)
            float _yRot = Input.GetAxisRaw("Mouse X");

            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

            //Apply rotation
            motor.Rotate(_rotation);

            //Calculate camera rotation as a 3D vector (turning around)
            float _xRot = Input.GetAxisRaw("Mouse Y");

            float _cameraRotationX = _xRot * lookSensitivity;

            //Apply camera rotation
            motor.RotateCamera(_cameraRotationX);
            */

            //ROTATION STUFF//
            //Calculate rotation as a 3D vector (turning around)
            float _yRot = Input.GetAxisRaw("Mouse X");

            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * 3f;

            //Apply rotation
            motor.Rotate(_rotation);

            //Calculate camera rotation as a 3D vector (turning around)
            float _xRot = Input.GetAxisRaw("Mouse Y");

            float _cameraRotationX = _xRot * 3f;

            //Apply camera rotation
            motor.RotateCamera(_cameraRotationX);

            // Animate movement
            newpos = transform.position;
            movement = (newpos - prevpos);
            if (movement != new Vector3(0, 0, 0))
            {
                if (Vector3.Dot(fwd, movement) < 0)
                {
                    anim.SetBool("movingbackward", true);
                    anim.SetBool("movingforward", false);
                }
                else
                {
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
               /* RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, hit, 100))
                {
                    Debug.Log("cast ray");
                    if (hit.collider.tag == "Player Two")
                    {

                        otherScript.getHit(20);
                    }
                }
                */
            }
            else
            {
                anim.SetBool("firing", false);
            }


            //Apply movement
            charControl.Move(_velocity*.1f);


            //Apply rotation
           // transform.Rotate(0, Input.GetAxis("Horizontal") * 90 * Time.deltaTime, 0);
           
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
