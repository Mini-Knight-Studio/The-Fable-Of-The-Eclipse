using System;
using System.Collections;
using Loopie;

public class PlayerTorch : PlayerComponent
{
    public float burnDuration = 2.0f;

    public Entity torchEntity;

    public Entity fireObject;

    private bool isTorching = false;
    public bool IsTorching => isTorching;

    void OnCreate()
    {
        if (torchEntity != null) torchEntity.SetActive(false);
        if (fireObject != null) fireObject.SetActive(false);
    }

    public void ProcessTorch()
    {
        if (Input.IsKeyPressed(KeyCode.O) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_Y)/*DatabaseRegistry.playerDB.Player.hasBurner && !isTorching*/)
        {
            StartCoroutine(TorchSequence());
        }
    }

    private IEnumerator TorchSequence()
    {
        isTorching = true;

        if (torchEntity != null) torchEntity.SetActive(true);
        if (fireObject != null) fireObject.SetActive(true);

        float timer = 0.0f;
        while (timer < burnDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (fireObject != null) fireObject.SetActive(false);
        if (torchEntity != null) torchEntity.SetActive(false);

        isTorching = false;
    }
}