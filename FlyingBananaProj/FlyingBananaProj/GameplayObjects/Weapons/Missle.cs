using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class Missle : PlayerWeapon
    {
        protected int splashRadius;
        protected bool exploded;
        public Missle(ContentManager content)
        {
            projectile = false;
            name = PlayerWeaponName.Missile;
            this.content = content;
            fireTime = TimeSpan.FromSeconds(.7f);
        }
        public Missle(ContentManager content, Vector3 startPos, Vector3 velocity)
        {
            damage = 36;
            splashRadius = 50;
            exploded = false;
            currentLevel = 0;
            name = PlayerWeaponName.Missile;
            this.velocity = velocity;
            model = content.Load<Model>(@"models/playermissile");
            texture = content.Load<Texture2D>(@"textures/missletexture");
            position = startPos;
            this.content = content;
            fireSound = "fireNeedle";
            projectile = true;
            rotate(new Vector3(MathHelper.ToRadians(90), 0, 0));
        }

        #region Update and Draw
        public override void Update(GameTime gameTime, Vector3 playerPosition, Vector3 playerRotation)
        {
            if (projectile)
            {
                position += velocity;
                rotate(new Vector3(0, 0, MathHelper.ToRadians(2)));
                sphere = new BoundingSphere(position, 1.2f);
                worldMat = Matrix.CreateScale(0.2f) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
                checkBounds();
            }
            else
            {
                position = playerPosition;
                playerRot = playerRotation;
            }
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }
        #endregion
        public override void Collision(GameEntity target)
        {
            if (!(target is Explosion))
            {
                Die();
                Level.Add(new Explosion(damage, 20, 5, position, content, true));
                base.Collision(target);
            }
        }

        public override void fire()
        {
            switch (currentLevel)
            {
                case 0:
                    Level.Add(new Missle(content, new Vector3(position.X - 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Missle(content, new Vector3(position.X + 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 1:
                    fireTime = TimeSpan.FromSeconds(.5f);
                    Level.Add(new Missle(content, new Vector3(position.X - 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Missle(content, new Vector3(position.X + 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 2: //need to have greater splash damage
                    fireTime = TimeSpan.FromSeconds(.5f);
                    Level.Add(new Missle(content, new Vector3(position.X - 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Missle(content, new Vector3(position.X + 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 3:
                    fireTime = TimeSpan.FromSeconds(.5f);
                    Level.Add(new Missle(content, new Vector3(position.X - 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Missle(content, new Vector3(position.X + 4, position.Y, position.Z), new Vector3(0, 0, 2 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Missle(content, new Vector3(position.X - 4, position.Y, position.Z), new Vector3(-2 * (float)Math.Sin(45 * Math.PI / 180), 0, 2 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(45 * Math.PI / 180))));
                    Level.Add(new Missle(content, new Vector3(position.X + 4, position.Y, position.Z), new Vector3(2 * (float)Math.Sin(45 * Math.PI / 180), 0, 2 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(45 * Math.PI / 180))));
                    break;
            }
        }
    }
}
