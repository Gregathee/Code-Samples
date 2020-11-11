/*
* Greg Brandt
* Assignment 7
* Controls Players
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody playerRb;
    GameObject focalPoint;
    [SerializeField] GameObject powerupIndicator;
    [SerializeField] TMP_Text gameOverText;
    public bool hasPowerup;
    private float PowerupStrength = 15;
    bool textAcknowledged = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        if(!textAcknowledged && Input.GetKeyDown(KeyCode.Space)) { textAcknowledged = true; gameOverText.text = ""; }
        else if(!textAcknowledged){ gameOverText.text = "-Fall off and you lose.\n\n -Survive to round 10 to win.\n\n Press -Space to continue"; }
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        if(transform.position.y < -1 || SpawnManager.waveNumber > 9) 
        {
            if (SpawnManager.waveNumber < 10) { gameOverText.text = "You Lose! Press R to restart"; }
            else { gameOverText.text = "You Win! Press R to restart"; }
            if (Input.GetKeyDown(KeyCode.R)) { SpawnManager.waveNumber = 1; SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            powerupIndicator.SetActive(true);
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * PowerupStrength, ForceMode.Impulse);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(10);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }
}
