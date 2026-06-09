using System;
using System.Collections;
using Loopie;

public class UIPopupManager : Component
{
    public static UIPopupManager Instance { get; private set; }

    private Entity currentActivePanel = null;
    private bool isWaitingForInput = false;

    private float fadeDuration = 0.5f;

    void OnCreate()
    {
        Instance = this;
    }

    void OnUpdate()
    {
        if (isWaitingForInput && currentActivePanel != null)
        {
            if (Player.Instance.Input.interactKeyPressed)
            {
                ClosePopup();
            }
        }
    }

    public void ShowPopup(string panelName)
    {
        Entity panel = Entity.FindEntityByName(panelName);
        if (panel != null)
        {
            panel.SetActive(true);
            currentActivePanel = panel;
            isWaitingForInput = true;

            StartCoroutine(FadeCanvasGroup(panel, 0.0f, 1.0f));

            GameManager.SetState(GameManager.GameState.PAUSE);
        }
        else
        {
            Debug.Log("UIPopupManager: No Popup was found with the name " + panelName);
        }
    }

    private void ClosePopup()
    {
        if (currentActivePanel != null)
        {
            StartCoroutine(FadeAndDisable(currentActivePanel));
            currentActivePanel = null;
        }
    }

    IEnumerator FadeCanvasGroup(Entity panel, float startAlpha, float endAlpha, bool wasWaitingForInput = false)
    {
        if (panel != null && panel.HasComponent<CanvasGroup>())
        {
            CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
            canvasGroup.Alpha = startAlpha;

            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / fadeDuration;
                canvasGroup.Alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }

            canvasGroup.Alpha = endAlpha;

        }
        if (wasWaitingForInput)
        {
            isWaitingForInput = false;
            GameManager.SetState(GameManager.GameState.DEFAULT);
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
    }

    IEnumerator FadeAndDisable(Entity panel)
    {
        yield return StartCoroutine(FadeCanvasGroup(panel, 1.0f, 0.0f, true));
    }
}