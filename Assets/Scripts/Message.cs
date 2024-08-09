using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public string message;
    public int time;
    public bool repeatable;
    private bool sent = false;
    private Player player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<Player>();
            if (!sent || repeatable)
            {
                player.Message(message, time);
                sent = true;
            }
        }
    }
}
