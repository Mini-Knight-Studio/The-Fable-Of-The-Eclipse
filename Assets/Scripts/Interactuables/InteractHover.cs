using System;
using System.Collections;
using Loopie;

class InteractHover : Component
{
    [Header("References")]
    public Entity interactGamepad;
    public Entity interactKeyboard;
    private Entity targetEntity; // Added for LookAt functionality

    [Header("Static or hovering")]
    public bool hovers = true;

    [Header("Hover Settings")]
    public float amplitude = 0.5f;
    public float speed = 2.0f;
    public bool started = false;

    private Vector3 startLocalPos;
    private float time;

    void OnCreate()
    {
        StartMoving();

        if (interactGamepad != null && interactKeyboard != null)
        {
            interactKeyboard.GetComponent<MeshRenderer>().SetActive(false);
            interactGamepad.GetComponent<MeshRenderer>().SetActive(false);
        }
    }

    void OnPostCreate()
    {
        targetEntity = Player.Instance.Camera.entity;
    }

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

        // 1. Input Device Mesh Toggling (Cleaned up duplicate checks)
        if (interactGamepad != null && interactKeyboard != null)
        {
            if (Input.CurrentInputDevice == Input.InputDevice.Gamepad && !interactGamepad.GetComponent<MeshRenderer>().IsActive())
            {
                interactGamepad.GetComponent<MeshRenderer>().SetActive(true);
                interactKeyboard.GetComponent<MeshRenderer>().SetActive(false);
            }
            else if (Input.CurrentInputDevice == Input.InputDevice.MouseKeyboard && !interactKeyboard.GetComponent<MeshRenderer>().IsActive())
            {
                interactKeyboard.GetComponent<MeshRenderer>().SetActive(true);
                interactGamepad.GetComponent<MeshRenderer>().SetActive(false);
            }
        }

        // 2. Hardcoded LookAt Logic (Target Axis: Y, Offset: 135 on Z)
        if (targetEntity != null)
        {
            LookAtTarget(interactKeyboard.transform);
            LookAtTarget(interactGamepad.transform);
        }

        // 3. Hovering Logic
        if (!started) return;

        time += Time.deltaTime * speed;
        float offsetY = (float)Math.Sin(time) * amplitude;

        interactKeyboard.transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
        interactGamepad.transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
    }

    public void StartMoving()
    {
        if (hovers)
        {
            startLocalPos = transform.local_position;
            time = 0f;
            started = true;
        }
    }

    public void LookAtTarget(Transform user)
    {
        Vector3 targetPos = targetEntity.transform.position;
        Vector3 currentPosition = user.position;
        Vector3 direction = targetPos - currentPosition;

        float distance = (float)direction.magnitude;
        if (distance >= 0.001f)
        {
            direction = direction / distance; // Normalize

            // Build coordinate system
            Vector3 worldUp = Vector3.Up;
            Vector3 right = Vector3.Cross(worldUp, direction);

            if (right.magnitude < 0.001f)
            {
                right = Vector3.Right;
            }
            else
            {
                right.Normalize();
            }

            Vector3 up = Vector3.Cross(direction, right).normalized;

            // Hardcoded 135 degree Z-Rotation offset manipulation
            // 135 degrees = 2.35619449 radians
            float radZ = -180f * Mathf.Deg2Rad;
            up = (up * Mathf.Cos(radZ) + right * Mathf.Sin(radZ)).normalized;

            // Hardcoded TargetAxis.Y Mapping
            Vector3 finalLookTarget = currentPosition - up;
            Vector3 finalUpVector = direction;

            user.LookAt(finalLookTarget, finalUpVector);
        }
    }

    public void DeactivatePromt()
    {
        interactKeyboard.SetActive(false);
        interactGamepad.SetActive(false);
    }

    public void ActivatePromt()
    {
        interactKeyboard.SetActive(true);
        interactGamepad.SetActive(true);
    }
}