using System;
using Loopie;

public class HealthHUD : Component
{
    public string playerName = "Player";

    public string iconNamePrefix = "Life_";

    public int maxHealthIcons = 5;

    private Health playerHealth;
    private Entity[] healthIcons;
    private int lastKnownHealth = -1;

    void OnCreate()
    {
        Entity playerEntity = Entity.FindEntityByName(playerName);
        if (playerEntity != null)
        {
            playerHealth = playerEntity.GetComponent<Health>();
        }

        healthIcons = new Entity[maxHealthIcons];

        for (int i = 0; i < maxHealthIcons; i++)
        {
            string searchName = iconNamePrefix + (i + 1).ToString();
            healthIcons[i] = Entity.FindEntityByName(searchName);

            if (healthIcons[i] == null)
            {
                Debug.Log("HealthHUD: Falta la parte de la barra llamada: " + searchName);
            }
        }
    }

    void OnUpdate()
    {
        if (playerHealth == null) return;

        int currentHealth = playerHealth.GetActualHealth();

        if (currentHealth != lastKnownHealth)
        {
            UpdateIcons(currentHealth);
            lastKnownHealth = currentHealth;
        }
    }

    private void UpdateIcons(int currentHealth)
    {
        for (int i = 0; i < maxHealthIcons; i++)
        {
            if (healthIcons[i] == null) continue;

            if (i < currentHealth)
            {
                healthIcons[i].SetActive(true);
            }

            else
            {
                healthIcons[i].SetActive(false);
            }
        }
    }
};