using System;
using System.Collections;
using Loopie;

class PuzzleDoor : Component
{
    [Header("References")]
    public Entity rightDoor;
    public Entity leftDoor;
    public Entity staticKey;
    public Entity animatedKey;
    public Entity focusPointOnInsert;

    [Header("Settings")]
    public Vector3 finalRightDoorRot = Vector3.Zero;
    public Vector3 finalLefttDoorRot = Vector3.Zero;
    public float keyTravelDuration = 1.0f;
    public float doorOpenDuration = 2.0f;
    public float pauseBeforeOpening = 0.5f;
    public float easeIntensity = 1.5f;

    private bool isOpening = false;
    private bool hasOpened = false;

    private Vector3 initialRightDoorPos;
    private Vector3 initialRightDoorRot;
    private Vector3 initialLeftDoorPos;
    private Vector3 initialLeftDoorRot;
    private Vector3 finalKeyScale;

    void OnCreate()
    {
        finalKeyScale = animatedKey.transform.scale;

        staticKey.SetActive(false);
        animatedKey.SetActive(false);

        initialRightDoorPos = rightDoor.transform.local_position;
        initialRightDoorRot = rightDoor.transform.local_rotation;
        initialLeftDoorPos = leftDoor.transform.local_position;
        initialLeftDoorRot = leftDoor.transform.local_rotation;
    }

    void OnUpdate()
    {
        if (hasOpened || isOpening) return;

        if (entity.GetComponent<BoxCollider>().IsColliding && Input.IsKeyDown(KeyCode.E) /*&& player has key*/)
        {
            isOpening = true;
            StartCoroutine(OpenDoors());
        }
    }

    IEnumerator OpenDoors()
    {
        animatedKey.SetActive(true);
        animatedKey.transform.position = Player.Instance.transform.position + new Vector3(0, 2, 0);
        animatedKey.transform.scale = Vector3.Zero;

        Vector3 startKeyPos = animatedKey.transform.position;
        Vector3 targetKeyPos = staticKey.transform.position;

        Vector3 startKeyScale = Vector3.Zero;
        Vector3 targetKeyScale = finalKeyScale;

        float elapsedTime = 0f;

        Player.Instance.Camera.FocusOnPoint(focusPointOnInsert.transform.position, 15, 7);

        yield return new WaitForSeconds(1);

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / keyTravelDuration;

            float curvedT = Mathf.Pow(t, easeIntensity);

            animatedKey.transform.position = Vector3.Lerp(startKeyPos, targetKeyPos, curvedT);
            animatedKey.transform.scale = Vector3.Lerp(startKeyScale, targetKeyScale, curvedT);

            if (t >= 1f)
            {
                animatedKey.transform.position = targetKeyPos;
                animatedKey.transform.scale = targetKeyScale;
                break;
            }
            yield return null;
        }

        animatedKey.SetActive(false);
        staticKey.SetActive(true);

        yield return new WaitForSeconds(pauseBeforeOpening);

        elapsedTime = 0f;

        while (true)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / doorOpenDuration;

            float curvedT = Mathf.Pow(t, easeIntensity);

            rightDoor.transform.local_rotation = Vector3.Lerp(initialRightDoorRot, finalRightDoorRot, curvedT);

            leftDoor.transform.local_rotation = Vector3.Lerp(initialLeftDoorRot, finalLefttDoorRot, curvedT);

            if (t >= 1f)
            {
                rightDoor.transform.local_rotation = finalRightDoorRot;
                leftDoor.transform.local_rotation = finalLefttDoorRot;
                break;
            }
            yield return null;
        }

        Player.Instance.Camera.StopFocus();

        hasOpened = true;

        yield return null;
    }

    void OnDestroy()
    {
        StopAllOwnedCoroutines();
    }
}