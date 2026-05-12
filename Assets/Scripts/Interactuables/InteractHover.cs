using System;
using System.Collections;
using Loopie;

class InteractHover : Component
{
    [Header("References")]
    public Entity interactGamepad;
    public Entity interactKeyboard;

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

    void OnUpdate()
    {
        if (GameManager.state != GameManager.GameState.DEFAULT) { return; }

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

        if (!started) return;

        time += Time.deltaTime * speed;

        float offsetY = (float)Math.Sin(time) * amplitude;

        transform.local_position = new Vector3(startLocalPos.x, startLocalPos.y + offsetY, startLocalPos.z);
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
};