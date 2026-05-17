using Loopie;

public enum TargetAxis
{
    X,
    Y,
    Z
}

class LookAtEntity : Component
{
    public Entity targetEntity;
    public TargetAxis lookAxis = TargetAxis.Z;

    [Tooltip("Rotation offset in degrees (X = Pitch, Y = Yaw, Z = Roll)")]
    public Vector3 rotationOffset = Vector3.Zero;

    void OnUpdate()
    {
        // 1. Safety check
        if (targetEntity == null) return;

        Vector3 targetPos = targetEntity.transform.position;
        Vector3 currentPosition = entity.transform.position;
        Vector3 direction = targetPos - currentPosition;

        // Prevent division by zero if entities overlap
        float distance = (float)direction.magnitude;
        if (distance < 0.001f)
            return;

        // Manually normalize to avoid any internal engine NaN quirks
        direction = direction / distance;

        // 2. Build a stable coordinate system based entirely on world coordinates
        Vector3 worldUp = Vector3.Up;
        Vector3 right = Vector3.Cross(worldUp, direction);

        // Handle target being directly above/below the entity
        if (right.magnitude < 0.001f)
        {
            // Fallback to a stable default world direction instead of reading transform
            right = Vector3.Right;
        }
        else
        {
            right.Normalize();
        }

        Vector3 up = Vector3.Cross(direction, right).normalized;

        // 3. Apply rotation offsets (Trig space manipulation)
        if (rotationOffset.y != 0.0f)
        {
            float radY = rotationOffset.y * Mathf.Deg2Rad;
            direction = (direction * Mathf.Cos(radY) + right * Mathf.Sin(radY)).normalized;
            // Recalculate right relative to world up to maintain horizon stability
            right = Vector3.Cross(worldUp, direction).normalized;
            up = Vector3.Cross(direction, right).normalized;
        }
        if (rotationOffset.x != 0.0f)
        {
            float radX = rotationOffset.x * Mathf.Deg2Rad;
            direction = (direction * Mathf.Cos(radX) + up * Mathf.Sin(radX)).normalized;
            up = Vector3.Cross(direction, right).normalized;
        }
        if (rotationOffset.z != 0.0f)
        {
            float radZ = rotationOffset.z * Mathf.Deg2Rad;
            up = (up * Mathf.Cos(radZ) + right * Mathf.Sin(radZ)).normalized;
        }

        // 4. Map the calculated vectors to standard LookAt targets.
        // Instead of reading 'transform' properties live, we mathematically determine
        // exactly where the true local Z-Forward vector must point.
        Vector3 finalLookTarget = Vector3.Zero;
        Vector3 finalUpVector = up;

        switch (lookAxis)
        {
            case TargetAxis.Z:
                // Normal setup: true forward faces target direction
                finalLookTarget = currentPosition + direction;
                finalUpVector = up;
                break;

            case TargetAxis.X:
                // Shift 90 degrees: true forward faces the calculated 'left' vector
                // so that local X faces the target direction
                finalLookTarget = currentPosition - right;
                finalUpVector = up;
                break;

            case TargetAxis.Y:
                // Shift 90 degrees: true forward faces the calculated 'down' vector
                // so that local Y faces the target direction
                finalLookTarget = currentPosition - up;
                finalUpVector = direction;
                break;
        }

        // 5. Fire a single, clean LookAt calculation
        transform.LookAt(finalLookTarget, finalUpVector);
    }
}