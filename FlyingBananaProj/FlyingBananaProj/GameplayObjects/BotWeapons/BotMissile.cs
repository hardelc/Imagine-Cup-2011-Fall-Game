using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class BotMissile : BotWeapon
    {
        protected int splashRadius;
        protected bool exploded;
        protected Vector3 playerPosition;
        public BotMissile(ContentManager content)
        {
            projectile = false;
            name = BotWeaponName.Missile;
            this.content = content;
        }
        public BotMissile(ContentManager content, Vector3 startPos, Vector3 velocity)
        {
            damage = 1;
            splashRadius = 50;
            exploded = false;
            name = BotWeaponName.Missile;
            this.velocity = velocity;
            this.rotation = rotation;
            model = content.Load<Model>(@"models/enemymissile");
            texture = content.Load<Texture2D>(@"textures/missletexture");
            position = startPos;
            this.content = content;
            fireSound = content.Load<SoundEffect>(@"audio/sfx/fireNeedle");
            projectile = true;

            Vector3 velNormal = velocity;
            velNormal.Normalize();
            Vector3 up = Vector3.UnitZ;
            double theta = Math.Acos(Vector3.Dot(velNormal, up));
            if (position.X < position.X + velNormal.X)
                theta = 0 - theta;
            rotate(new Vector3(MathHelper.ToRadians(90), -(float)theta, 0));
        }

        #region Update and Draw
        public override void Update(GameTime gameTime, Vector3 botPosition, Vector3 playerPosition)
        {
            if (projectile)
            {
                position += velocity;
                sphere = new BoundingSphere(position, 3);
                worldMat = Matrix.CreateScale(0.15f) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
                checkBounds();
            }
            else
            {
                position = botPosition;
                this.playerPosition = playerPosition;
            }
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }
        #endregion
        public override void Collision(GameEntity target)
        {
            if (!(target is BotExplosion))
            {
                Die();
                Level.Add(new BotExplosion(damage, 50, 5, position, content));
                base.Collision(target);
            }
        }

        public override void fire()
        {
            Vector3 fireDirection = playerPosition - position;
            fireDirection.Normalize();
            fireDirection *= 2;
            Level.Add(new BotMissile(content, new Vector3(position.X, position.Y, position.Z), fireDirection));
        }
    }

}
