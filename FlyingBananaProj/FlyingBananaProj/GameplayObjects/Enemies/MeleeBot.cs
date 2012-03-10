using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    enum MeleeBotState
    {
        Idle,
        Attacking
    }
    class MeleeBot : EnemiesAndPlayer
    {
        protected int velDirection;
        protected double timeToPauseSeconds = 1;
        protected double timeToGoBackSeconds = 0.3;
        protected double timeToChargeSeconds = 0.5;
        protected TimeSpan timeToPause;
        protected TimeSpan timeToGoBack;
        protected TimeSpan timeToCharge;
        protected MeleeBotState state;
        protected float distanceToGoBack;
        protected bool charging;
        protected Vector3 positionToCharge;
        protected float acceleration;
        public MeleeBot(ContentManager content, Vector3 position, Vector3 playerPosition)
        {
            this.position = position;
            velocity = playerPosition - position;
            velocity.Normalize();
            health = 20;
            Model = content.Load<Model>(@"models/meleebot");
            texture = content.Load<Texture2D>(@"textures/meleeBotTexture");
            name = RealName.MeleeBot;
            score = 30;
            state = MeleeBotState.Idle;
            if (new Random().Next(2) == 0)
            {
                velDirection = 1;                
            }
            else velDirection = -1;
            timeToGoBack = TimeSpan.FromSeconds(timeToGoBackSeconds);
            timeToPause = TimeSpan.FromSeconds(timeToPauseSeconds);
            timeToCharge = TimeSpan.FromSeconds(timeToChargeSeconds);
            distanceToGoBack = 20;
            charging = false;
            enabled = true;
            //rotate(new Vector3(0, -MathHelper.ToRadians(90), 0));
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            if (enabled)
            {
                Vector3 oldVel = velocity;
                oldVel.Normalize();
                if (Vector3.DistanceSquared(position, playerPosition) > 2500 && state == MeleeBotState.Idle)
                {
                    velocity = playerPosition - position;
                    velocity.Normalize();
                }
                else
                {
                    timeToPause = timeToPause.Subtract(gameTime.ElapsedGameTime);
                    if (timeToPause.TotalSeconds <= 0)
                    {
                        state = MeleeBotState.Attacking;

                        if (!charging)
                            velocity = Vector3.Zero;

                        timeToGoBack = timeToGoBack.Subtract(gameTime.ElapsedGameTime);

                        if (timeToGoBack.TotalSeconds <= 0)
                        {
                            timeToCharge = timeToCharge.Subtract(gameTime.ElapsedGameTime);
                            if (!charging)
                            {
                                velocity = position - playerPosition;
                                velocity.Normalize();
                            }

                            if (timeToCharge.TotalSeconds <= 0)
                            {
                                if (!charging)
                                {
                                    acceleration = 1;
                                    charging = true;
                                    positionToCharge = playerPosition;
                                }
                                else //final charge
                                {
                                    velocity = positionToCharge - position;
                                    velocity.Normalize();
                                    velocity *= acceleration;
                                    acceleration *= 0.994f;
                                    if (Vector3.Distance(position, positionToCharge) < 4) //done charging
                                    {
                                        state = MeleeBotState.Idle;
                                        timeToGoBack = TimeSpan.FromSeconds(timeToGoBackSeconds);
                                        timeToPause = TimeSpan.FromSeconds(timeToPauseSeconds);
                                        timeToCharge = TimeSpan.FromSeconds(timeToChargeSeconds);
                                        charging = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        velocity = Vector3.Cross(playerPosition - position, new Vector3(0, 1, 0));
                        velocity.Normalize();
                        velocity *= 0.5f;
                        velocity *= velDirection;
                        if (position.Z <= playerPosition.Z && velocity.Z < 0)
                        {
                            velDirection = 0 - velDirection;
                            velocity *= velDirection;
                        }
                    }
                }

                position += velocity;
                velocity.Normalize();
                Vector3 newDir = playerPosition - position;
                newDir.Normalize();

                double angle = MathHelper.ToRadians(180) - Math.Atan2(newDir.Z, newDir.X);
                if (!charging)
                    rotate(new Vector3(0, (float)angle - (float)rotation.Y, 0));
            }
            else
            {
                position += velocity;
            }

            sphere = new BoundingSphere(position, 5);

            worldMat = Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(rotation.Y - MathHelper.ToRadians(90)) * Matrix.CreateTranslation(position);
            checkBounds();
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player && state == MeleeBotState.Attacking)
            {
                velocity = Vector3.Zero - velocity;
                state = MeleeBotState.Idle;
                timeToGoBack = TimeSpan.FromSeconds(timeToGoBackSeconds);
                timeToPause = TimeSpan.FromSeconds(timeToPauseSeconds);
                timeToCharge = TimeSpan.FromSeconds(timeToChargeSeconds);
                charging = false;
                Player p = target as Player;
                p.takeDamage(damage);
            }
            else if (target is PlayerWeapon)
            {
                target.Collision(this);
            }
            else if (target is Explosion)
            {
                target.Collision(this);
            }
            else if (target is MeleeBot)
            {
            }
        }
    }
}
