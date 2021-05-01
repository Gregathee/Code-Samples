/* * (Student Name) 
 * * (Assignment 4) 
 * * Controls the player with input. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    public float jumpForce;
	public ForceMode forceMode;
	public float gravityModifier;

	ScoreManager scoreManager;

	public bool gameOver = false;
	public bool isOnGround = true;

	public Animator playerAnimator;
	public ParticleSystem explosion;
	public ParticleSystem dirt;

	public AudioClip crash;
	public AudioClip jump;

	public AudioSource source;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		forceMode = ForceMode.Impulse;
		if(Physics.gravity.y > -10)
		Physics.gravity *= gravityModifier;
		playerAnimator = GetComponent<Animator>();
		playerAnimator.SetFloat("Speed_f", 1.0f);
		scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
		source = gameObject.AddComponent<AudioSource>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space) && isOnGround && !scoreManager.gameOver)
		{
			rigidbody.AddForce(Vector3.up * jumpForce, forceMode);
			isOnGround = false;
			playerAnimator.SetTrigger("Jump_trig");
			dirt.Stop();
			source.PlayOneShot(jump);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isOnGround = true;
			dirt.Play();
		}
		if(collision.gameObject.CompareTag("Obstacle"))
		{
			scoreManager.gameOver = true;
			playerAnimator.SetBool("Death_b", true);
			if(transform.position.y > 0.75f){ playerAnimator.SetInteger("DeathType_int", 2);}
			else { playerAnimator.SetInteger("DeathType_int", 1); }
			explosion.Play();
			dirt.Stop();
			source.PlayOneShot(crash, 2f);
		}
	}
}
