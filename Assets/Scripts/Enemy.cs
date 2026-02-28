using System;

namespace Loopie
{
    public class Enemy : Component
    {
        public float speed = 4.0f;
        public string targetEntityName = "BrightWishker";

        private Entity playerEntity;

        public Enemy() { }

        public void OnCreate()
        {
            playerEntity = Entity.FindEntityByName(targetEntityName);
        }

        public void OnUpdate()
        {
            if (playerEntity == null)
            {
                playerEntity = Entity.FindEntityByName(targetEntityName);
                if (playerEntity == null) return;
            }

            Vector3 currentPos = entity.transform.position;
            Vector3 targetPos = playerEntity.transform.position;

            Vector3 direction = new Vector3(
                targetPos.x - currentPos.x,
                targetPos.y - currentPos.y,
                targetPos.z - currentPos.z
            );

            float distance = (float)Math.Sqrt((direction.x * direction.x) + (direction.y * direction.y) + (direction.z * direction.z));

            if (distance > 0.1f)
            {
                direction.x /= distance;
                direction.y /= distance;
                direction.z /= distance;

                entity.transform.position += direction * speed * Time.deltaTime;

                entity.transform.LookAt(targetPos, new Vector3(0, 1, 0));
            }
        }
    }
}


