using Loopie;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class DialogUI : Component
{
    public Entity containerEntity;
    public Entity textEntity;
    public Entity nextLineEntity;
    [HideInInspector]public Text text;

    List<string> lines;
    public bool IsDialogOpen { get; private set; } = false;
    private bool isWaitingForNextLine = false;
    private float readSpeed;

    public static DialogUI Instance { get; private set; }
    void OnCreate()
    {
        if (Instance == null)
            Instance = this;
        else
            return;

        text = textEntity.GetComponent<Text>();
        containerEntity.SetActive(false);
        nextLineEntity.SetActive(false);
    }

    public void SetText(string value, string splitChar)
    {
        if (!string.IsNullOrEmpty(splitChar))
        {
            lines = value.Split(new string[] { splitChar }, StringSplitOptions.None).ToList();
        }
        else
        {
            lines = new List<string> { value };
        }
    }

    public void StartReading(float readSpeed)
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        IsDialogOpen = true;
        this.readSpeed = readSpeed;

        containerEntity.SetActive(true);
        nextLineEntity.SetActive(false);
        text.SetText("");

        StopAllOwnedCoroutines();
        StartCoroutine(Read());
    }

    public void Close()
    {
        GameManager.SetState(GameManager.GameState.DEFAULT);

        containerEntity.SetActive(false);
        nextLineEntity.SetActive(false);

        IsDialogOpen = false;
        isWaitingForNextLine = false;
    }

    IEnumerator Read()
    {

        yield return null;

        foreach (string line in lines)
        {
            text.SetText("");

            for (int i = 0; i < line.Length; i++)
            {
                text.SetText(line.Substring(0, i + 1));

                float timer = 0f;
                bool skipRequested = false;

                while (timer < readSpeed)
                {
                    if (Player.Instance.Input.interactKeyPressed)
                    {
                        text.SetText(line);
                        skipRequested = true;
                        break;
                    }

                    timer += Time.deltaTime;
                    yield return null;
                }
                if (skipRequested)
                {
                    break;
                }
            }

            while (Player.Instance.Input.interactKeyPressed)
            {
                yield return null;
            }

            StartCoroutine(NextLineIconBlink(0.4f));

            isWaitingForNextLine = true;
            while (isWaitingForNextLine)
            {
                if (Player.Instance.Input.interactKeyPressed)
                {
                    isWaitingForNextLine = false;
                }
                yield return null;
            }
            while (Player.Instance.Input.interactKeyPressed)
            {
                yield return null;
            }
        }

        Close();
    }

    IEnumerator NextLineIconBlink(float blinkSpeed)
    {
        bool iconState = true;

        while (isWaitingForNextLine)
        {
            if (nextLineEntity != null)
            {
                nextLineEntity.SetActive(iconState);
            }

            iconState = !iconState;
            float timer = 0f;

            while (timer < blinkSpeed && isWaitingForNextLine)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        if (nextLineEntity != null)
        {
            nextLineEntity.SetActive(false);
        }
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
};