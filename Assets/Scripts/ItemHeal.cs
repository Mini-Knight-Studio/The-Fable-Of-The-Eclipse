using System;
using Loopie;

public class HealItem : Component
{
	public int healAmount = 20;
	private BoxCollider collision;
	private Player player;

	void OnCreate()
	{
		collision = entity.GetComponent<BoxCollider>();
		player = Entity.FindEntityByName("Player").GetComponent<Player>();
    }

	void OnUpdate()
	{
		if(collision == null || !collision.HasCollided || player == null) return;
		player.PlayerHealth.Heal(healAmount);
		entity.SetActive(false);
	}
};