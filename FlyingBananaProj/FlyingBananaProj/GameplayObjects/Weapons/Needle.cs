using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class Needle : PlayerWeapon
    {
        public Needle(ContentManager content)
        {
            projectile = false;
            name = PlayerWeaponName.Needle;
            this.content = content;
            fireSound = "fireNeedle";
            fireTime = TimeSpan.FromSeconds(0.15f);
            avc = new AudioVideoController();
        }
        public Needle(ContentManager content, Vector3 startPos, Vector3 velocity)
        {
            damage = 9;
            currentLevel = 0;
            name = PlayerWeaponName.Needle;
            this.velocity = velocity;
            model = content.Load<Model>(@"models/bullet");
            texture = content.Load<Texture2D>(@"textures/missletexture");
            position = startPos;

            projectile = true;
        }
        #region Update and Draw
        public override void Update(GameTime gameTime, Vector3 playerPosition, Vector3 playerRotation)
        {
            if (projectile)
            {
                position += velocity;
                sphere = new BoundingSphere(position, 1.2f);
                worldMat = Matrix.CreateScale(0.3f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
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
            base.Collision(target);
        }
        public override void fire()
        {
            switch (currentLevel)
            {
                case 0:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 1:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(-8 * (float)Math.Sin(30 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(30 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(8 * (float)Math.Sin(30 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(30 * Math.PI / 180))));
                    break;
                case 2:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(-8 * (float)Math.Sin(30 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(30 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(8 * (float)Math.Sin(30 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(30 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(-8 * (float)Math.Sin(45 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(45 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(8 * (float)Math.Sin(45 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(45 * Math.PI / 180))));
                    break;
                case 3:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(-8 * (float)Math.Sin(30 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(30 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(8 * (float)Math.Sin(30 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(30 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(-8 * (float)Math.Sin(45 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(45 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(8 * (float)Math.Sin(45 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(45 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X + 1.6f, position.Y, position.Z), new Vector3(-8 * (float)Math.Sin(60 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(60 * Math.PI / 180))));
                    Level.Add(new Needle(content, new Vector3(position.X - 1.6f, position.Y, position.Z), new Vector3(8 * (float)Math.Sin(60 * Math.PI / 180), 0, 8 * (float)Math.Cos(playerRot.Y) * (float)Math.Cos(60 * Math.PI / 180))));
                    break;
            }
        }
    }

}
