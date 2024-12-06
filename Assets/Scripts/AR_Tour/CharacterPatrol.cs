using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPatrol : MonoBehaviour
{
    public static event Action<int> OnInfoPointReached;

    public Transform[] patrolPoints;
    public List<int> broadcastPoints;
    private int currentBroadcastIndex;

    public int targetPoint;
    public float speed;
    public float rotationSpeed ; // Speed of rotation
    public float waitTime ; // Time to wait at each patrol point

    private bool isWaiting = false;
    private Coroutine patrolCoroutine;

    [SerializeField] private bool isMainCharacter; //so that only one broadcast is going
    private Animator animator; // Reference to the Animator

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void OnEnable()
    {
        if (patrolCoroutine == null)  // Make sure it doesn't start multiple times
        {
            patrolCoroutine = StartCoroutine(Patrol());
        }
    }

    private void OnDisable()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);  // Stop the coroutine when the object is disabled
            patrolCoroutine = null;
        }
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            // Move towards the target patrol point
            while (transform.position != patrolPoints[targetPoint].position)
            {
                // Move the character
                transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime);

                // Calculate the direction to the target
                Vector3 direction = patrolPoints[targetPoint].position - transform.position;

                // Calculate the rotation step
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                // Set animator to walking
                animator.SetBool("Walk", true);

                yield return null; // Wait for the next frame
            }

            // Stop walking animation when reached the point
            animator.SetBool("Walk", false);

            if (broadcastPoints.Contains(targetPoint))
            {
                if (isMainCharacter) // Only the main character broadcasts
                {
                    // Get the index of the patrol point in the broadcast list
                    int broadcastIndex = broadcastPoints.IndexOf(targetPoint);
                    OnInfoPointReached?.Invoke(broadcastIndex); // Broadcast the index
                }

                // Wait at the patrol point for a specified time before moving to the next
                yield return new WaitForSeconds(waitTime);
            }

            // Increase the target point index
            IncreaseTargetInt();
        }
    }

    private void IncreaseTargetInt()
    {
        targetPoint++;
        if (targetPoint >= patrolPoints.Length)
        {
            targetPoint = 0; // Loop back to the start
        }
    }
}
