using System;
using Loopie;

class VistaPoint : Component
{
    public float zoom = 20;
    public float speed = 0.5f;

    private string cameraName = "PlayerCamera";

    private PlayerCamera camera;
    private BoxCollider collider;

    void OnCreate()
    {
        Entity cameraEntity = Entity.FindEntityByName(cameraName);
        if (cameraEntity != null)
        {
            camera = cameraEntity.GetComponent<PlayerCamera>();
        }
        collider = entity.GetComponent<BoxCollider>();
    }

    void OnUpdate()
    {
        if (camera == null) return;
        if (collider == null) return;

        if (collider.IsColliding)
        {
            camera.FocusOnPoint(entity.transform.position, zoom, speed);
            Debug.Log("Colliding");
        }
        else if (collider.HasEndedCollision)
        {
            camera.StopFocus();
            Debug.Log("Stop Colliding");
        }

    }
};