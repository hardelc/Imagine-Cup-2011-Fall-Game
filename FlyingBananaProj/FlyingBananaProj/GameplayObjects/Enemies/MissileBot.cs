using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    enum MissileBotState
    {
        Idle,
        Attacking
    }
    class MissileBot : EnemiesAndPlayer
    {
        protected MeleeBotState state;
        protected BotWeapon weapon;
        TimeSpan timeToFire;
        public MissileBot(ContentManager content, Vector3 position, Vector3 playerPosition)
        {
            this.position = position;
            velocity = playerPosition - position;
            velocity.Normalize();
            health = 30;
            Model = content.Load<Model>(@"models/missilebot");
            texture = content.Load<Texture2D>(@"models/rocketBotTexture");
            name = RealName.MeleeBot;
            score = 30;
            state = MeleeBotState.Idle;
            timeToFire = TimeSpan.FromSeconds(2);
            weapon = new BotMissile(content);
            enabled = true;
            rotate(new Vector3(0, -MathHelper.ToRadians(90), 0));
        }

        public void fireMissile()
        {
            weapon.fire();
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            if (enabled)
            {
                weapon.Update(gameTime, position, playerPosition);
                timeToFire = timeToFire.Subtract(gameTime.ElapsedGameTime);
                if (timeToFire.TotalSeconds <= 0)
                {
                    timeToFire = TimeSpan.FromSeconds(2);
                    fireMissile();
                }

                Vector3 oldVel = velocity;
                oldVel.Normalize();

                if (Math.Abs(Vector3.DistanceSquared(position, playerPosition) - 1600) < 90)
                {
                    velocity = Vector3.Zero;
                }
                else if (Vector3.DistanceSquared(position, playerPosition) > 1600)
                {
                    velocity = playerPosition - position;
                    velocity.Normalize();
                }
                else if (Vector3.DistanceSquared(position, playerPosition) < 1600)
                {
                    velocity = position - playerPosition;
                    velocity.Normalize();
                }
            }

            position += velocity;

            Vector3 newDir = playerPosition - position;
            newDir.Normalize();

            double angle = MathHelper.ToRadians(180) - Math.Atan2(newDir.Z, newDir.X);
            
            rotate(new Vector3(0, (float)angle - (float)rotation.Y, 0));

            sphere = new BoundingSphere(position, 2);
            worldMat = Matrix.CreateScale(0.4f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
            checkBounds();
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player)
            {
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
