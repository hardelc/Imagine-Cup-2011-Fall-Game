using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class LatchingCell : EnemiesAndPlayer
    {
        protected TimeSpan latchDuration;
        protected bool latched;
        GameEntity victim; //the latchee
        public LatchingCell(ContentManager content, Vector3 position, Vector3 playerPosition)
        {
            this.position = position;
            velocity = playerPosition - position;
            velocity.Normalize();
            health = 20;
            Model = content.Load<Model>(@"models/latchingcell");
            texture = content.Load<Texture2D>(@"models/latchingCellTexture");
            name = RealName.LatchingCell;
            score = 15;
            latched = false;
            latchDuration = TimeSpan.FromSeconds(5); //latches for five seconds
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            if (latched)
            {
                latchDuration = latchDuration.Subtract(gameTime.ElapsedGameTime);
                if (latchDuration.Seconds <= 0) // releasing
                {
                    Die();
                    if (victim is Player)
                    {
                        Player p = victim as Player;
                        p.setVelocity(p.getVelocity() / .25f);
                        velocity = p.Velocity;
                    }
                    else
                    {
                        Shield s = victim as Shield;
                        s.VelocityFactor = s.VelocityFactor * 4;
                    }
                }
            }
            else //keeping homing for player
            {
                velocity = playerPosition - position;
                velocity.Normalize();
                velocity /= 2;
            }

            position += velocity;
            sphere = new BoundingSphere(position, 2);
            worldMat = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
            checkBounds();
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player || target is Shield)
            {
                if (!latched)
                {
                    latched = true;
                    if (target is Player)
                    {
                        Player p = target as Player;
                        p.setVelocity(p.getVelocity() * .25f);
                        velocity = p.Velocity;
                        victim = p;
                    }
                    else
                    {
                        Shield s = target as Shield;
                        s.VelocityFactor = s.VelocityFactor / 4;
                        victim = s;
                    }
                }
                else
                {
                    //position = target.Position;
                    velocity = target.Velocity;
                }
            }
            else if (target is PlayerWeapon && !(target is Shield))
            {
                if (!latched)
                    target.Collision(this);
            }
            else if (target is Explosion)
            {
                target.Collision(this);
            }
            else //not player, shield, OR any other weapon. Handle physics.
            {

            }
        }
    }
}
