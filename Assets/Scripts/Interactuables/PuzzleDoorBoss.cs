using System;
using System.Collections.Generic;
using System.Collections;
using Loopie;

class PuzzleDoorBoss : Component
{
    [Header("Identity")]
    public string puzzleDoorID = "UNASSIGNED_PUZZLEDOOR";
    public string requiredKeyID1 = "UNASSIGNED_KEY_1";
    public string requiredKeyID2 = "UNASSIGNED_KEY_2";
    public string requiredKeyID3 = "UNASSIGNED_KEY_3";

    private string[] requiredKeyIDs;

    [Header("References")]
    public Entity rightDoor;
    public Entity leftDoor;
    public Entity staticKey1;
    public Entity staticKey2;
    public Entity staticKey3;
    public Entity animatedKey1;
    public Entity animatedKey2;
    public Entity animatedKey3;

    private Entity[] staticKeys;
    private Entity[] animatedKeys;
    private bool[] keyInserted;

    public Entity focusPointOnInsert;
    public Entity blockingCollider;
    public Entity interactPrompt;

    private InteractHover interactPromptComponent;

    [Header("Settings")]
    public Vector3 finalRightDoorRot = Vector3.Zero;
    public Vector3 finalLefttDoorRot = Vector3.Zero;
    public float camFocusDuration = 1.0f;
    public float keyTravelDuration = 1.0f;
    public float doorOpenDuration = 2.0f;
    public float pauseBeforeOpening = 0.5f;
    public float easeIntensity = 1.5f;
    public float cameraZoom = 20;

    public float cameraShakeDuration = 0.5f;
    public float cameraShakeAmount = 0.3f;
    public float cameraShakeRotation = 0.3f;
    public float cameraShakeAmountVel = 0.3f;
    public float cameraShakeRotationVel = 0.3f;

    private bool isOpening = false;
    private bool hasOpened = false;

    private Vector3 initialRightDoorRot;
    private Vector3 initialLeftDoorRot;
    private Vector3 finalKeyScale;

    [Header("Feedback")]
    public Entity rightParticle;
    public Entity leftParticle;

    public Entity keyParticles1;
    public Entity keyParticles2;
    public Entity keyParticles3;

    private Entity[] keyParticles;

    public Entity bossEyesParticles;

    public Entity door1SFX;
    public Entity door2SFX;
    public Entity door3SFX;
    public Entity doorSongSFX;
    public Entity impactSFX;
    public Entity bellSFX;
    public Entity insertKey1SFX;
    public Entity insertKey2SFX;
    public Entity insertKey3SFX;
    public Entity eyesIgniteSFX;

    private Entity[] insertKeySFXs;

