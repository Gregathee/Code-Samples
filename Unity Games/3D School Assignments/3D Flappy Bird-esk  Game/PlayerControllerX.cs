using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounce;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y > 14)
        {
            transform.position = new Vector3(-3, 14, 0);
            playerRb.AddForce(-Vector3.up * floatForce, ForceMode.Force);
        }
        else
        {
            // While space is pressed and player is low enough, float up
            if (Input.GetKey(KeyCode.Space) && !gameOver)
            {
                playerRb.AddForce(Vector3.up * floatForce, ForceMode.Force);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>().score++;
            Destroy(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Ground"))
		{
            playerAudio.PlayOneShot(bounce);
            playerRb.AddForce(Vector3.up, ForceMode.Impulse);
        }

    }

}
