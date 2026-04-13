using System;
using Loopie;

public class SkillsHUD : Component
{
    // Nombres exactos de las imágenes en la jerarquía de tu escena
    public string playerName = "Player";

    public string dashActiveName = "Dash_Active";
    public string dashInactiveName = "Dash_Inactive";

    public string grappleActiveName = "Grapple_Active";
    public string grappleInactiveName = "Grapple_Inactive";

    // Variables privadas para no petar el motor
    private PlayerMovement playerMovement;
    private PlayerGrapple playerGrapple;

    private Entity dashActiveEntity;
    private Entity dashInactiveEntity;
    private Entity grappleActiveEntity;
    private Entity grappleInactiveEntity;

    // Control para no hacer SetActive miles de veces por segundo
    private bool wasDashReady = true;
    private bool wasGrappleReady = true;

    void OnCreate()
    {
        // 1. Buscamos los scripts del jugador
        Entity player = Entity.FindEntityByName(playerName);
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
            playerGrapple = player.GetComponent<PlayerGrapple>();
        }

        // 2. Buscamos las entidades de la interfaz
        dashActiveEntity = Entity.FindEntityByName(dashActiveName);
        dashInactiveEntity = Entity.FindEntityByName(dashInactiveName);

        grappleActiveEntity = Entity.FindEntityByName(grappleActiveName);
        grappleInactiveEntity = Entity.FindEntityByName(grappleInactiveName);

        // Ocultamos las imágenes grises al iniciar, asumiendo que empezamos con todo listo
        if (dashInactiveEntity != null) dashInactiveEntity.SetActive(false);
        if (grappleInactiveEntity != null) grappleInactiveEntity.SetActive(false);
    }

    void OnUpdate()
    {
        // --- LÓGICA DEL DASH ---
        if (playerMovement != null && dashActiveEntity != null && dashInactiveEntity != null)
        {
            // El dash está listo si el temporizador llega a 0
            bool isDashReady = playerMovement.dashCooldownTimer <= 0;

            // Si el estado ha cambiado respecto al último frame, intercambiamos imágenes
            if (isDashReady != wasDashReady)
            {
                dashActiveEntity.SetActive(isDashReady);
                dashInactiveEntity.SetActive(!isDashReady);
                wasDashReady = isDashReady;
            }
        }

        // --- LÓGICA DEL GRAPPLE ---
        if (playerGrapple != null && grappleActiveEntity != null && grappleInactiveEntity != null)
        {
            // El grapple está listo si el temporizador llega a 0
            bool isGrappleReady = playerGrapple.grappleCooldownTimer <= 0;

            if (isGrappleReady != wasGrappleReady)
            {
                grappleActiveEntity.SetActive(isGrappleReady);
                grappleInactiveEntity.SetActive(!isGrappleReady);
                wasGrappleReady = isGrappleReady;
            }
        }
    }
};