using System;

namespace Loopie
{
    public class PlayerMovement : Component
    {
        public float speed = 10.0f;
        public float rotSpeed = 5.0f;

        //Dash Settings
        public float dashSpeed = 40.0f;
        public float dashDuration = 0.2f;
        private float dashTimer = 0.0f;
        private Vector3 dashDirection = new Vector3(0, 0, 0);
        private bool tempDashOnce = false;
        private bool wasSpacePressed = false;

        //CamFollow
        public Vector3 camExtTransform;
        public Entity camEntity;

        public PlayerMovement() { }

        public void OnCreate() { }

        public void OnUpdate()
        {

            HandleDash();
            HandleNormalMovement();
        }

        private bool HandleDash()
        {
            //TODO: gamepad input + input once
            if (dashTimer > 0)
            {
                entity.transform.position += dashDirection * dashSpeed * Time.deltaTime;
                dashTimer -= Time.deltaTime;

                return true;
            }

            bool isSpacePressed = Input.IsKeyPressed(KeyCode.SPACE);

            if (isSpacePressed && !wasSpacePressed)
            {
                dashTimer = dashDuration;
                dashDirection = entity.transform.Forward;
            }

            wasSpacePressed = isSpacePressed;

            return false;
        }

        private void HandleNormalMovement()
        {
            //TODO: joystick controllers
            Vector3 moveDirection = new Vector3(0, 0, 0);
            bool isMoving = false;

            if (Input.IsKeyPressed(KeyCode.W) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_UP))
            {
                moveDirection += new Vector3(0, 0, 1);
                isMoving = true;
            }

            if (Input.IsKeyPressed(KeyCode.S) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_DOWN))
            {
                moveDirection += new Vector3(0, 0, -1);
                isMoving = true;
            }

            if (Input.IsKeyPressed(KeyCode.A) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_LEFT))
            {
                moveDirection += new Vector3(-1, 0, 0);
                isMoving = true;
            }

            if (Input.IsKeyPressed(KeyCode.D) || Input.IsGamepadButtonPressed(GamepadButton.GAMEPAD_DPAD_RIGHT))
            {
                moveDirection += new Vector3(1, 0, 0);
                isMoving = true;
            }

            if (isMoving)
            {
                entity.transform.position += moveDirection * Time.deltaTime * speed;

                Vector3 targetLookAt = entity.transform.position + moveDirection;
                entity.transform.LookAt(targetLookAt, new Vector3(0, 1, 0));
            }
        }
    }
}
