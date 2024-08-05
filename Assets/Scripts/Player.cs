using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float viewAngle = 30.0f;  // The angle range in which the player can look at the enemies
    public int rayCount = 10;  // Number of rays to cast within the view angle
    public float maxDistance = 10.0f;  // Maximum distance to check for enemies

    private List<Enemy> enemies;
    private HashSet<Enemy> enemiesInSight;
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the list of enemies
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
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
}
