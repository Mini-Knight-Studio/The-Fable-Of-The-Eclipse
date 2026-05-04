using System;
using Loopie;

class Meteorite : Component
{
    [Header("Settings")]
    public float meteoriteTime = 2.0f;
    public float fallDistance = 25.0f;

    [Header("Wiggle Effect")]
    public float wiggleSpeed = 10.0f;
    public float wiggleIntensity = 15.0f;

    [Header("Juice / Feedback")]
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.3f;
    public float shakeRotation = 0.2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float timer = 0.0f;
    private float totalElapsed = 0.0f; // Cronómetro manual para el Wiggle
    private bool isFalling = false;

    void OnCreate()
    {
        // Guardamos posición inicial y calculamos destino
        startPos = transform.position;
        targetPos = new Vector3(startPos.x, startPos.y - fallDistance, startPos.z);

        // Inicializamos estados y tiempos
        isFalling = true;
        timer = 0.0f;
        totalElapsed = 0.0f;
    }

    void OnUpdate()
    {
        if (!isFalling) return;

        // Sumamos el tiempo transcurrido (usando la propiedad estática de tu namespace Loopie)
        totalElapsed += Time.deltaTime;

        if (timer < meteoriteTime)
        {
            timer += Time.deltaTime;
            float t = timer / meteoriteTime;

            // 1. Movimiento lineal de caída hacia el suelo
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            // 2. Efecto de rotación oscilatoria (Wiggle)
            // Calculamos el ángulo usando el Seno del tiempo acumulado
            float angle = Mathf.Sin(totalElapsed * wiggleSpeed) * wiggleIntensity;

            // Aplicamos la rotación al transform
            transform.rotation = new Vector3(0, 0, angle);
        }
        else
        {
            // Detenemos la caída y ejecutamos el impacto
            isFalling = false;
            Impact();
        }
    }

    void Impact()
    {
        // Trigger del Camera Shake al tocar el suelo
        // Usamos la referencia estática del Player que funciona en tu proyecto
        if (Player.Instance != null && Player.Instance.Camera != null)
        {
            Player.Instance.Camera.SetIsShaking(true, shakeDuration, shakeAmount, shakeRotation);
        }

        // Destrucción de la entidad para liberar memoria
        entity.Destroy();
    }
};