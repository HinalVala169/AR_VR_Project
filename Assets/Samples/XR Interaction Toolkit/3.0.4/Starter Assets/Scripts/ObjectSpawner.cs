using System;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using UnityEngine;

namespace UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets
{
    /// <summary>
    /// Behavior with an API for spawning objects from a given set of prefabs.
    /// </summary>
    public class ObjectSpawner : MonoBehaviour
    {
        
        


        [SerializeField]
        [Tooltip("The camera that objects will face when spawned. If not set, defaults to the main camera.")]
        Camera m_CameraToFace;

        public Camera cameraToFace
        {
            get
            {
                EnsureFacingCamera();
                return m_CameraToFace;
            }
            set => m_CameraToFace = value;
        }

        [SerializeField]
        [Tooltip("The list of prefabs available to spawn.")]
        List<GameObject> m_ObjectPrefabs = new List<GameObject>();

        public List<GameObject> objectPrefabs
        {
            get => m_ObjectPrefabs;
            set => m_ObjectPrefabs = value;
        }

        [SerializeField]
        [Tooltip("Optional prefab to spawn for each spawned object. Use a prefab with the Destroy Self component to make " +
            "sure the visualization only lives temporarily.")]
        GameObject m_SpawnVisualizationPrefab;

        public GameObject spawnVisualizationPrefab
        {
            get => m_SpawnVisualizationPrefab;
            set => m_SpawnVisualizationPrefab = value;
        }

        [SerializeField]
        [Tooltip("The index of the prefab to spawn. If outside the range of the list, this behavior will select " +
            "a random object each time it spawns.")]
        int m_SpawnOptionIndex = -1;

        public int spawnOptionIndex
        {
            get => m_SpawnOptionIndex;
            set => m_SpawnOptionIndex = value;
        }

        public bool isSpawnOptionRandomized => m_SpawnOptionIndex < 0 || m_SpawnOptionIndex >= m_ObjectPrefabs.Count;

        [SerializeField]
        [Tooltip("Whether to only spawn an object if the spawn point is within view of the camera.")]
        bool m_OnlySpawnInView = true;

        public bool onlySpawnInView
        {
            get => m_OnlySpawnInView;
            set => m_OnlySpawnInView = value;
        }

        [SerializeField]
        [Tooltip("The size, in viewport units, of the periphery inside the viewport that will not be considered in view.")]
        float m_ViewportPeriphery = 0.15f;

        public float viewportPeriphery
        {
            get => m_ViewportPeriphery;
            set => m_ViewportPeriphery = value;
        }


        [SerializeField]
        [Tooltip("When enabled, the object will be rotated about the y-axis when spawned by Spawn Angle Range, " +
            "in relation to the direction of the spawn point to the camera.")]
        bool m_ApplyRandomAngleAtSpawn = true;

        public bool applyRandomAngleAtSpawn
        {
            get => m_ApplyRandomAngleAtSpawn;
            set => m_ApplyRandomAngleAtSpawn = value;
        }

        [SerializeField]
        [Tooltip("The range in degrees that the object will randomly be rotated about the y axis when spawned, " +
            "in relation to the direction of the spawn point to the camera.")]
        float m_SpawnAngleRange = 45f;

        public float spawnAngleRange
        {
            get => m_SpawnAngleRange;
            set => m_SpawnAngleRange = value;
        }

        [SerializeField]
        [Tooltip("Whether to spawn each object as a child of this object.")]
        bool m_SpawnAsChildren;

        public bool spawnAsChildren
        {
            get => m_SpawnAsChildren;
            set => m_SpawnAsChildren = value;
        }

        [SerializeField] private bool m_HasSpawned = false; // Track if an object has been spawned

        /// <summary>
        /// Event invoked after an object is spawned.
        /// </summary>
        public event Action<GameObject> objectSpawned;

        void Awake()
        {
            EnsureFacingCamera();
        }

        void EnsureFacingCamera()
        {
            if (m_CameraToFace == null)
                m_CameraToFace = Camera.main;
        }

        public void RandomizeSpawnOption()
        {
            m_SpawnOptionIndex = -1;
        }

        /// <summary>
        /// Attempts to spawn an object from <see cref="objectPrefabs"/> at the given position only if it hasn't been spawned yet.
        /// </summary>
        /// <param name="spawnPoint">The world space position at which to spawn the object.</param>
        /// <param name="spawnNormal">The world space normal of the spawn surface.</param>
        /// <returns>Returns <see langword="true"/> if the spawner successfully spawned an object. Otherwise returns <see langword="false"/>.</returns>
        public bool TrySpawnObjectOnce(Vector3 spawnPoint, Vector3 spawnNormal)
        {
            if (m_HasSpawned)
            {
                return false; // Prevents further spawns if already spawned
            }

            bool success = TrySpawnObject(spawnPoint, spawnNormal);
            if (success)
            {
                m_HasSpawned = true; // Mark as spawned
            }

            return success;
        }

        public bool TrySpawnObject(Vector3 spawnPoint, Vector3 spawnNormal)
        {
            if (m_HasSpawned == false)
            {
                if (m_OnlySpawnInView)
                {
                    var inViewMin = m_ViewportPeriphery;
                    var inViewMax = 1f - m_ViewportPeriphery;
                    var pointInViewportSpace = cameraToFace.WorldToViewportPoint(spawnPoint);
                    if (pointInViewportSpace.z < 0f || pointInViewportSpace.x > inViewMax || pointInViewportSpace.x < inViewMin ||
                        pointInViewportSpace.y > inViewMax || pointInViewportSpace.y < inViewMin)
                    {
                        return false;
                    }
                }

                var objectIndex = isSpawnOptionRandomized ? UnityEngine.Random.Range(0, m_ObjectPrefabs.Count) : m_SpawnOptionIndex;
                var newObject = Instantiate(m_ObjectPrefabs[objectIndex]);
                if (m_SpawnAsChildren)
                    newObject.transform.parent = transform;

                newObject.transform.position = spawnPoint;
                EnsureFacingCamera();

                var facePosition = m_CameraToFace.transform.position;
                var forward = facePosition - spawnPoint;
                BurstMathUtility.ProjectOnPlane(forward, spawnNormal, out var projectedForward);
                newObject.transform.rotation = Quaternion.LookRotation(projectedForward, spawnNormal);

                if (m_ApplyRandomAngleAtSpawn)
                {
                    var randomRotation = UnityEngine.Random.Range(-m_SpawnAngleRange, m_SpawnAngleRange);
                    newObject.transform.Rotate(Vector3.up, randomRotation);
                }

                if (m_SpawnVisualizationPrefab != null)
                {
                    var visualizationTrans = Instantiate(m_SpawnVisualizationPrefab).transform;
                    visualizationTrans.position = spawnPoint;
                    visualizationTrans.rotation = newObject.transform.rotation;
                }

                objectSpawned?.Invoke(newObject);
                m_HasSpawned = true; // Mark as spawned
                return true;
            }
            return true;


        }


        /// <summary>
        /// ////////////ADDITIOn
        /// </summary>
        
        

        public void HasDespawned() //it's a stement not a question
        {
            m_HasSpawned = false;
        }
    }
}
