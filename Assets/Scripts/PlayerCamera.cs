using System;
using Loopie;

class PlayerCamera : Component
{
    public string playerName = "";
    public float distance = 100f;
    public float movementLimit = 5f;
    public float speed = 20f;
    public float followSpeed = 8f;
    public float returnFromInputSpeed = 20f;
    public float returnFromFocusSpeed = 5f;

    private Entity player;
    private Camera camera;

    private string currentState = "FollowingPlayer";
    private string previousState = "FollowingPlayer";

    private bool hasInput;
    private Vector3 inputOffset = Vector3.Zero;
    private Vector3 resultCameraPosition = Vector3.Zero;

    // Isometric
    private const float ISOMETRIC_ANGLE = (float)Math.PI / 4f;
    private float cos = (float)Math.Cos(ISOMETRIC_ANGLE);
    private float sin = (float)Math.Sin(ISOMETRIC_ANGLE);

    // Focus
    private Vector3 focusTarget;
    private float originalZoom;
    private float focusZoom;
    private float currentZoom;
    private float focusSpeed;

    // Testing
    private float toggleCooldown = 0f;

    public void OnCreate()
    {
        player = Entity.FindEntityByName(playerName);
        camera = entity.GetComponent<Camera>();

        originalZoom = camera.GetOrthoSize();
        currentZoom = originalZoom;
    }

    public void OnUpdate()
    {
        //------------------------------------------------------------
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
                FocusOnPoint(new Vector3(-70, 0, 359), 100f, 0.5f);
            }
            else if (currentState == "Focusing")
            {
                StopFocus();
            }
        }
        //------------------------------------------------------------

        EnsurePlayerExists();

        hasInput = CheckInput();

        // Camera Return Mode
        if (!hasInput)
        {
            switch (currentState)
            {
                case "Focusing": ReturnCameraToPlayer(returnFromFocusSpeed); break;
                case "FollowingPlayer": ReturnCameraToPlayer(returnFromInputSpeed); break;
            }
        }

        // Camera Movement Mode
        switch (currentState)
        {
            case "Focusing": UpdateFocus(); break;
            case "FollowingPlayer": UpdateFollowPlayer(); break;
        }
    }

    private void EnsurePlayerExists()
    {
        if (player != null) return;
        player = Entity.FindEntityByName(playerName);
    }

    private bool CheckInput()
    {
        return Input.IsKeyPressed(KeyCode.UP) || Input.IsKeyPressed(KeyCode.DOWN) || Input.IsKeyPressed(KeyCode.LEFT) || Input.IsKeyPressed(KeyCode.RIGHT) || Input.RightAxis != Vector2.Zero;
    }

    private void UpdateFollowPlayer()
    {
        Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * 1.45f, -distance);
        Vector2 movementDirection = GetInputDirection();

        if (hasInput)
        {
            inputOffset += new Vector3(movementDirection.x * speed * Time.deltaTime, 0, movementDirection.y * speed * Time.deltaTime);
        }

        inputOffset.x = Mathf.Clamp(inputOffset.x, -movementLimit, movementLimit);
        inputOffset.z = Mathf.Clamp(inputOffset.z, -movementLimit, movementLimit);

        Vector3 rotatedOffset = new Vector3(inputOffset.x * cos + inputOffset.z * sin, 0f, inputOffset.x * sin - inputOffset.z * cos);

        resultCameraPosition = cameraOriginalPosition + rotatedOffset;

        entity.transform.position += (resultCameraPosition - entity.transform.position) * followSpeed * Time.deltaTime;
    }

    private Vector2 GetInputDirection()
    {
        Vector2 direction = Vector2.Zero;

        if (Input.IsKeyPressed(KeyCode.UP)) direction.x += 1f;
        if (Input.IsKeyPressed(KeyCode.DOWN)) direction.x -= 1f;
        if (Input.IsKeyPressed(KeyCode.LEFT)) direction.y -= 1f;
        if (Input.IsKeyPressed(KeyCode.RIGHT)) direction.y += 1f;

        if (Input.RightAxis != Vector2.Zero)
        {
            direction.x = Input.RightAxis.y;
            direction.y = Input.RightAxis.x;
        }

        float directionLength = (float)Math.Sqrt(direction.x * direction.x + direction.y * direction.y);

        if (directionLength > 1f)
        {
            direction.Normalize();
        }

        return direction;
    }

    private void UpdateFocus()
    {
        Vector3 targetPosition = new Vector3(focusTarget.x, entity.transform.position.y, focusTarget.z);

        entity.transform.position = Vector3.Lerp(entity.transform.position, targetPosition, focusSpeed * Time.deltaTime);

        currentZoom = Mathf.Lerp(currentZoom, focusZoom, focusSpeed * Time.deltaTime);
        camera.SetOrthoSize(currentZoom);
    }

    public void FocusOnPoint(Vector3 destination, float zoomSize, float speed)
    {
        currentState = "Focusing";
        previousState = "FollowingPlayer";
        focusTarget = destination;
        focusZoom = zoomSize;
        focusSpeed = speed;
    }

    public void StopFocus()
    {
        currentState = "FollowingPlayer";
        previousState = "Focusing";
        currentZoom = originalZoom;
    }

    private void ReturnCameraToPlayer(float returnSpeed)
    {
        inputOffset = Vector3.Lerp(inputOffset, Vector3.Zero, returnSpeed * Time.deltaTime);

        currentZoom = Mathf.Lerp(currentZoom, originalZoom, focusSpeed * Time.deltaTime);
        camera.SetOrthoSize(currentZoom);
    }
}