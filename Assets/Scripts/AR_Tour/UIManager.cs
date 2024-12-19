using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CharacterPatrol  character;
    [SerializeField] MonumentInfoSO monumentSO;
    [SerializeField] private TextMeshProUGUI monumentTitle;
    [SerializeField] private TextMeshProUGUI monumentInfo1;
    [SerializeField] private TextMeshProUGUI monumentInfo2;

    public RectTransform conversationPanel;

    [SerializeField] private RectTransform monumentPapyrusObject;
    [SerializeField] private Image monumentPapyrusImage;

    private Vector3 originalPapyrusScale;

    // List to hold references to your buttons
    [SerializeField] private List<Button> monumentButtons;
    [SerializeField] private List<Sprite> monumnetInfoSprite;  // papyrus scroll info

    private int currentActiveIndex = -1; // To track the currently active button index

    private void OnEnable()
    {
        originalPapyrusScale = monumentPapyrusObject?.transform.localScale ?? Vector3.one; // Null check

        // Subscribe to the event
        CharacterPatrol.OnInfoPointReached += CharacterPatrol_OnInfoPointReached;

        // Initialize buttons
        AssignButtons();
    }

    private void OnDisable()
    {
        // Unsubscribe from the event
        CharacterPatrol.OnInfoPointReached -= CharacterPatrol_OnInfoPointReached;
    }

    private void Start()
    {
        // You might want to reset the panel at start
        InfoSlideOut();
    }

    private void CharacterPatrol_OnInfoPointReached(int index)
    {
        if (monumentPapyrusImage != null && monumnetInfoSprite.Count > index)
        {
            monumentPapyrusImage.sprite = monumnetInfoSprite[index];
            StartCoroutine(StartPapyrusCycle());
        }
    }

    private void AssignButtons()
    {
        for (int i = 0; i < monumentButtons.Count; i++)
        {
            int index = i; // Capture the correct index
            monumentButtons[i].onClick.AddListener(() => OnMonumentButtonClicked(index));
        }
    }

    private void OnMonumentButtonClicked(int index)
    {
        if (currentActiveIndex == index)
        {
            // Same button clicked, slide out
            InfoSlideOut();
            currentActiveIndex = -1; // Reset the active index
        }
        else
        {
            // Different button clicked
            InfoSlideOut(); // Slide out the current information
            currentActiveIndex = index; // Update the active index

            // Use a delayed invocation to update and slide in the new information
            Invoke("SecondBtnSlideIn", 0.2f);
        }
    }

    private void SecondBtnSlideIn()
    {
        // Update and slide in the new information
        UpdateInfoDisplay(currentActiveIndex);
        InfoSlideIn();
    }

    private void UpdateInfoDisplay(int index)
    {
        if (monumentSO != null && monumentSO.monumentInformation.monumentInformation.Length > index)
        {
            monumentTitle.text = monumentSO.monumentInformation.monumentInformation[index].title;
            monumentInfo1.text = monumentSO.monumentInformation.monumentInformation[index].details[0];
            monumentInfo2.text = monumentSO.monumentInformation.monumentInformation[index].details[1];
        }
    }

    IEnumerator StartInfoCycle()
    {
        InfoSlideIn();
        yield return new WaitForSeconds(4);
        InfoSlideOut();
    }

    IEnumerator StartPapyrusCycle()
    {
        if (monumentPapyrusObject != null)
        {
            PapyrusSlideIn();
            yield return new WaitForSeconds(character.waitTimeList[character.broadcastIndex]);
            PapyrusSlideOut();
        }
    }

    public void InfoSlideIn()
    {
        conversationPanel.DOAnchorPos(new Vector3(300, 0, 0), 0.25f);
    }

    public void InfoSlideOut()
    {
        conversationPanel.DOAnchorPos(new Vector3(1800, 0, 0), 0.25f);
    }

    public void PapyrusSlideIn()
    {
        if (monumentPapyrusObject != null)
        {
            monumentPapyrusObject.DOScale(2.5f, 0.5f).SetEase(Ease.InBounce);
        }
    }

    public void PapyrusSlideOut()
    {
        if (monumentPapyrusObject != null)
        {
            monumentPapyrusObject.DOScale(0f, 0.5f).SetEase(Ease.InBounce);

        }
        AudioManager.Instance.ClipToEnd();
    }

    private void Update()
    {
        // Check for touch input to close the panel
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button (or touch)
        {
            Vector2 touchPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(conversationPanel, touchPosition, null, out Vector2 localPoint);

            // Check if the touch position is outside the panel
            if (!RectTransformUtility.RectangleContainsScreenPoint(conversationPanel, touchPosition))
            {
                InfoSlideOut(); // Slide out if touched outside
            }
        }
    }
}
