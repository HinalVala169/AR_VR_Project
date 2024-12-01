using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InstructionDisplay : MonoBehaviour
{
    public Image imageComponent; // Assign your Image component in the Inspector
    public Sprite[] sprites;      // Assign your sprites in the Inspector
    public float displayDuration = 2f; // Time each sprite is displayed

    private int currentSpriteIndex = 0;

    private void Start()
    {
        if (sprites.Length > 0)
        {
            StartCoroutine(DisplaySprites());
        }
    }

    private IEnumerator DisplaySprites()
    {
        while (currentSpriteIndex < sprites.Length)
        {
            imageComponent.sprite = sprites[currentSpriteIndex];
            yield return new WaitForSeconds(displayDuration);

            // Move to the next sprite
            currentSpriteIndex++;
        }
    }
}
