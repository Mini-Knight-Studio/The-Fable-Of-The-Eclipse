using System;
using Loopie;

class PlayerCamera : Component
{
    public bool freeCamera = false;
    public string playerName = "";
    public float distance = 15f;
    public float movementLimit = 5f;
    public float speed = 10f;

    private Entity player;
    private Vector3 inputOffset = Vector3.Zero;
    private Vector3 resultCameraPosition = Vector3.Zero;

    private const float ISO_ANGLE = (float)Math.PI / 4f;
    private float cos = (float)Math.Cos(ISO_ANGLE);
    private float sin = (float)Math.Sin(ISO_ANGLE);

    public void OnCreate()
    {
        player = Entity.FindEntityByName(playerName);
    }

    public void OnUpdate()
    {
        if (player == null)
        {
            player = Entity.FindEntityByName(playerName);
            if (player == null) return;
        }

        if (!freeCamera)
        {
            Vector3 cameraOriginalPosition = player.transform.position + new Vector3(-distance, distance * 1.25f, -distance);

            // Move Camera
            Vector2 movementDirection = new Vector2(0,0);

            if (Input.IsKeyPressed(KeyCode.UP)) { movementDirection.x += 1f; }
            if (Input.IsKeyPressed(KeyCode.DOWN)) { movementDirection.x -= 1f; }
            if (Input.IsKeyPressed(KeyCode.LEFT)) { movementDirection.y += 1f; }
            if (Input.IsKeyPressed(KeyCode.RIGHT)) { movementDirection.y -= 1f; }

            float lengthMovementDirection = (float)Math.Sqrt(movementDirection.x * movementDirection.x + movementDirection.y * movementDirection.y);

            if (lengthMovementDirection > 1f)
            {
                movementDirection.x /= lengthMovementDirection;
                movementDirection.y /= lengthMovementDirection;
            }

            inputOffset.x += movementDirection.x * speed * Time.deltaTime;
            inputOffset.z += movementDirection.y * speed * Time.deltaTime;

            // Return to Origin
            if (!Input.IsKeyPressed(KeyCode.UP) && !Input.IsKeyPressed(KeyCode.DOWN) && !Input.IsKeyPressed(KeyCode.LEFT) && !Input.IsKeyPressed(KeyCode.RIGHT))
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
            inputOffset.x = Clamp(inputOffset.x, -movementLimit, movementLimit);
            inputOffset.z = Clamp(inputOffset.z, -movementLimit, movementLimit);

            Vector3 rotatedOffset = new Vector3(inputOffset.x * cos - inputOffset.z * sin, 0f, inputOffset.x * sin + inputOffset.z * cos);

            resultCameraPosition = cameraOriginalPosition + rotatedOffset;

            entity.transform.position = resultCameraPosition;
        }
    }

    private float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}