using System;
using System.Collections;
using Loopie;

public class BurnableBlock : Component
{
	private BoxCollider myCollider;
	private bool isPlayerTriggerTouching = false;
	private bool isBurning = false;

	void OnCreate()
	{
		myCollider = entity.GetComponent<BoxCollider>();

	}

	void OnUpdate()
	{
		if (myCollider == null || isBurning) return;

		if (myCollider.HasCollided)
		{
			isPlayerTriggerTouching = true;
		}
		else if (myCollider.HasEndedCollision)
		{
			isPlayerTriggerTouching = false;
		}

		if (isPlayerTriggerTouching && Input.IsKeyPressed(KeyCode.O) && DatabaseRegistry.playerDB.Player.hasBurner)
		{
			StartCoroutine(BurnSequence());
		}
	}

	private IEnumerator BurnSequence()
	{
		isBurning = true;

		float timer = 0.0f;
		while (timer < 1.5f)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		entity.SetActive(false);
	}
}