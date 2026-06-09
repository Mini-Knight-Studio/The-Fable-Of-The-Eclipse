using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class CinematicUI : Component
{
    public static CinematicUI Instance { get; private set; }

    public Entity closingPageEntity;
    SpriteAnimator closingPageAnimator;

    public Entity openingPageEntity;
    SpriteAnimator openingPageAnimator;

    float fadeInDuration;
    float fadeOutDuration;

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

        closingPageAnimator = closingPageEntity.GetComponent<SpriteAnimator>();
        openingPageAnimator = openingPageEntity.GetComponent<SpriteAnimator>();
    }

    public void SetUpCinematic(Entity cinematicContainer, float fadeInTime, float fadeOutTime)
    {
        fadeOutDuration = fadeOutTime;
        fadeInDuration = fadeInTime;

        frames.Clear();
        cinematicOwner = cinematicContainer;
        if(cinematicOwner == null)
        {
            Debug.LogError("Cinematic container is null.");
            return;
        }

        for (int i = 0; i < cinematicContainer.ChildCount; i++) {
            var imageEntity = cinematicContainer.GetChildByName((i+1).ToString());

            if (imageEntity.HasComponent<CinematicFrame>())
            {
                CinematicFrame frame = imageEntity.GetComponent<CinematicFrame>();
                frames.Add(frame);
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
        Debug.Log("Start");
        
        StartCoroutine(Play());
    }

    public void Close()
    {
        cinematicOwner.SetActive(false);
        foreach (var frame in frames)
        {
            frame.FrameEntity.SetActive(false);
        }
       
        GameManager.SetState(GameManager.GameState.DEFAULT);
        IsCinematicOpen = false;

        openingPageEntity.SetActive(false);
        closingPageEntity.SetActive(false);
    }

    IEnumerator Play()
    {
        cinematicOwner.SetActive(false);
        float timer = 0;
        int index = 0;
        cinematicOwner.SetActive(true);
        yield return null;

        if (MusicStopper.Instance != null)
            MusicStopper.Instance.PauseMusic();

        while (index < frames.Count)
        {
            CinematicFrame frame = frames[index];
            frame.FrameEntity.SetActive(true);
            if (frame.FadeTextGroup && frame.TextGroupCanvasGroup != null){
                frame.TextGroupCanvasGroup.Alpha = 0;
            }

            if (frame.FadeInText && frame.TextCanvasGroup != null)
            {
                frame.TextCanvasGroup.Alpha = 0;
            }

            if (index == 0)
            {
                timer = 0;
                while (timer < fadeInDuration)
                {
                    if(frame.FrameCanvasGroup != null)
                    {
                        frame.FrameCanvasGroup.Alpha = Mathf.Lerp(0, 1, timer / fadeInDuration);
                    }
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            else if(frame.UsePage)
            {
                if (frame.HasAnimation)
                {
                    frame.FrameAnimator.Play();
                }

                closingPageEntity.SetActive(false);
                openingPageEntity.SetActive(true);
                openingPageAnimator.Play();
                yield return new WaitForUnscaledSeconds(0.5f);
                openingPageEntity.SetActive(false);
            }

            

            if (frame.FadeTextGroup && frame.TextGroupCanvasGroup != null)
            {
                frame.TextGroupCanvasGroup.Alpha = 0;
                yield return new WaitForUnscaledSeconds(0.5f);
                timer = 0;
                while (timer < fadeInDuration/2)
                {
                    frame.TextGroupCanvasGroup.Alpha = Mathf.Lerp(0, 1, timer / (fadeInDuration/2));
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            if (frame.FadeInText && frame.TextCanvasGroup != null)
            {
                timer = 0;
                while (timer < fadeInDuration / 2)
                {
                    frame.TextCanvasGroup.Alpha = Mathf.Lerp(0, 1, timer / (fadeInDuration / 2));
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            if (frame.AudioSource != null)
                frame.AudioSource.Play();

            

            if (!frame.WaitsForInput)
            {
                yield return new WaitForUnscaledSeconds(frame.Duration);
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

            if (frame.FadeOutText && frame.TextCanvasGroup != null)
            {
                timer = 0;
                while (timer < fadeOutDuration / 2)
                {
                    frame.TextCanvasGroup.Alpha = Mathf.Lerp(1, 0, timer / (fadeOutDuration / 2));
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            if (index == frames.Count - 1)
            {
                timer = 0;
                while (timer < fadeOutDuration)
                {
                    if (frame.FrameCanvasGroup != null)
                    {
                        frame.FrameCanvasGroup.Alpha = Mathf.Lerp(1, 0, timer / fadeOutDuration);
                    }
                    timer += Time.unscaledDeltaTime;
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
            }
            else if(index + 1 < frames.Count)
            {
                CinematicFrame nextFrame = frames[index+1];
                if (nextFrame.UsePage)
                {
                    closingPageEntity.SetActive(true);
                    closingPageAnimator.Play();
                    yield return new WaitForUnscaledSeconds(0.5f);
                    

                }     
            }

                yield return null;
            frame.FrameEntity.SetActive(false);


            index++;
        }

        Close();

        if (MusicStopper.Instance != null)
            MusicStopper.Instance.ResumeMusic();
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}

