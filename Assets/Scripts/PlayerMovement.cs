using System;
using Loopie;

class PlayerMovement : Component
{
    public float speed = 10;
    public float rotSpeed = 5;
    public PlayerMovement()
    {

    }
 
    public void OnCreate()
    {
    }

    public void OnUpdate()
    {
        if (Input.IsKeyPressed(KeyCode.W))
    	{
    		entity.transform.position += transform.Forward * Time.deltaTime * speed;
    	}
    	
		if (Input.IsKeyPressed(KeyCode.S))         
		{
			entity.transform.position += transform.Back * Time.deltaTime * speed;
		}
		
		if (Input.IsKeyPressed(KeyCode.A))
		{
			Vector3 rot = new Vector3(0, -1, 0);
			entity.transform.Rotate(rot * Time.deltaTime * rotSpeed, Transform.Space.LocalSpace);
		}
		
		if (Input.IsKeyPressed(KeyCode.D))
		{
			Vector3 rot = new Vector3(0, 1, 0);
			entity.transform.Rotate(rot * Time.deltaTime * rotSpeed, Transform.Space.LocalSpace);
		}
	}
}



