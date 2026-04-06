using System;
using Loopie;

public class HealItem : Component
{
	public int healAmount = 20;

	void OnCreate()
	{
		if (healAmount <= 0)
		{
			healAmount = 20;
		}
	}

	void OnUpdate()
	{
		BoxCollider col = entity.GetComponent<BoxCollider>();

		if (col != null && col.HasCollided)
		{
			Entity playerEntity = Entity.FindEntityByName("Player");
			if (playerEntity != null)
			{
				Health playerHealth = playerEntity.GetComponent<Health>();
				if (playerHealth != null)
				{
					if (playerHealth.actualHealth < playerHealth.maxHealth)
					{
						playerHealth.actualHealth += healAmount;

						if (playerHealth.actualHealth > playerHealth.maxHealth)
						{
							playerHealth.actualHealth = playerHealth.maxHealth;
						}

						Debug.Log("¡Curado! Nueva vida actual: " + playerHealth.actualHealth);

						entity.SetActive(false);
					}
				}
			}
		}
	}
};