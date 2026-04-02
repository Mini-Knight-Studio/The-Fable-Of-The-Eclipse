using Loopie;

class PlayerCamera : Component
{
    public float distance = 100f;
    public float speed = 25f;
    public float verticalScale = 1.22f;

    public Entity player;
    private Camera camera;

    private cameraState currentState = cameraState.FOLLOWING_PLAYER;
    private cameraState previousState = cameraState.FOLLOWING_PLAYER;

    private enum cameraState
    {
        FOLLOWING_PLAYER,
        FOCUSING,
        STOP_FOCUSING
    }

    // Isometric
    private const float ISOMETRIC_ANGLE = (float)Mathf.PI / 4f;
    private float cos = (float)Mathf.Cos(ISOMETRIC_ANGLE);
    private float sin = (float)Mathf.Sin(ISOMETRIC_ANGLE);

    // Input 
    public float movementLimit = 5f;
    private bool hasInput;
    private Vector3 inputOffset = Vector3.Zero;

    // Focus
    private Vector3 focusTarget;
    private float originalZoom;
    private float focusZoom;
    private float currentZoom;

    // Shaking
    private bool isShaking = false;
    private float transformShakeAmount = 1;
    private float rotationShakeAmount = 1.5f;
    private Vector3 shakeOffset = Vector3.Zero;
    private Vector3 shakeRotationOffset = Vector3.Zero;
    
    // Timers
    private float lerpTimer;
    private float shakeTimer;
    public float timeToFollowPlayer = 1f;
    public float timeToFocus = 5;
    public float timeToStopFocusing = 1;
    public float timeToShake = 3;

    public void OnCreate()
    {
        if (player != null)
        {
            // Get any player data.
        }
        else
        {
            Debug.Log("Error: There is no Player.");
        }

        camera = entity.GetComponent<Camera>();
        if (camera != null)
        {
            originalZoom = camera.GetOrthoSize();
            currentZoom = originalZoom;
        }
        else
        {
            Debug.Log("Error: The PlayerCamera lacks a Camera Component.");
        }
    }

    public void OnUpdate()
    {
        if (player == null) return;

        hasInput = CheckInput();

        switch (currentState)
        {
            case cameraState.FOCUSING: UpdateFocus(); break;
            case cameraState.STOP_FOCUSING: UpdateStopFocus(); break;
            case cameraState.FOLLOWING_PLAYER: UpdateFollowPlayer(); break;
        }

        if (isShaking)
        {
            UpdateShake();
        }

        lerpTimer += Time.deltaTime;
    }

    private bool CheckInput()
    {
        return Input.IsKeyPressed(KeyCode.UP) || Input.IsKeyPressed(KeyCode.DOWN) || Input.IsKeyPressed(KeyCode.LEFT) || Input.IsKeyPressed(KeyCode.RIGHT) || Input.RightAxis != Vector2.Zero;
    }

    private void UpdateFollowPlayer()
    {
        Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * verticalScale, -distance);
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
            float length = (float)Mathf.Sqrt(inputOffset.x * inputOffset.x + inputOffset.z * inputOffset.z);

            if (length > 0f)
            {
                float newLength = Mathf.Max(0f, length - moveAmount);

                inputOffset.x = inputOffset.x / length * newLength;
                inputOffset.z = inputOffset.z / length * newLength;
            }

            currentZoom = Mathf.Lerp(currentZoom, originalZoom, Time.deltaTime / timeToFollowPlayer);
            camera.SetOrthoSize(currentZoom);
        }

        Vector3 rotatedOffset = new Vector3(inputOffset.x * cos + inputOffset.z * sin, 0f, inputOffset.x * sin - inputOffset.z * cos);
        Vector3 targetPosition = cameraOriginalPosition + rotatedOffset;

        entity.transform.position = Vector3.Lerp(entity.transform.position, targetPosition, Time.deltaTime / timeToFollowPlayer);
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

        float directionLength = (float)Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);

        if (directionLength > 1f)
        {
            direction.Normalize();
        }

        return direction;
    }

    private void UpdateFocus()
    {
        Vector3 targetPosition = new Vector3(focusTarget.x - distance, entity.transform.position.y, focusTarget.z - distance);

        entity.transform.position = Vector3.Lerp(entity.transform.position, targetPosition, lerpTimer/timeToFocus);

        currentZoom = Mathf.Lerp(currentZoom, focusZoom, lerpTimer/timeToFocus);
        camera.SetOrthoSize(currentZoom);
    }

    public void FocusOnPoint(Vector3 destination, float zoomSize, float time)
    {
        currentState = cameraState.FOCUSING; 
        previousState = cameraState.FOLLOWING_PLAYER;
        focusTarget = destination;
        focusZoom = zoomSize;
        timeToFocus = time;

        lerpTimer = 0;
    }

    private void UpdateStopFocus()
    {
        Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * verticalScale, -distance);

        if (Mathf.Abs(currentZoom - originalZoom) < 0.1f)
        {
            currentZoom = originalZoom;

            if (Vector3.Distance(entity.transform.position, cameraOriginalPosition) < 0.1f)
            {
                entity.transform.position = cameraOriginalPosition;
                currentState = cameraState.FOLLOWING_PLAYER;
                lerpTimer = 0;
            }
            else
            {
                entity.transform.position = Vector3.Lerp(entity.transform.position, cameraOriginalPosition, lerpTimer/timeToStopFocusing);
            }
        }
        else
        {
            currentZoom = Mathf.Lerp(currentZoom, originalZoom, lerpTimer/timeToStopFocusing);
            camera.SetOrthoSize(currentZoom);

            entity.transform.position = Vector3.Lerp(entity.transform.position, cameraOriginalPosition, lerpTimer/timeToStopFocusing);
        }
    }

    public void StopFocus()
    {
        currentState = cameraState.STOP_FOCUSING;
        previousState = cameraState.FOCUSING;

        lerpTimer = 0;
    }

    private void UpdateShake()
    {
        shakeTimer += Time.deltaTime;

        entity.transform.position -= shakeOffset;
        entity.transform.rotation -= shakeRotationOffset;

        float progress = shakeTimer / timeToShake;

        if (progress >= 1f)
        {
            shakeOffset = Vector3.Zero;
            shakeRotationOffset = Vector3.Zero;
            isShaking = false;
            return;
        }

        float strength = 1f - Mathf.SmoothStep(0f, 1f, progress);

        float currentTransformShake = transformShakeAmount * strength;
        float currentRotationShake = rotationShakeAmount * strength;

        float offsetX = Loopie.Random.Range(-currentTransformShake, currentTransformShake);
        float offsetY = Loopie.Random.Range(-currentTransformShake, currentTransformShake);
        float offsetZ = Loopie.Random.Range(-currentTransformShake, currentTransformShake);

        shakeOffset = new Vector3(offsetX, offsetY, offsetZ);

        float rotZ = Loopie.Random.Range(-currentRotationShake, currentRotationShake);

        shakeRotationOffset = new Vector3(0f, 0f, rotZ);

        entity.transform.position += shakeOffset;
        entity.transform.rotation += shakeRotationOffset;
    }

    public void SetIsShaking(bool active, float time = 1f, float amount = 1f, float rotationAmount = 1f)
    {
        isShaking = active;
        timeToShake = time;
        transformShakeAmount = amount;
        rotationShakeAmount = rotationAmount;

        shakeTimer = 0f;
    }
}
