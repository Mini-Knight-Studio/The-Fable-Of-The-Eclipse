using System;
using Loopie;

class PlayerCamera : Component
{
    public string playerName = "";
    public float distance = 15f;
    public float movementLimit = 5f;
    public float speed = 20f;
    public float followSpeed = 8f;

    private string currentState = "FollowingPlayer";
    private string previousState = "FollowingPlayer";

    private Entity player;

    private Vector3 inputOffset = Vector3.Zero;
    private Vector3 resultCameraPosition = Vector3.Zero;

    // Isometric
    private const float ISOMETRIC_ANGLE = (float)Math.PI / 4f;
    private float cos = (float)Math.Cos(ISOMETRIC_ANGLE);
    private float sin = (float)Math.Sin(ISOMETRIC_ANGLE);

    // Focus
    private Vector3 focusTarget;
    private float focusZoom;
    private float currentZoom;
    private float focusSpeed;

    // Testing
    private float toggleCooldown = 0f;

    public void OnCreate()
    {
        player = Entity.FindEntityByName(playerName);
        currentZoom = distance;
    }

    public void OnUpdate()
    {
        if (player == null)
        {
            player = Entity.FindEntityByName(playerName);
            if (player == null) return;
        }

        // Test focus on point
        if (toggleCooldown > 0f)
        {
            toggleCooldown -= Time.deltaTime;
        }
        if (Input.IsKeyPressed(KeyCode.P) && toggleCooldown <= 0f)
        {
            toggleCooldown = 0.25f; 

            if (currentState == "FollowingPlayer")
            {
                FocusOnPoint(new Vector3(0, 0, 0), 30f, 1);

                currentState = "Focusing";
                previousState = "FollowingPlayer";
            }
            else if (currentState == "Focusing")
            {
                currentZoom = distance;

                currentState = "FollowingPlayer";
                previousState = "Focusing";
            }
        }

        if (currentState == "Focusing")
        {
            UpdateFocus();
        }
        else if (currentState == "FollowingPlayer")
        {
            UpdateFollowPlayer();
        } 
    }

    private void UpdateFollowPlayer()
    {
        Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * 1.25f, -distance);

        // Move Camera
        Vector2 movementDirection = new Vector2(0, 0);

        if (Input.IsKeyPressed(KeyCode.UP)) { movementDirection.x += 1f; }
        if (Input.IsKeyPressed(KeyCode.DOWN)) { movementDirection.x -= 1f; }
        if (Input.IsKeyPressed(KeyCode.LEFT)) { movementDirection.y -= 1f; }
        if (Input.IsKeyPressed(KeyCode.RIGHT)) { movementDirection.y += 1f; }

        if (Input.RightAxis.x != 0 || Input.RightAxis.y != 0)
        {
            movementDirection.x = -Input.RightAxis.y;
            movementDirection.y = Input.RightAxis.x;
        }

        float lengthMovementDirection = (float)Math.Sqrt(movementDirection.x * movementDirection.x + movementDirection.y * movementDirection.y);

        if (lengthMovementDirection > 1f)
        {
            movementDirection.x /= lengthMovementDirection;
            movementDirection.y /= lengthMovementDirection;
        }

        inputOffset.x += movementDirection.x * speed * Time.deltaTime;
        inputOffset.z += movementDirection.y * speed * Time.deltaTime;

        // Return to Origin
        if (!Input.IsKeyPressed(KeyCode.UP) && !Input.IsKeyPressed(KeyCode.DOWN) && !Input.IsKeyPressed(KeyCode.LEFT) && !Input.IsKeyPressed(KeyCode.RIGHT) && Input.RightAxis == Vector2.Zero)
        {
            if (previousState == "Focusing")
            {
                ReturnCameraToPlayer(focusSpeed);
            }
            else if (previousState == "FollowingPlayer")
            {
                ReturnCameraToPlayer(speed);
            }
        }

        // Camera Limits
        inputOffset.x = Mathf.Clamp(inputOffset.x, -movementLimit, movementLimit);
        inputOffset.z = Mathf.Clamp(inputOffset.z, -movementLimit, movementLimit);

        Vector3 rotatedOffset = new Vector3(inputOffset.x * cos + inputOffset.z * sin, 0f, inputOffset.x * sin - inputOffset.z * cos);

        resultCameraPosition = cameraOriginalPosition + rotatedOffset;

        entity.transform.position += (resultCameraPosition - entity.transform.position) * followSpeed * Time.deltaTime;
    }

    private void UpdateFocus()
    {
        float zoomDifference = focusZoom - currentZoom;
        currentZoom = currentZoom + zoomDifference * focusSpeed * Time.deltaTime;

        Vector3 targetCameraPosition = new Vector3(focusTarget.x - currentZoom, focusTarget.y + currentZoom * 1.25f, focusTarget.z - currentZoom);

        Vector3 currentPosition = entity.transform.position;
        currentPosition.x = currentPosition.x + (targetCameraPosition.x - currentPosition.x) * focusSpeed * Time.deltaTime;
        currentPosition.y = currentPosition.y + (targetCameraPosition.y - currentPosition.y) * focusSpeed * Time.deltaTime;
        currentPosition.z = currentPosition.z + (targetCameraPosition.z - currentPosition.z) * focusSpeed * Time.deltaTime;

        entity.transform.position = currentPosition;
    }

    public void FocusOnPoint(Vector3 destination, float zoomAmount, float speed)
    {
        focusTarget = destination;
        focusZoom = zoomAmount;
        focusSpeed = speed;
    }

    private void ReturnCameraToPlayer(float returnSpeed)
    {
        float lengthInputOffset = (float)Math.Sqrt(inputOffset.x * inputOffset.x + inputOffset.z * inputOffset.z);
        if (lengthInputOffset > 0f)
        {
            float returnStep = returnSpeed * Time.deltaTime;

            if (lengthInputOffset <= returnStep)
            {
                inputOffset = Vector3.Zero;
            }
            else
            {
                float scale = (lengthInputOffset - returnStep) / lengthInputOffset;
                inputOffset.x *= scale;
                inputOffset.z *= scale;
            }
        }
    }
}