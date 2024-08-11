using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TMP_FontAsset font;
    public bool startDialogue;
    public int timer = 5;

    private void Start()
    {
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
            // Calculate the target position behind the player
            Vector3 targetPosition = player.position - player.forward * followDistance;

            // Set the target position height to the same as the agent's current position
            targetPosition.y = agent.transform.position.y;

            // Use the NavMeshAgent to move towards the target position
            agent.speed = followSpeed;
            agent.SetDestination(targetPosition);

            // Ensure the guide is facing the player while moving
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * followSpeed);
        }
    }

    public void TriggerGuideAppearance()
    {
        if (currentState == GuideState.NonExistent)
        {
            // Position the guide in front of the player
            gameObject.SetActive(true);
            Vector3 appearPosition = player.position + player.forward * 4.0f;
            appearPosition.y = player.position.y;
            transform.position = appearPosition;
            transform.rotation = Quaternion.LookRotation(player.forward);
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
        if (startDialogue)
        {
            for (int dialogueIndex = 0; dialogueIndex < dialogues.Length; dialogueIndex++)
            {
                Player playerS = player.GetComponent<Player>();
                playerS.Message(dialogues[dialogueIndex], timer, font);
                yield return new WaitForSeconds(timer + 2 + dialogues[dialogueIndex].Length * 0.05f);
            }
        }
        StartFollowing();
    }
}
