using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraController : MonoBehaviour
{
    public TMP_Text movementSpeed;
    public TMP_Text detectionRange;
    public TMP_Text maxHunger;
    public TMP_Text mateThreshold;
    public TMP_Text gestationPeriod;
    public TMP_Text startingDevelopment;
    public TMP_Text developementSpeed;
    public TMP_Text intimidation;
    public TMP_Text lifeSpan;
    public TMP_Text attackDamage;
    public TMP_Text health;
    public TMP_Text healthRegen;
    public TMP_Text litter;
    public TMP_Text currentHealth;
    public TMP_Text hunger;
    public float mouseSensitivitiy = 500f;
    public float moveSpeed = 10;
    public float sprintSpeed = 100;
    float verticalLookRotation = 0f;
    Specimen target = null;
    float scroll = 10;
    bool toggleThirdPerson;

    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateStats();
        transform.localPosition = Vector3.zero;
        if(!toggleThirdPerson)
        { 
            if (transform.parent.position.x > Spawner.WorldBounds) { transform.parent.position = new Vector3(Spawner.WorldBounds, transform.parent.position.y, transform.parent.position.z); }
            if (transform.parent.position.x < -Spawner.WorldBounds) { transform.parent.position = new Vector3(-Spawner.WorldBounds, transform.parent.position.y, transform.parent.position.z); }
            if (transform.parent.position.z > Spawner.WorldBounds) { transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, Spawner.WorldBounds); }
            if (transform.parent.position.z < -Spawner.WorldBounds) { transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y, -Spawner.WorldBounds); }
            if (transform.parent.position.y > 30) { transform.parent.position = new Vector3(transform.position.x, 30, transform.parent.position.z); }
            if (transform.parent.position.y < 5) { transform.parent.position = new Vector3(transform.parent.position.x, 5, transform.parent.position.z); }
        }
        if (Cursor.lockState == CursorLockMode.Locked)
        {
			if (Input.GetMouseButtonDown(1)) { target = null; scroll = 10; toggleThirdPerson = false; }
            if (target)
            {
                if(Input.GetKeyDown(KeyCode.Tab)) { toggleThirdPerson = !toggleThirdPerson; }
                scroll -= Input.mouseScrollDelta.y;
                if(scroll < 5) { scroll = 5; }
                if(scroll > 30) { scroll = 30; }
                Vector3 pos = target.transform.position;
                if (toggleThirdPerson) 
                {
                    pos = target.transform.position - target.transform.forward * 10;
                    transform.localEulerAngles = Vector3.zero;
                    if (Input.GetKey(KeyCode.LeftAlt))
					{
                        pos = target.transform.position - target.transform.forward * -10;
                        transform.localEulerAngles = new Vector3(0, 180, 0);
					}
                    transform.parent.rotation = target.transform.rotation;
                }
                pos.y = scroll; 
                transform.parent.position = pos;
            }
            else
            {
                toggleThirdPerson = false;
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                Vector3 up = Vector3.zero;
                Vector3 down = Vector3.zero;


                if (Input.GetKey(KeyCode.Q)) { down = -transform.parent.up; }
                if (Input.GetKey(KeyCode.E)) { up = transform.parent.up; }

                Vector3 move = (up + down + transform.parent.right * x + transform.parent.forward * z);
                if (Input.GetKey(KeyCode.LeftShift)) { transform.parent.position = (transform.parent.position + (move * sprintSpeed * Time.deltaTime)); }
                else { transform.parent.position = (transform.parent.position + (move * moveSpeed * Time.deltaTime)); }

                if (Input.GetButtonDown("Fire1"))
                {
                    RaycastHit hitInfo;
                    if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 100))
                    { target = hitInfo.transform.gameObject.GetComponent<Specimen>(); }
                }
            }
            if (!toggleThirdPerson)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivitiy * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivitiy * Time.deltaTime;

                transform.parent.Rotate(Vector3.up * mouseX);

                verticalLookRotation -= mouseY;

                verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

                transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
            }
        }
	}
    void UpdateStats()
	{
        if (target)
        {
            movementSpeed.text = "Movement Speed: " + target.movementSpeed.Stat;
            detectionRange.text = "Detection Range: " + target.detectionRange.Stat;
            maxHunger.text = "Max Hunger: " + target.maxHunger.Stat;
            mateThreshold.text = "Mate Food Threshold: " + target.mateThreshold.Stat; ;
            gestationPeriod.text = "Gestation Period: " + target.gestationPeriod.Stat; ;
            startingDevelopment.text = "Starting Development: " + target.startingDevelopment.Stat; ;
            developementSpeed.text = "Development Speed: " + target.developementSpeed.Stat; ;
            intimidation.text = "Intimidation: " + target.intimidation.Stat;
            lifeSpan.text = "Life Span: " + target.lifeSpan.Stat; ;
            attackDamage.text = "Attack Damage: " + target.attackDamage.Stat;
            health.text = "Health: " + target.health.Stat;
            healthRegen.text = "Health Regen: " + target.healthRegen.Stat;
            litter.text = "Litter Size: " + target.litter.Stat;
            currentHealth.text = "Current Health: " + target.currentHealth;
            hunger.text = "Current Hunger: " + target.hunger;
        }
		else
		{
            movementSpeed.text = "";
            detectionRange.text = "";
            maxHunger.text = "";
            mateThreshold.text= "";
            gestationPeriod.text = "";
            startingDevelopment.text = "";
            developementSpeed.text = "";
            intimidation.text = "";
            lifeSpan.text = "";
            attackDamage.text = "";
            health.text = "";
            healthRegen.text = "";
            litter.text = "";
            currentHealth.text = "";
            hunger.text = "";
        }
    }
}
