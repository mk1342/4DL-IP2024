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
    public float maxDistance = 2.0f;  // Maximum distance to things
    public float holdDuration = 0.5f; // Duration to hold the button to open the door

    private List<Enemy> enemies;
    private HashSet<Enemy> enemiesInSight;
    private float holdTime = 0f;
    private float cooldown = 0.15f;
    private bool onCooldown = false;

    public TextMeshProUGUI messageText;
    public TextMeshProUGUI interactText;
    public UnityEngine.UI.Slider progressBar;
    public TMP_FontAsset defaultFont;
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
        //for (int i = 0; i < rayCount; i++)
        //{
            //float angle = viewAngle * ((float)i / (rayCount - 1) - 0.5f);  // Calculate angle for each ray
            //Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            //Ray ray = new Ray(tracnsform.position, direction);
        //}
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) //inf range
        {
            DetectCoilHead(hit);
        }
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            CheckForDoor(hit);
            CheckForKeypad(hit);
        }
        else
        {
            progressBar.gameObject.SetActive(false);
            interactText.enabled = false;
        }
        MoveCoilHeads();
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

    public void Message(string message, int timer, TMP_FontAsset font)
    {
        StartCoroutine(TypewriterEffect(message, timer, font));
    }

    private IEnumerator TypewriterEffect(string message, int timer, TMP_FontAsset font)
    {
        messageText.text = "";
        messageText.enabled = true;
        messageText.color = new Color(1, 1, 1, 1);
        if (font)
        {
            messageText.font = font;
        }
        else
        {
            messageText.font = defaultFont;
        }

        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            if (letter.ToString() == ",")
            {
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(timer);
        float fadeDuration = 1f;
        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        messageText.enabled = false;
    }

    public void Killed()
    {
        Debug.Log("i am dead!");
    }
}
