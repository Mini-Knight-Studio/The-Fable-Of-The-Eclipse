using System;
using System.Collections;
using Loopie;

class WaterpathSequence : Component
{
    public bool playOnlyOnce = true;

    [Header("Title Text")]
    public string textValue = "";
    public float textDurationPercent = 0f;
    public float textStartingPercent = 0f;

    private float textDuration = 0f;
    private float textStarting = 0f;

    [Header("References")]
    public Entity prepFocusTarget;

    [Header("Settings")]
    public int cameraStartingFarPlane = 1000;
    public int cameraEndingFarPlane = 300;
    public float zoom = 111f;
    public float cameraSpeed = 4f;
    public float duration = 2.5f;
    public float sequenceDistance = 0f;
    public float cameraReturnTime = 0f;

    //[Header("Feedback")]

    private bool hasTriggered = false;
    public static bool SequenceFinished = false;

    void OnCreate()
    {
        textStarting = duration * textStartingPercent;
        textDuration = duration * textDurationPercent;
}

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        if (hasTriggered) return;

        if (entity.GetComponent<BoxCollider>().IsColliding)
        {
            hasTriggered = true;
            StartCoroutine(PlayWaterpathSequence());
        }
    }

    IEnumerator PlayWaterpathSequence()
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        float originalDistance = Player.Instance.Camera.distance;
        Player.Instance.Camera.distance = sequenceDistance;

        Player.Instance.Camera.SetFarPlane(cameraStartingFarPlane);

        if (prepFocusTarget != null)
        {
            Player.Instance.Camera.FocusOnHeightPoint(prepFocusTarget.transform.position, zoom, cameraSpeed);
        }

        yield return new WaitForSeconds(textStarting);

        SimpleTextUI.Instance.Open();
        SimpleTextUI.Instance.SetText(textValue);

        yield return new WaitForSeconds(textDuration);

        SimpleTextUI.Instance.Close();

        float remainingTime = duration - textStarting - textDuration;
        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        Player.Instance.Camera.StopFocus();
        Player.Instance.Camera.distance = originalDistance;

        yield return null;
        VolcanoSequence.SequenceFinished = true;
        GameManager.SetState(GameManager.GameState.DEFAULT);

        yield return new WaitForSeconds(cameraReturnTime);
        Player.Instance.Camera.SetFarPlane(cameraEndingFarPlane);

        if (!playOnlyOnce)
        {
            yield return new WaitForSeconds(2.0f);
            hasTriggered = false;
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}