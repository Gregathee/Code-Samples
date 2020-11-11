/*
* Greg Brandt
* Assignment 7
* Controls Players
*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerX : MonoBehaviour
{
    [SerializeField] TMP_Text gameOverText;
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    public ParticleSystem smoke;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    bool textAcknowledged = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    IEnumerator Boost()
    {
        smoke.Play();
        playerRb.AddForce(focalPoint.transform.forward * speed * 2 * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        smoke.Stop();
    }

    void Update()
    {
        if (!textAcknowledged && Input.GetKeyDown(KeyCode.Space)) { textAcknowledged = true; gameOverText.text = ""; }
        else if (!textAcknowledged) { gameOverText.text = "-If all enmies go through your goal, you lose.\n\n -Survive to round 10 to win.\n\n Press -Space to continue"; }
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine( Boost());
        }
        if (EnemyX.enemiesInPlayerGoal == SpawnManagerX.waveCount || SpawnManagerX.waveCount > 9)
        {
            if (SpawnManagerX.waveCount < 10) { gameOverText.text = "You Lose! Press R to restart"; }
            else { gameOverText.text = "You Win! Press R to restart"; }
            if (Input.GetKeyDown(KeyCode.R)) { SpawnManagerX.waveCount = 0; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        }
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }



}
