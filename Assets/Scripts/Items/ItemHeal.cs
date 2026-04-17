using System;
using Loopie;

public class HealItem : Component
{
	public int healAmount = 20;
	private BoxCollider collision;

	void OnCreate()
	{
		collision = entity.GetComponent<BoxCollider>();
    }

	void OnUpdate()
	{
		if(collision == null || !collision.HasCollided) 
			return;
		Player.Instance.PlayerHealth.Heal(healAmount);
		entity.SetActive(false);
	}
};