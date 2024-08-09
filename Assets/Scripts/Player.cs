using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float viewAngle = 30.0f;  // The angle range in which the player can look at the enemies
    public int rayCount = 10;  // Number of rays to cast within the view angle
    public float maxDistance = 10.0f;  // Maximum distance to check for enemies
    public float holdDuration = 0.5f; // Duration to hold the button to open the door

    private List<Enemy> enemies;
    private HashSet<Enemy> enemiesInSight;
    private float holdTime = 0f;
    private float cooldown = 0.2f;
    private bool onCooldown = false;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI interactText;
    public UnityEngine.UI.Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the list of enemies
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        progressBar.gameObject.SetActive(false);
        interactText.enabled = false;
        messageText.enabled = false;
    }
    void Update()
    {
        FireRaycast();
    }

    void FireRaycast()
    {
        enemiesInSight = new HashSet<Enemy>(); //thx JunHang    
        for (int i = 0; i < rayCount; i++)
        {
            float angle = viewAngle * ((float)i / (rayCount - 1) - 0.5f);  // Calculate angle for each ray
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            //Ray ray = new Ray(tracnsform.position, direction);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                DetectCoilHead(hit);
                CheckForDoor(hit);
                CheckForKeypad(hit);
            }
            MoveCoilHeads();
        }

    }

    void DetectCoilHead(RaycastHit hit)
    {
        Enemy enemy = hit.transform.GetComponent<Enemy>();
        if (enemy)
        {
            enemiesInSight.Add(enemy);
            enemy.SetMovement(false);
        }
    }

    void MoveCoilHeads()
    {
        foreach (Enemy enemy in enemies)
        {
            if (!enemiesInSight.Contains(enemy))
            {
                enemy.SetMovement(true);
            }
        }
    }

    void CheckForDoor(RaycastHit hit)
    {
        Door door = hit.transform.GetComponentInChildren<Door>();
        if (door != null && !onCooldown && !door.autoOpen)
        {
            interactText.enabled = true;
            if (!door.locked)
            {
                if (door.isOpen)
                {
                    interactText.text = "[E]: Close Door";
                }
                else if (!door.isOpen)
                {
                    interactText.text = "[E]: Open Door";
                }
                if (Input.GetKey(KeyCode.E))
                {
                    progressBar.gameObject.SetActive(true);
                    holdTime += Time.deltaTime;
                    progressBar.value = holdTime / holdDuration;
                    if (holdTime >= holdDuration)
                    {
                        door.Open();
                        StartCoroutine(ResetCooldown());
                        holdTime = 0f; // Reset the hold time after opening the door
                        progressBar.gameObject.SetActive(false);
                        interactText.enabled = false;

                    }
                }
                else
                {
                    holdTime = 0f; // Reset the hold time if the button is released
                    progressBar.gameObject.SetActive(false);
                }
            }
            else if (!onCooldown)
            {
                interactText.text = "Door is locked!";
            }
        }
        else
        {
            holdTime = 0f; // Reset the hold time if no door is hit
            interactText.enabled = false;
        }
    }

    void CheckForKeypad(RaycastHit hit)
    {
        KeypadButtons button = hit.transform.GetComponent<KeypadButtons>();
        if (button)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                button.Interact();
            }
            interactText.enabled = true;
            interactText.text = "[M1]: Click";
        }
    }

    private IEnumerator ResetCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public void Message(string message, int timer)
    {
        messageText.text = message;
        messageText.enabled = true;
    }
}
