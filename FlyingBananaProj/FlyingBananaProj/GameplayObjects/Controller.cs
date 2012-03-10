using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace FlyingBananaProj
{
    class Controller
    {
        #region Class Variables
        private KeyboardState _keyboardState;
        private GamePadState _gamepadState;
        Vector3 myPosition;
        Vector3 myRotation;
        Vector3 myVelocity;

        //Xbox 360 controls
        Buttons moveLeftPad = Buttons.LeftThumbstickLeft;
        Buttons moveRightPad = Buttons.LeftThumbstickRight;
        Buttons moveUpPad = Buttons.LeftThumbstickUp;
        Buttons moveDownPad = Buttons.LeftThumbstickDown;
        Buttons userPowerupPad = Buttons.X;
        Buttons switchWeapPad = Buttons.DPadRight;
        Buttons fireWeapPad = Buttons.RightTrigger;
        Buttons dodgeLeftPad = Buttons.LeftShoulder;
        Buttons dodgeRightPad = Buttons.RightShoulder;
        
        //PC controls
        Keys moveLeftKeyboard = Keys.Left;
        Keys moveRightKeyboard = Keys.Right;
        Keys moveUpKeyboard = Keys.Up;
        Keys moveDownKeyboard = Keys.Down;
        Keys powerupKeyboard = Keys.OemPlus;
        Keys usePowerupKeyboard = Keys.W;
        Keys switchWeapKeyboard = Keys.OemCloseBrackets;
        Keys fireWeapKeyboard = Keys.S;
        Keys dodgeLeftKeyboard = Keys.A;
        Keys dodgeRightKeyboard = Keys.D;

        bool locked;
        bool isBossMode;

		protected float velocityFactor = 2.0f;

        TimeSpan timeSincePowerUp = TimeSpan.FromSeconds(30);
        TimeSpan timeSinceSwitch = TimeSpan.FromSeconds(60);
        #endregion

        public Controller()
        {
            isBossMode = false;
        }
        public Vector3 UpdatePlayerRotation()
        {
            return myRotation;
        }
        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }
        public Vector3 UpdatePlayerMovement(Vector3 position, Vector3 rotation)
        {
            myVelocity = Vector3.Zero;
            _keyboardState = Keyboard.GetState();
            _gamepadState = GamePad.GetState(PlayerIndex.One);
            myPosition = position;
            myRotation = rotation;
            myVelocity += PlayerStrafeLeft();
            myVelocity += PlayerStrafeRight();
            myVelocity += PlayerMoveForwardOrUp();
            myVelocity += PlayerMoveBackwardOrDown();
            if (locked)
                return Vector3.Zero;
            return myVelocity;
        }

        #region Handling Input

        private Vector3 PlayerStrafeLeft()
        {
            if (!_keyboardState.IsKeyDown(moveLeftKeyboard) && !_gamepadState.IsButtonDown(moveLeftPad)) return Vector3.Zero;
            return Vector3.UnitX * velocityFactor;
        }
        private Vector3 PlayerStrafeRight()
        {
            if (!_keyboardState.IsKeyDown(moveRightKeyboard) && !_gamepadState.IsButtonDown(moveRightPad)) return Vector3.Zero;
            return -Vector3.UnitX * velocityFactor;
        }
        private Vector3 PlayerMoveForwardOrUp()
        {
            if (!_keyboardState.IsKeyDown(moveUpKeyboard) && !_gamepadState.IsButtonDown(moveUpPad)) return Vector3.Zero;
            if (isBossMode)
                return Vector3.UnitY * velocityFactor;
            else return Vector3.UnitZ * velocityFactor;
        }
        private Vector3 PlayerMoveBackwardOrDown()
        {
            if (!_keyboardState.IsKeyDown(moveDownKeyboard) && !_gamepadState.IsButtonDown(moveDownPad)) return Vector3.Zero;
            if (isBossMode)
                return -Vector3.UnitY * velocityFactor;
            else return -Vector3.UnitZ * velocityFactor;
        }
        public bool isPlayerFiring()
        {
            if ((!_keyboardState.IsKeyDown(fireWeapKeyboard) && !_gamepadState.IsButtonDown(fireWeapPad)) || locked) return false;
            return true;
        }
        public bool isPlayerDodgingLeft()
        {
            if (!_keyboardState.IsKeyDown(dodgeLeftKeyboard) && !_gamepadState.IsButtonDown(dodgeLeftPad)) return false;
            return true;
        }
        public bool isPlayerDodgingRight()
        {
            if (!_keyboardState.IsKeyDown(dodgeRightKeyboard) && !_gamepadState.IsButtonDown(dodgeRightPad)) return false;
            return true;
        }
        public bool isPlayerPowering(GameTime gameTime)
        {
            timeSincePowerUp = timeSincePowerUp.Subtract(gameTime.TotalGameTime);
            if ((!_keyboardState.IsKeyDown(powerupKeyboard)) || timeSincePowerUp.TotalSeconds > 0 || locked) return false;

            timeSincePowerUp = TimeSpan.FromSeconds(30);
            return true;
        }
        public bool isPlayerSwitching(GameTime gameTime)
        {
            timeSinceSwitch = timeSinceSwitch.Subtract(gameTime.TotalGameTime);
            if ((!_keyboardState.IsKeyDown(switchWeapKeyboard) && !_gamepadState.IsButtonDown(switchWeapPad)) || timeSinceSwitch.TotalSeconds > 0 || locked) return false;

            timeSinceSwitch = TimeSpan.FromSeconds(60);
            return true;
        }
        public bool isPlayerTurningLeft()
        {
            if ((!_keyboardState.IsKeyDown(Keys.A) || locked)) return false;
            return true;
        }
        public bool isPlayerTurningRight()
        {
            if ((!_keyboardState.IsKeyDown(Keys.D) || locked)) return false;
            return true;
        }
        public bool isPlayerUsingPowerup()
        {
            if ((!_keyboardState.IsKeyDown(usePowerupKeyboard) && !_gamepadState.IsButtonDown(userPowerupPad) )|| locked) return false;
            return true;
        }
        #endregion
        #region Accessors and Mutators
        public void setPlayerVelocity(float newVel)
        {
            velocityFactor = newVel;
        }
        public float getPlayerVelocity()
        {
            return velocityFactor;
        }
        public void changeToBossMode()
        {
            isBossMode = true;
        }
        #endregion
    }
}
