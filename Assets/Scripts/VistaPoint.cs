using System;
using Loopie;

class VistaPoint : Component
{
    public float zoom = 20;
    public float time = 50f;

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
        else
        {
            Debug.Log("There is no PlayerCamera");
        }

        collider = entity.GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.Log("The VistaPoint lacks a Collider");
        }
    }

    void OnUpdate()
    {
        if (camera == null) return;
        if (collider == null) return;

        if (collider.HasCollided)
        {
            camera.FocusOnPoint(entity.transform.position, zoom, time);
        }
        else if (collider.HasEndedCollision)
        {
            camera.StopFocus();
        }

    }
};