    void OnCreate()
    {
        requiredKeyIDs = new string[] { requiredKeyID1, requiredKeyID2, requiredKeyID3 };
        staticKeys = new Entity[] { staticKey1, staticKey2, staticKey3 };
        animatedKeys = new Entity[] { animatedKey1, animatedKey2, animatedKey3 };
        keyParticles = new Entity[] { keyParticles1, keyParticles2, keyParticles3 };
        insertKeySFXs = new Entity[] { insertKey1SFX, insertKey2SFX, insertKey3SFX };
        keyInserted = new bool[3];

        if (animatedKeys[0] != null)
        {
            finalKeyScale = animatedKeys[0].transform.scale;
        }

        for (int i = 0; i < 3; i++)
        {
            if (staticKeys[i] != null) staticKeys[i].SetActive(false);
            if (animatedKeys[i] != null) animatedKeys[i].SetActive(false);
            if (keyParticles[i] != null) keyParticles[i].GetComponent<ParticleComponent>().Stop();

            if (DatabaseRegistry.levelsDB.Levels.IsBossDoorKeyInserted(i))
            {
                staticKeys[i].SetActive(true);
                keyInserted[i] = true;
                switch (i)
                {
                    case 0:
                        {
                            bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 5);
                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 5);

                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(0, new Vector4(0.105f, 0.361f, 0.322f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(1, new Vector4(0.105f, 0.361f, 0.322f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(0, new Vector4(0.384f, 0.976f, 0.875f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(1, new Vector4(0.384f, 0.976f, 0.875f, 1.0f));
                            break;
                        }

                    case 1:
                        {
                            bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 10);
                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 10);

                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(0, new Vector4(0.105f, 0.176f, 0.361f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(1, new Vector4(0.105f, 0.176f, 0.361f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(0, new Vector4(0.384f, 0.627f, 0.976f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(1, new Vector4(0.384f, 0.627f, 0.976f, 1.0f));
                            break;
                        }

                    case 2:
                        {
                            bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 30);
                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 30);

                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(0, new Vector4(0.361f, 0.105f, 0.105f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(1, new Vector4(0.361f, 0.105f, 0.105f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(0, new Vector4(0.976f, 0.384f, 0.384f, 1.0f));
                            bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(1, new Vector4(0.976f, 0.384f, 0.384f, 1.0f));
                            break;
                        }

                    default:
                        {
                            bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 10);
                            bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 10);
                            break;
                        }
                }
                bossEyesParticles.GetComponent<ParticleComponent>().Play();
            }
        }

        initialRightDoorRot = rightDoor.transform.local_rotation;
        initialLeftDoorRot = leftDoor.transform.local_rotation;

        if (rightParticle != null) rightParticle.GetComponent<ParticleComponent>().Stop();
        if (leftParticle != null) leftParticle.GetComponent<ParticleComponent>().Stop();

        if (door1SFX != null) door1SFX.GetComponent<AudioSource>().Stop();
        if (door2SFX != null) door2SFX.GetComponent<AudioSource>().Stop();
        if (door3SFX != null) door3SFX.GetComponent<AudioSource>().Stop();

        if (impactSFX != null) impactSFX.GetComponent<AudioSource>().Stop();

        if (DatabaseRegistry.levelsDB.Levels.IsPuzzleDoorOpened(puzzleDoorID))
        {
            hasOpened = true;
            DoorOpened();
        }
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }
        if (hasOpened || isOpening) return;

        List<int> keysReadyToInsert = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            if (!keyInserted[i] && DatabaseRegistry.levelsDB.Levels.IsRewardCollected(requiredKeyIDs[i]))
            {
                keysReadyToInsert.Add(i);
            }
        }

        if (entity.GetComponent<BoxCollider>().IsColliding && keysReadyToInsert.Count > 0)
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed)
            {
                isOpening = true;
                StartCoroutine(InsertKeysAndOpen(keysReadyToInsert));
                interactPrompt.SetActive(false);
            }
        }
        else
        {
            if (interactPrompt.Active)
            {
                interactPrompt.SetActive(false);
            }
        }
    }

    IEnumerator InsertKeysAndOpen(List<int> keysToInsert)
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        Player.Instance.Camera.FocusOnPoint(focusPointOnInsert.transform.position, cameraZoom, 4);
        yield return new WaitForSeconds(camFocusDuration);

        foreach (int keyIndex in keysToInsert)
        {
            Entity currentAnimKey = animatedKeys[keyIndex];
            Entity currentStaticKey = staticKeys[keyIndex];
            Entity currentParticle = keyParticles[keyIndex];
            Entity currentInsertSFX = insertKeySFXs[keyIndex];

            currentAnimKey.SetActive(true);
            currentAnimKey.transform.position = Player.Instance.transform.position + new Vector3(0, 2, 0);
            currentAnimKey.transform.scale = Vector3.Zero;

            Vector3 startKeyPos = currentAnimKey.transform.position;
            Vector3 targetKeyPos = currentStaticKey.transform.position;

            Vector3 startKeyScale = Vector3.Zero;
            Vector3 targetKeyScale = finalKeyScale;

            float elapsedTime = 0f;

            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / keyTravelDuration;
                float curvedT = Mathf.Pow(t, easeIntensity);

                currentAnimKey.transform.position = Vector3.Lerp(startKeyPos, targetKeyPos, curvedT);
                currentAnimKey.transform.scale = Vector3.Lerp(startKeyScale, targetKeyScale, curvedT);

                if (t >= 0.85f && currentParticle != null && !currentParticle.GetComponent<ParticleComponent>().IsPlaying)
                {
                    currentParticle.GetComponent<ParticleComponent>().Play();
                    if (currentInsertSFX != null) currentInsertSFX.GetComponent<AudioSource>().Play();
                }
                if (t >= 1f)
                {
                    currentAnimKey.transform.position = targetKeyPos;
                    currentAnimKey.transform.scale = targetKeyScale;

                    switch (keyIndex)
                    {
                        case 0:
                            {
                                bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 5);
                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 5);

                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(0, new Vector4(0.105f, 0.361f, 0.322f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(1, new Vector4(0.105f, 0.361f, 0.322f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(0, new Vector4(0.384f, 0.976f, 0.875f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(1, new Vector4(0.384f, 0.976f, 0.875f, 1.0f));
                                break;
                            }

                        case 1:
                            {
                                bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 10);
                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 10);

                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(0, new Vector4(0.105f, 0.176f, 0.361f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(1, new Vector4(0.105f, 0.176f, 0.361f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(0, new Vector4(0.384f, 0.627f, 0.976f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(1, new Vector4(0.384f, 0.627f, 0.976f, 1.0f));
                                break;
                            }

                        case 2:
                            {
                                bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 30);
                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 30);

                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(0, new Vector4(0.361f, 0.105f, 0.105f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorBegin(1, new Vector4(0.361f, 0.105f, 0.105f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(0, new Vector4(0.976f, 0.384f, 0.384f, 1.0f));
                                bossEyesParticles.GetComponent<ParticleComponent>().SetColorEnd(1, new Vector4(0.976f, 0.384f, 0.384f, 1.0f));
                                break;
                            }

                        default:
                            {
                                bossEyesParticles.GetComponent<ParticleComponent>().Stop();

                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(0, 10);
                                bossEyesParticles.GetComponent<ParticleComponent>().SetSpawnRate(1, 10);
                                break;
                            }
                    }
                    bossEyesParticles.GetComponent<ParticleComponent>().Play();
                    if (eyesIgniteSFX != null) eyesIgniteSFX.GetComponent<AudioSource>().Play();

                    break;
                }
                yield return null;
            }

            currentAnimKey.SetActive(false);
            currentStaticKey.SetActive(true);
            keyInserted[keyIndex] = true;
            DatabaseRegistry.levelsDB.Levels.SetBossDoorKeyInserted(keyIndex);

            yield return new WaitForSeconds(pauseBeforeOpening * 0.5f);

            if (currentParticle != null) currentParticle.GetComponent<ParticleComponent>().Stop();

            yield return new WaitForSeconds(pauseBeforeOpening * 0.5f);
        }

        bool allKeysInserted = keyInserted[0] && keyInserted[1] && keyInserted[2];

        if (allKeysInserted)
        {
            if (door1SFX != null) door1SFX.GetComponent<AudioSource>().Play();
            if (door2SFX != null) door2SFX.GetComponent<AudioSource>().Play();
            if (door3SFX != null) door3SFX.GetComponent<AudioSource>().Play();
            if (doorSongSFX != null) doorSongSFX.GetComponent<AudioSource>().Play();

            float elapsedTime = 0f;

            Player.Instance.Camera.SetIsShaking(true, doorOpenDuration * 2f, cameraShakeAmount, cameraShakeRotation, cameraShakeAmountVel, cameraShakeRotationVel);

            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / doorOpenDuration;
                float curvedT = Mathf.Pow(t, easeIntensity);

                rightDoor.transform.local_rotation = Vector3.Lerp(initialRightDoorRot, finalRightDoorRot, curvedT);
                leftDoor.transform.local_rotation = Vector3.Lerp(initialLeftDoorRot, finalLefttDoorRot, curvedT);

                if (t >= 0.1f && rightParticle != null && !rightParticle.GetComponent<ParticleComponent>().IsPlaying)
                {
                    if (rightParticle != null) rightParticle.GetComponent<ParticleComponent>().Play();
                    if (leftParticle != null) leftParticle.GetComponent<ParticleComponent>().Play();
                }

                if (t >= 1f)
                {
                    rightDoor.transform.local_rotation = finalRightDoorRot;
                    leftDoor.transform.local_rotation = finalLefttDoorRot;

                    if (door1SFX != null) door1SFX.GetComponent<AudioSource>().Stop();
                    if (door2SFX != null) door2SFX.GetComponent<AudioSource>().Stop();
                    if (door3SFX != null) door3SFX.GetComponent<AudioSource>().Stop();
                    if (impactSFX != null) impactSFX.GetComponent<AudioSource>().Play();
                    if (bellSFX != null) bellSFX.GetComponent<AudioSource>().Play();

                    Player.Instance.Camera.SetIsShaking(true, cameraShakeDuration, cameraShakeAmount * 2, cameraShakeRotation * 2, cameraShakeAmountVel, cameraShakeRotationVel);

                    break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(pauseBeforeOpening);

            Player.Instance.Camera.StopFocus();

            if (rightParticle != null) rightParticle.GetComponent<ParticleComponent>().Stop();
            if (leftParticle != null) leftParticle.GetComponent<ParticleComponent>().Stop();

            if (blockingCollider != null) blockingCollider.SetActive(false);

            hasOpened = true;
            DatabaseRegistry.levelsDB.Levels.SetPuzzleDoorOpened(puzzleDoorID);
        }
        else
        {
            Player.Instance.Camera.StopFocus();
        }

        isOpening = false;
        GameManager.SetState(GameManager.GameState.DEFAULT);
    }

    void DoorOpened()
    {
        for (int i = 0; i < 3; i++)
        {
            if (staticKeys[i] != null) staticKeys[i].SetActive(true);
            if (animatedKeys[i] != null) animatedKeys[i].SetActive(false);
            keyInserted[i] = true;
        }

        rightDoor.transform.local_rotation = finalRightDoorRot;
        leftDoor.transform.local_rotation = finalLefttDoorRot;

        if (blockingCollider != null) blockingCollider.SetActive(false);
        if (interactPrompt != null) interactPrompt.SetActive(false);
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}