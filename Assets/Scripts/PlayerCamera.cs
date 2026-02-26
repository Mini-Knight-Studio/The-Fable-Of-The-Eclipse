using System;
using Loopie;

class PlayerCamera : Component
{
    public string playerName = "";
    public float distance = 15f;
    public float movementLimit = 5f;
    public float speed = 20f;
    public float followStrength = 8f;

    private Entity player;
    private Vector3 inputOffset = Vector3.Zero;
    private Vector3 resultCameraPosition = Vector3.Zero;

    private const float ISOMETRIC_ANGLE = (float)Math.PI / 4f;
    private float cos = (float)Math.Cos(ISOMETRIC_ANGLE);
    private float sin = (float)Math.Sin(ISOMETRIC_ANGLE);

    private bool isFocusing = false;
    private bool isFollowingPlayer = true;
    private bool isFree = false;

    // Focus
    private Vector3 focusTarget;
    private float focusZoom;
    private float currentZoom;
    public float focusStrength = 1f;

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

        // Test trigger for focus
        if (toggleCooldown > 0f)
        {
            toggleCooldown -= Time.deltaTime;
        }
        if (Input.IsKeyPressed(KeyCode.P) && toggleCooldown <= 0f)
        {
            toggleCooldown = 0.25f; 

            if (isFollowingPlayer)
            {
                isFocusing = true;
                isFollowingPlayer = false;

                FocusOnPoint(new Vector3(0, 0, 0), 30f);
            }
            else if (isFocusing)
            {
                isFocusing = false;
                isFollowingPlayer = true;

                currentZoom = distance;
            }
        }

        if (isFocusing)
        {
            UpdateFocus();
            return;
        }

        if (isFollowingPlayer)
        {
            Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * 1.25f, -distance);

            // Move Camera
            Vector2 movementDirection = new Vector2(0,0);

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
                float lengthInputOffset = (float)Math.Sqrt(inputOffset.x * inputOffset.x + inputOffset.z * inputOffset.z);
                if (lengthInputOffset > 0f)
                {
                    float returnStep = speed * Time.deltaTime;

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

            // Camera Limits
            inputOffset.x = Mathf.Clamp(inputOffset.x, -movementLimit, movementLimit);
            inputOffset.z = Mathf.Clamp(inputOffset.z, -movementLimit, movementLimit);

            Vector3 rotatedOffset = new Vector3(inputOffset.x * cos + inputOffset.z * sin, 0f, inputOffset.x * sin - inputOffset.z * cos);

            resultCameraPosition = cameraOriginalPosition + rotatedOffset;

            entity.transform.position += (resultCameraPosition - entity.transform.position) * followStrength * Time.deltaTime;
        }
    }

    public void FocusOnPoint(Vector3 destination, float zoomAmount)
    {
        focusTarget = destination;
        focusZoom = zoomAmount;
    }

    private void UpdateFocus()
    {
        // ----- Smooth Zoom -----
        float zoomDifference = focusZoom - currentZoom;
        currentZoom = currentZoom + zoomDifference * followStrength * Time.deltaTime;

        // ----- Build Target Camera Position -----
        Vector3 targetCameraPosition = new Vector3(
            focusTarget.x - currentZoom,
            focusTarget.y + currentZoom * 1.25f,
            focusTarget.z - currentZoom
        );

        // ----- Manual Smooth Movement -----
        Vector3 currentPosition = entity.transform.position;

        float dx = targetCameraPosition.x - currentPosition.x;
        float dy = targetCameraPosition.y - currentPosition.y;
        float dz = targetCameraPosition.z - currentPosition.z;

        currentPosition.x = currentPosition.x + dx * followStrength * Time.deltaTime;
        currentPosition.y = currentPosition.y + dy * followStrength * Time.deltaTime;
        currentPosition.z = currentPosition.z + dz * followStrength * Time.deltaTime;

        entity.transform.position = currentPosition;
    }
}