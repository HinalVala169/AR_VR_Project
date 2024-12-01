using System.Collections;
using UnityEngine;

public class MainSceneCycler : MonoBehaviour
{
    [System.Serializable]
    public class GameObjectDuration
    {
        public GameObject gameObject;
        public float duration; // Time to enable the GameObject
    }

    public GameObjectDuration[] gameObjects; // Array of GameObject and duration pairs
    public float defaultDuration = 2f; // Default duration if not specified

    [SerializeField] private GameObject mainScene;


    

    public void StartCycling()
    {

        mainScene.SetActive(false);
        StartCoroutine(CycleThroughGameObjects());

    }

    private IEnumerator CycleThroughGameObjects()
    {
        foreach (var objDuration in gameObjects)
        {
            // Enable the GameObject
            objDuration.gameObject.SetActive(true);

            // Wait for the specified duration or default duration
            yield return new WaitForSeconds(objDuration.duration > 0 ? objDuration.duration : defaultDuration);

            // Disable the GameObject
            objDuration.gameObject.SetActive(false);
        }
    }
}
