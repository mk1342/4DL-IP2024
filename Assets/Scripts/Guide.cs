using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Guide : MonoBehaviour
{
    public enum GuideState
    {
        NonExistent,
        Idle,
        Following
    }

    public GuideState currentState = GuideState.NonExistent;

    public Transform player;
    public NavMeshAgent agent;
    public float followDistance = 2.0f;
    public float followSpeed = 2.0f;
    public string[] dialogues;
    private int dialogueIndex = 0;

    private void Start()
    {
        // Start in NonExistent state, guide will not appear
        if (currentState == GuideState.NonExistent)
        {
            NonExistent();
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GuideState.NonExistent:
                NonExistent();
                break;

            case GuideState.Idle:
                // Idle state logic (guide stands still)
                break;

            case GuideState.Following:
                FollowPlayer();
                break;
        }
    }

    private void SetState(GuideState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GuideState.NonExistent:
                gameObject.SetActive(false);
                break;

            case GuideState.Idle:
                gameObject.SetActive(true);
                PlayAppearEffect();
                StartCoroutine(ShowMessage());
                break;

            case GuideState.Following:
                break;
        }
    }

    private void FollowPlayer()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position - player.forward * followDistance;
            targetPosition.y = transform.position.y; // Keep the guide on the same vertical level
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.LookAt(player);
        }
    }

    public void TriggerGuideAppearance()
    {
        if (currentState == GuideState.NonExistent)
        {
            // Position the guide in front of the player
            gameObject.SetActive(true);
            Vector3 appearPosition = player.position + player.forward * 4.0f; // 2.0f is the distance in front of the player
            appearPosition.y = player.position.y; // Ensure the guide appears at the same height as the player
            transform.position = appearPosition;
            transform.rotation = Quaternion.LookRotation(player.forward);

            // Set the state to Idle, which will make the guide appear and display a message
            SetState(GuideState.Idle);
        }
    }

    public void StartFollowing()
    {
        if (currentState == GuideState.Idle)
        {
            SetState(GuideState.Following);
        }
    }

    private void PlayAppearEffect()
    {

    }

    private void NonExistent()
    {
        gameObject.SetActive(false);
    }

    private IEnumerator ShowMessage()
    {
        if (dialogueIndex < dialogues.Length)
        {
            Player playerS = player.GetComponent<Player>();
            playerS.Message(dialogues[dialogueIndex], 5);
            dialogueIndex++;
            yield return new WaitForSeconds(5);
        }
    }
}
