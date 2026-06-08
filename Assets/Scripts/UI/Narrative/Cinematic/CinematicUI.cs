using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class CinematicUI : Component
{
    private struct CinematicFrame
    {
        public Image image;
        public AudioSource audioSource;
        public float displayDuration;
    }
    public static CinematicUI Instance { get; private set; }

    public Entity topBar;
    RectTransform topBarRect;
    public Entity bottomBar;
    RectTransform bottomBarRect;
    public float barAnimationDuration = 0.5f;
    public float barDelay = 0.5f;
    public float fadeInOutDuration = 0.5f;

    public bool IsCinematicOpen { get; private set; } = false;
    private List<CinematicFrame> frames = new List<CinematicFrame>();
    private Entity cinematicOwner;
    void OnCreate()
    {
        if (Instance == null)
            Instance = this;
        else
            return;

        frames = new List<CinematicFrame>();

        topBarRect = topBar.GetComponent<RectTransform>();
        bottomBarRect = bottomBar.GetComponent<RectTransform>();

    }

    public void SetUpCinematic(Entity cinematicContainer)
    {
        frames.Clear();
        cinematicOwner = cinematicContainer;
        if(cinematicOwner == null)
        {
            Debug.LogError("Cinematic container is null.");
            return;
        }
        foreach (var imageEntity in cinematicContainer.GetChildren())
        {
            var image = imageEntity.GetComponent<Image>();
            var audioSource = imageEntity.GetComponent<AudioSource>();
            if (image != null)
            {
                string name = imageEntity.Name;
                float displayDuration = -1;
                if (float.TryParse(name, out float duration))
                {
                    displayDuration = duration;
                }

                frames.Add(new CinematicFrame { image = image, displayDuration = displayDuration, audioSource = audioSource });
            }
            imageEntity.SetActive(false);
        }
    }

    public void StartCinematic()
    {
        if(frames.Count == 0)
        {
            Debug.LogWarning("No frames found for cinematic.");
            return;
        }
        IsCinematicOpen = true;
        GameManager.SetState(GameManager.GameState.PAUSE);
        StartCoroutine(Play());
    }

    public void Close()
    {
        cinematicOwner.SetActive(false);
        foreach (var frame in frames)
        {
            frame.image.entity.SetActive(false);
        }
        GameManager.SetState(GameManager.GameState.DEFAULT);
        IsCinematicOpen = false;

        topBar.SetActive(false);
        bottomBar.SetActive(false);
    }

    IEnumerator Play()
    {
        cinematicOwner.SetActive(false);
        float timer = 0;
        float sizeBar = topBarRect.size.y;

        topBar.SetActive(true);
        bottomBar.SetActive(true);
        while (timer < barAnimationDuration)
        {
            float t = timer / barAnimationDuration;
            topBarRect.anchored_position = Vector2.Lerp(new Vector2(0, sizeBar), new Vector2(0, 0), t);
            bottomBarRect.anchored_position = Vector2.Lerp(new Vector2(0, -sizeBar), new Vector2(0, 0), t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForUnscaledSeconds(barDelay);

        int index = 0;
        cinematicOwner.SetActive(true);
        while (index < frames.Count)
        {
            CinematicFrame frame = frames[index];
            frame.image.entity.SetActive(true);

            if(index == 0)
            {
                Vector4 color = frame.image.Tint;
                timer = 0;
                while (timer < fadeInOutDuration)
                {
                    float t = timer / fadeInOutDuration;
                    frame.image.Tint = new Vector4(color.x, color.y, color.z, Mathf.Lerp(0, 255, t));
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            frame.audioSource?.Play();
            if (frame.displayDuration != -1)
            {
                yield return new WaitForUnscaledSeconds(frame.displayDuration);
            }
            else
            {
                while (Input.Any)
                {
                    yield return null;
                }

                while (!Input.Any)
                {
                    yield return null;
                }
            }

            if (index == frames.Count - 1)
            {
                Vector4 color = frame.image.Tint;
                timer = 0;
                while (timer < fadeInOutDuration)
                {
                    float t = timer / fadeInOutDuration;
                    frame.image.Tint = new Vector4(color.x, color.y, color.z, Mathf.Lerp(255, 0, t));
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            yield return null;
            frame.image.entity.SetActive(false);
            index++;
        }

        timer = 0;
        while (timer < barAnimationDuration)
        {
            float t = timer / barAnimationDuration;
            topBarRect.anchored_position = Vector2.Lerp(new Vector2(0, 0), new Vector2(0, sizeBar), t);
            bottomBarRect.anchored_position = Vector2.Lerp(new Vector2(0, 0), new Vector2(0, -sizeBar), t);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        yield return new WaitForUnscaledSeconds(barDelay);

        Close();
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}

