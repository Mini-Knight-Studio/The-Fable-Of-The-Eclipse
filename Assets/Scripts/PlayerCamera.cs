using System;
using Loopie;

class PlayerCamera : Component
{
    public string playerName = "";
    public float distance = 100f;
    public float speed = 25f;
    public float followSpeed = 8f;

    private Entity player;
    private Camera camera;

    private string currentState = "FollowingPlayer";
    private string previousState = "FollowingPlayer";

    // Isometric
    private const float ISOMETRIC_ANGLE = (float)Math.PI / 4f;
    private float cos = (float)Math.Cos(ISOMETRIC_ANGLE);
    private float sin = (float)Math.Sin(ISOMETRIC_ANGLE);

    // Input 
    public float movementLimit = 5f;
    private bool hasInput;
    private Vector3 inputOffset = Vector3.Zero;

    // Focus
    private Vector3 focusTarget;
    private float originalZoom;
    private float focusZoom;
    private float currentZoom;
    private float focusSpeed;

    public void OnCreate()
    {
        player = Entity.FindEntityByName(playerName);
        camera = entity.GetComponent<Camera>();

        originalZoom = camera.GetOrthoSize();
        currentZoom = originalZoom;
    }

    public void OnUpdate()
    {
        if (player == null) return;

        hasInput = CheckInput();

        switch (currentState)
        {
            case "Focusing": UpdateFocus(); break;
            case "FollowingPlayer": UpdateFollowPlayer(); break;
        }
    }

    private bool CheckInput()
    {
        return Input.IsKeyPressed(KeyCode.UP) || Input.IsKeyPressed(KeyCode.DOWN) || Input.IsKeyPressed(KeyCode.LEFT) || Input.IsKeyPressed(KeyCode.RIGHT) || Input.RightAxis != Vector2.Zero;
    }

    private void UpdateFollowPlayer()
    {
        Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * 1.45f, -distance);
        Vector2 movementDirection = GetInputDirection();

        float delta = Time.deltaTime;
        float moveAmount = speed * delta;

        if (hasInput)
        {
            inputOffset.x += movementDirection.x * moveAmount;
            inputOffset.z += movementDirection.y * moveAmount;

            inputOffset.x = Mathf.Clamp(inputOffset.x, -movementLimit, movementLimit);
            inputOffset.z = Mathf.Clamp(inputOffset.z, -movementLimit, movementLimit);
        }
        else
        {
            float length = (float)Math.Sqrt(inputOffset.x * inputOffset.x + inputOffset.z * inputOffset.z);

            if (length > 0f)
            {
                float newLength = Math.Max(0f, length - moveAmount);

                inputOffset.x = inputOffset.x / length * newLength;
                inputOffset.z = inputOffset.z / length * newLength;
            }

            currentZoom = Mathf.Lerp(currentZoom, originalZoom, focusSpeed * Time.deltaTime);
            camera.SetOrthoSize(currentZoom);
        }

        Vector3 rotatedOffset = new Vector3(inputOffset.x * cos + inputOffset.z * sin, 0f, inputOffset.x * sin - inputOffset.z * cos);
        Vector3 targetPosition = cameraOriginalPosition + rotatedOffset;

        entity.transform.position = Vector3.Lerp(entity.transform.position, targetPosition, followSpeed * delta);
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
        Vector3 targetPosition = new Vector3(focusTarget.x - distance, entity.transform.position.y, focusTarget.z - distance);

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
}
