using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class Player : EnemiesAndPlayer
    {
        #region Class Variables
        public static string[] weapList = { "Needle", "Missile", "Shield" };
        public Controller controls;
        Vector3 CameraStaticPosition;
        protected bool cameraEnabled;
        protected int shield;
        protected int powerup;
        protected PlayerWeapon currentWeapon;
        protected ContentManager content;
        protected TimeSpan currFireTime;
        protected bool initialLock;
        protected bool bossMode;
        protected bool switchedFromShield;
        protected AudioVideoController playerAVC;
        protected Vector3 bossCenter; //for boss of level 1
        protected Vector3 oldPos;

        protected double dodgeTimeSeconds = 0.3;
        protected TimeSpan dodgeTime;

        protected double betweenDodgeSeconds = 0.1;
        protected TimeSpan betweenDodge;

        protected bool pressedDodgeLeft;
        protected bool pressedDodgeRight;
        protected bool isInvincible;
        protected bool isPoweredUp;

        protected double flickerTimeSeconds = 0.15;
        protected TimeSpan flickerTime;

        protected double invincibleTimeSeconds = 3;
        protected TimeSpan invincibleTime;

        protected double powerupTimeSeconds = 5;
        protected TimeSpan powerUpTime;

        protected double pressedPowerUpTimeSeconds = 1;
        protected TimeSpan pressedPowerUpTime;

        #endregion

        public Player(ContentManager content)
        {
            position = new Vector3(0, 0, -250);
            CameraStaticPosition = new Vector3(0, 0, 0);
            this.content = content;
            model = content.Load<Model>(@"models/maincharacter");
            texture = content.Load<Texture2D>(@"models/characterTextureBody");
            controls = new Controller();
            initialLock = true;
            controls.Locked = true;
            shield = 3;
            powerup = 0;
            currentWeapon = new Needle(content);
            oldPos = Vector3.Zero;
            pressedDodgeLeft = false;
            pressedDodgeRight = false;
            isInvincible = false;
            currFireTime = new TimeSpan(currentWeapon.FireTime.Ticks);
            dodgeTime = TimeSpan.FromSeconds(dodgeTimeSeconds);
            betweenDodge = TimeSpan.FromSeconds(betweenDodgeSeconds);
            invincibleTime = TimeSpan.FromSeconds(invincibleTimeSeconds);
            flickerTime = TimeSpan.FromSeconds(flickerTimeSeconds);
            powerUpTime = TimeSpan.FromSeconds(powerupTimeSeconds);
            pressedPowerUpTime = TimeSpan.FromSeconds(pressedPowerUpTimeSeconds);
            playerAVC = new AudioVideoController();
            cameraEnabled = true;
        }

        public void setCameraStaticPosition(Vector3 csp)
        {
            CameraStaticPosition = csp;
            bossMode = false;
        }

        public bool CameraEnabled
        {
            get { return cameraEnabled; }
            set { cameraEnabled = value; }
        }

        public void shieldDamage()
        {
            if (!isInvincible)
            {
                shield--;
                isInvincible = true;
                invincibleTime = TimeSpan.FromSeconds(invincibleTimeSeconds);
            }

            if (shield == 0 && Game1.debug)
                shield = 3; //invincibility!
        }

        public void AddPowerup(int value)
        {
            if (powerup < 1182 && powerup + value <= 1182)
                powerup += value;
            else if (powerup + value > 1182)
                powerup = 1182;
        }

        public void setShield(int value)
        {
            if (value <= 3)
                shield = value;
        }

        public void resetScore()
        {
            score = 0;
        }

        public void addScore(int value)
        {
            score += value;
        }

        /// <summary>
        ///  Return player statistics
        /// </summary>
        public int getPowerup()
        {
            return powerup;
        }

        public int getShields()
        {
            return shield;
        }

        public int getScore()
        {
            return score;
        }

        public bool SwitchedFromShield
        {
            get { return switchedFromShield; }
            set { switchedFromShield = value; }
        }

        public bool InitialLock
        {
            get { return initialLock; }
        }

        public void takeDamage()
        {
            shieldDamage();
        }

        #region Update and Draw
        public override void Update(GameTime gameTime)
        {
            currentWeapon.Update(gameTime, position, rotation);
            if (currentWeapon is Shield)
            {
                Shield s = currentWeapon as Shield;
                s.UpdateWeapFirePosition(Position, Rotation);
            }
            currFireTime = currFireTime.Subtract(gameTime.ElapsedGameTime);
            betweenDodge = betweenDodge.Subtract(gameTime.ElapsedGameTime);
            invincibleTime = invincibleTime.Subtract(gameTime.ElapsedGameTime);
            pressedPowerUpTime = pressedPowerUpTime.Subtract(gameTime.ElapsedGameTime);
            if (isInvincible)
            {
                flickerTime = flickerTime.Subtract(gameTime.ElapsedGameTime);
            }
            if (invincibleTime.TotalSeconds <= 0)
            {
                isInvincible = false;
            }
            if (isPoweredUp)
            {
                powerUpTime = powerUpTime.Subtract(gameTime.ElapsedGameTime);
            }
            if (powerUpTime.TotalSeconds <= 0)
            {
                isPoweredUp = false;
                currentWeapon.CurrentLevel = 0;
            }
            if (cameraEnabled)
            {
                if (!bossMode)
                {
                    Camera.Instance.UpdateCamera(CameraStaticPosition, Vector3.Zero);
                }
                else
                {
                    Camera.Instance.UpdateCamera(position, Vector3.UnitY * rotation.Y);
                }
            }
            sphere = new BoundingSphere(position, 6);
            worldMat = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);

            if (!controls.Locked)
            {
                velocity = controls.UpdatePlayerMovement(position, rotation);
            }
            else
            {
                if (initialLock)
                {
                    velocity = new Vector3(0, 0, 1.3f);
                    if (position.Z > 0)
                    {
                        // controls.Locked = false;
                        velocity = Vector3.Zero;
                        initialLock = false;
                    }
                }
            }

            if (velocity.X != 0.0f && velocity.Z != 0.0f)
            {
                velocity /= (float)Math.Sqrt(2); // don't want diagonal speed to be faster
            }

            if (bossMode)
                oldPos = position;

            if (bossMode && !controls.Locked) //move around the boss when going left/right
            {
                if (velocity.X != 0)
                {
                    velocity = getRotationVelocityAroundPoint(velocity, bossCenter);
                }
            }
            #region Player dodge logic
            if (controls.isPlayerDodgingLeft())
            {
                pressedDodgeLeft = true;
                pressedDodgeRight = false;
            }
            if (controls.isPlayerDodgingRight())
            {
                pressedDodgeRight = true;
                pressedDodgeLeft = false;
            }

            if (pressedDodgeLeft && !pressedDodgeRight)
            {
                if (betweenDodge.TotalSeconds <= 0)
                {
                    dodgeTime = dodgeTime.Subtract(gameTime.ElapsedGameTime);
                    if (dodgeTime.TotalSeconds > 0)
                    {
                        velocity = Vector3.UnitX * 5;
                    }
                    else
                    {
                        pressedDodgeLeft = false;
                        dodgeTime = TimeSpan.FromSeconds(dodgeTimeSeconds);
                        betweenDodge = TimeSpan.FromSeconds(betweenDodgeSeconds);
                    }
                }
            }

            if (pressedDodgeRight && !pressedDodgeLeft)
            {
                if (betweenDodge.TotalSeconds <= 0)
                {
                    dodgeTime = dodgeTime.Subtract(gameTime.ElapsedGameTime);
                    if (dodgeTime.TotalSeconds > 0)
                    {
                        velocity = -Vector3.UnitX * 5;
                    }
                    else
                    {
                        pressedDodgeRight = false;
                        dodgeTime = TimeSpan.FromSeconds(dodgeTimeSeconds);
                        betweenDodge = TimeSpan.FromSeconds(betweenDodgeSeconds);
                    }
                }
            }
            #endregion

            position += velocity;
            #region Player rotation while moving
            if (velocity.X < 0)
            {
                if (rotation.Z == 0 || rotation.Z > MathHelper.ToRadians(180))
                {
                    rotate(new Vector3(0, 0, MathHelper.ToRadians(3)));
                }
                else
                {
                    if (rotation.Z > 0 && rotation.Z <= MathHelper.ToRadians(180))
                    {
                        if (rotation.Z < MathHelper.ToRadians(30))
                            rotate(new Vector3(0, 0, MathHelper.ToRadians(3)));
                    }
                }
            }
            else if (velocity.X > 0)
            {
                if (rotation.Z == 0 || rotation.Z > 0 && rotation.Z <= MathHelper.ToRadians(180))
                {
                    rotate(new Vector3(0, 0, -MathHelper.ToRadians(3)));
                }
                else
                {
                    if (rotation.Z > MathHelper.ToRadians(180))
                    {
                        if (rotation.Z > MathHelper.ToRadians(330))
                            rotate(new Vector3(0, 0, -MathHelper.ToRadians(3)));
                    }
                }
            }
            else
            {
                if (rotation.Z > 0 && rotation.Z <= MathHelper.ToRadians(180))
                {
                    rotate(new Vector3(0, 0, -MathHelper.ToRadians(3)));
                }
                else if (rotation.Z > MathHelper.ToRadians(180))
                {
                    rotate(new Vector3(0, 0, MathHelper.ToRadians(3)));
                }
                if (MathHelper.ToDegrees(rotation.Z) < 3)
                {
                    rotation.Z = 0;
                }
            }
            #endregion

            if (position.X > 60) position.X = 60;
            if (position.X < -60) position.X = -60;

            if (!controls.Locked)
            {
                if (position.Z < -40) position.Z = -40;
                if (position.Z > 40) position.Z = 40;
            }

            if (controls.isPlayerFiring())
            {
                //is the player allowed to fire again since his last fire?
                if (currFireTime.TotalSeconds <= 0)
                {
                    //Reset current time
                    currFireTime = TimeSpan.FromTicks(currentWeapon.FireTime.Ticks);
                    fire();
                }
            }

            if (controls.isPlayerUsingPowerup())
            {
                if (pressedPowerUpTime.TotalSeconds <= 0)
                {
                    if (!isPoweredUp)
                    {
                        if (powerup < 394)
                        {
                            playerAVC.playSoundEffect("cancel", 1);
                        }
                        else
                        {
                            isPoweredUp = true;
                            powerUpTime = TimeSpan.FromSeconds(powerupTimeSeconds);
                            if (powerup >= 394 && powerup < 788)
                            {
                                currentWeapon.CurrentLevel = 1;
                                powerup -= 394;
                            }
                            else if (powerup >= 788 && powerup < 1182)
                            {
                                currentWeapon.CurrentLevel = 2;
                                powerup -= 788;
                            }
                            else
                            {
                                currentWeapon.CurrentLevel = 3;
                                powerup -= 1182;
                            }
                        }
                    }
                    pressedPowerUpTime = TimeSpan.FromSeconds(pressedPowerUpTimeSeconds);
                }
            }
            if (Game1.debug)
            {
                if (controls.isPlayerPowering(gameTime))
                {
                    if (currentWeapon.CurrentLevel < 3)
                        currentWeapon.CurrentLevel++;
                    else currentWeapon.CurrentLevel = 0;
                }

                if (controls.isPlayerSwitching(gameTime))
                {
                    int loc = Array.IndexOf(weapList, currentWeapon.Name.ToString());
                    if (loc < weapList.Length - 1)
                    {
                        changeWeapon((PlayerWeaponName)Enum.Parse(typeof(PlayerWeaponName), weapList[loc + 1], true));
                    }
                    else
                    {
                        changeWeapon((PlayerWeaponName)Enum.Parse(typeof(PlayerWeaponName), weapList[0], true));
                    }
                }
            }
        }

        public override void Draw()
        {
            if (!isInvincible)
            {
                DrawModel(Model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
            }
            else
            {
                if (flickerTime.TotalSeconds <= 0)
                {
                    DrawModel(Model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
                    flickerTime = TimeSpan.FromSeconds(flickerTimeSeconds);
                }
            }
        }
        #endregion

        #region Accessors and Mutators
        public int Shield
        {
            get { return Shield;}
            set {Shield = value;}
        }
        public void setVelocity(float newVel)
        {
            controls.setPlayerVelocity(newVel);
        }
        public float getVelocity()
        {
            return controls.getPlayerVelocity();
        }
        public PlayerWeapon Weapon
        {
            get { return currentWeapon; }
        }
        public void changeToBossMode(Vector3 center)
        {
            bossMode = true;
            bossCenter = center;
            controls.changeToBossMode();
        }
        public void changeToNormalMode()
        {
            bossMode = false;
        }
        #endregion

        #region PlayerWeapon Handling
        public void fire()
        {
            currentWeapon.fire();
        }

        public void changeWeapon(PlayerWeaponName name)
        {
            if (currentWeapon.Name == name)
            {
                return;
            }
            if (currentWeapon is Shield)
            {
                switchedFromShield = true;
            }
            int currentLevel = currentWeapon.CurrentLevel;
            switch (name)
            {
                case PlayerWeaponName.Needle:
                    currentWeapon = new Needle(content);
                    break;
                case PlayerWeaponName.Missile:
                    currentWeapon = new Missle(content);
                    break;
                case PlayerWeaponName.Shield:
                    currentWeapon = new Shield(content, new Vector3(position.X - 400, position.Y + 300, position.Z - 12), new Vector3(position.X - 4, position.Y, position.Z + 4));
                    Level.Add(new Shield(content, new Vector3(position.X - 4, position.Y, position.Z + 10), new Vector3(position.X - 8, position.Y, position.Z + 10)));
                    Level.Add(new Shield(content, new Vector3(position.X + 4, position.Y, position.Z + 10), new Vector3(position.X + 8, position.Y, position.Z + 10)));
                    break;
            }
            currentWeapon.CurrentLevel = currentLevel;
        }
        #endregion
        public override void Collision(GameEntity target)
        {
            if (!(target is PlayerWeapon) && !(target is Explosion))
            {
                target.Collision(this);
            }
        }
        public Vector3 getRotationVelocityAroundPoint(Vector3 vel, Vector3 center)
        {
            Vector3 newVel = vel;
            Vector3 centerY = new Vector3(center.X, position.Y, center.Z);
            float radius = Vector3.Distance(center, position);
            float circumference = 2 * (float)Math.PI * radius;
            Vector3 diffVect = position - center;
            Vector3 rotatedVect = Vector3.Transform(diffVect, Matrix.CreateRotationY(MathHelper.ToRadians(1)));
            rotatedVect += center;
            //player is currently rotating too fast with respect to the ship's revolution around the boss.
            if (newVel.X < 0)
            {
                rotate(new Vector3(0, (float)Math.Asin(1 / radius), 0));
                //velocity = Vector3.Zero;
                newVel = Vector3.Cross(position - center, new Vector3(0, 1, 0));
            }
            else
            {
                rotate(new Vector3(0, (float)Math.Asin(-1 / radius), 0));
                //velocity = Vector3.Zero;
                newVel = -Vector3.Cross(position - center, new Vector3(0, 1, 0));
            }
            newVel.Normalize();
            return newVel;
        }
    }
}