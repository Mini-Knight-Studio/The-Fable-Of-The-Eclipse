using System;
using System.Collections.Generic;
using System.Collections;
using Loopie;

class PuzzleDoorBoss : Component
{
    [Header("Identity")]
    public string puzzleDoorID = "UNASSIGNED_PUZZLEDOOR";

    [Header("References")]
    public Entity rightDoor;
    public Entity leftDoor;
    public Entity staticGem1;
    public Entity staticGem2;
    public Entity staticGem3;
    public Entity animatedGem1;
    public Entity animatedGem2;
    public Entity animatedGem3;

    private Entity[] staticGems;
    private Entity[] animatedGems;
    private bool[] gemInserted;

    public Entity focusPointOnInsert;
    public Entity blockingCollider;
    public Entity interactPrompt;

    [Header("Settings")]
    public Vector3 finalRightDoorRot = Vector3.Zero;
    public Vector3 finalLefttDoorRot = Vector3.Zero;
    public float camFocusDuration = 1.0f;
    public float gemTravelDuration = 1.0f;
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
    private Vector3 finalGemScale;

    [Header("Feedback")]
    public Entity rightParticle;
    public Entity leftParticle;

    public Entity gemParticles1;
    public Entity gemParticles2;
    public Entity gemParticles3;

    private Entity[] gemParticles;

    public Entity bossEyesParticles;

    public Entity door1SFX;
    public Entity door2SFX;
    public Entity door3SFX;
    public Entity doorSongSFX;
    public Entity impactSFX;
    public Entity bellSFX;
    public Entity insertGem1SFX;
    public Entity insertGem2SFX;
    public Entity insertGem3SFX;
    public Entity eyesIgniteSFX;

    private Entity[] insertGemSFXs;

    void OnCreate()
    {
        staticGems = new Entity[] { staticGem1, staticGem2, staticGem3 };
        animatedGems = new Entity[] { animatedGem1, animatedGem2, animatedGem3 };
        gemParticles = new Entity[] { gemParticles1, gemParticles2, gemParticles3 };
        insertGemSFXs = new Entity[] { insertGem1SFX, insertGem2SFX, insertGem3SFX };
        gemInserted = new bool[3];

        if (animatedGems[0] != null)
        {
            finalGemScale = animatedGems[0].transform.scale;
        }

        for (int i = 0; i < 3; i++)
        {
            if (staticGems[i] != null) staticGems[i].SetActive(false);
            if (animatedGems[i] != null) animatedGems[i].SetActive(false);
            if (gemParticles[i] != null) gemParticles[i].GetComponent<ParticleComponent>().Stop();

            if (DatabaseRegistry.levelsDB.Levels.IsBossDoorGemInserted(i))
            {
                staticGems[i].SetActive(true);
                gemInserted[i] = true;
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

        List<int> gemsReadyToInsert = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            bool isCollected = false;
            if (i == 0) isCollected = DatabaseRegistry.playerDB.Player.gemAirCollected;
            else if (i == 1) isCollected = DatabaseRegistry.playerDB.Player.gemWaterCollected;
            else if (i == 2) isCollected = DatabaseRegistry.playerDB.Player.gemFireCollected;

            if (!gemInserted[i] && isCollected)
            {
                gemsReadyToInsert.Add(i);
            }
        }

        if (entity.GetComponent<BoxCollider>().IsColliding && gemsReadyToInsert.Count > 0)
        {
            if (!interactPrompt.Active)
            {
                interactPrompt.SetActive(true);
            }

            if (Player.Instance.Input.interactKeyPressed)
            {
                isOpening = true;
                StartCoroutine(InsertGemsAndOpen(gemsReadyToInsert));
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

    IEnumerator InsertGemsAndOpen(List<int> gemsToInsert)
    {
        GameManager.SetState(GameManager.GameState.PAUSE);

        Player.Instance.Camera.FocusOnPoint(focusPointOnInsert.transform.position, cameraZoom, 4);
        yield return new WaitForSeconds(camFocusDuration);

        foreach (int gemIndex in gemsToInsert)
        {
            Entity currentAnimGem = animatedGems[gemIndex];
            Entity currentStaticGem = staticGems[gemIndex];
            Entity currentParticle = gemParticles[gemIndex];
            Entity currentInsertSFX = insertGemSFXs[gemIndex];

            currentAnimGem.SetActive(true);
            currentAnimGem.transform.position = Player.Instance.transform.position + new Vector3(0, 2, 0);
            currentAnimGem.transform.scale = Vector3.Zero;

            Vector3 startGemPos = currentAnimGem.transform.position;
            Vector3 targetGemPos = currentStaticGem.transform.position;

            Vector3 startGemScale = Vector3.Zero;
            Vector3 targetGemScale = finalGemScale;

            float elapsedTime = 0f;

            while (true)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / gemTravelDuration;
                float curvedT = Mathf.Pow(t, easeIntensity);

                currentAnimGem.transform.position = Vector3.Lerp(startGemPos, targetGemPos, curvedT);
                currentAnimGem.transform.scale = Vector3.Lerp(startGemScale, targetGemScale, curvedT);

                if (t >= 0.85f && currentParticle != null && !currentParticle.GetComponent<ParticleComponent>().IsPlaying)
                {
                    currentParticle.GetComponent<ParticleComponent>().Play();
                    if (currentInsertSFX != null) currentInsertSFX.GetComponent<AudioSource>().Play();
                }
                if (t >= 1f)
                {
                    currentAnimGem.transform.position = targetGemPos;
                    currentAnimGem.transform.scale = targetGemScale;

                    switch (gemIndex)
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

            currentAnimGem.SetActive(false);
            currentStaticGem.SetActive(true);
            gemInserted[gemIndex] = true;
            DatabaseRegistry.levelsDB.Levels.SetBossDoorGemInserted(gemIndex);

            yield return new WaitForSeconds(pauseBeforeOpening * 0.5f);

            if (currentParticle != null) currentParticle.GetComponent<ParticleComponent>().Stop();

            yield return new WaitForSeconds(pauseBeforeOpening * 0.5f);
        }

        bool allGemsInserted = gemInserted[0] && gemInserted[1] && gemInserted[2];

        if (allGemsInserted)
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
            if (staticGems[i] != null) staticGems[i].SetActive(true);
            if (animatedGems[i] != null) animatedGems[i].SetActive(false);
            gemInserted[i] = true;
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