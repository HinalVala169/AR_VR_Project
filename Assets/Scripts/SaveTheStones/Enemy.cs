using UnityEngine;
using RehtseStudio.SimpleWaveSystem.Managers;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private Animator _animator; // Reference to the Animator for playing animations

    private NavMeshAgent enemyNavMesh;

    private void Start()
    {
        _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        enemyNavMesh = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that triggered the collider has the tag "PlayerWeapon"
        if (other.CompareTag("PlayerWeapon"))
        {

            enemyNavMesh.enabled = false;

            // Play dead animation
            PlayDeadAnimation();

            // Call the method in the SpawnManager
            _spawnManager.ObjectWaveCheck();

            

        }
    }

    private void PlayDeadAnimation()
    {
        // Assuming you have a trigger parameter named "Die" in your Animator
        _animator.SetTrigger("Die");

        Invoke("OnDead", 0.5f);
    }

    private void OnDead()
    {
        this.gameObject.SetActive(false);
    }
}
