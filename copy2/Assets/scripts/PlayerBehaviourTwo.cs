using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourTwo : MonoBehaviour {
        public int startingHealth;
        public int currentHealth;
        public int bulldogsSaved;
        Renderer render;
        Animator anim;
        bool dead;
     public Texture2D crosshairImage;
    // Use this for initialization
    void Start () {
            startingHealth = 100;
            currentHealth = 100;
            bulldogsSaved = 0;
            dead = false;
        }
	
	// Update is called once per frame
	void Update () {
		if(dead)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), .1f);
        }
	}
    void OnGUI()
    {
        float xMin = (Screen.width * 3 / 4) - (crosshairImage.width / 2);
        float yMin = (Screen.height / 2) - (crosshairImage.height / 2);
        GUI.DrawTexture(new Rect(xMin, yMin, crosshairImage.width, crosshairImage.height), crosshairImage);
    }
    public void getHit(int amount)
    {
        Debug.Log("hit");
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            dead = true;
        }
    }
}
