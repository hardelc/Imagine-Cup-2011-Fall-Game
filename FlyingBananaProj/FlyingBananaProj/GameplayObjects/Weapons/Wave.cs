using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class Wave : PlayerWeapon
    {
        BoundingBox box;
        public Wave(ContentManager content)
        {
            projectile = false;
            name = PlayerWeaponName.Wave;
            this.content = content;
            fireTime = TimeSpan.FromSeconds(.7f);
        }
        public Wave(ContentManager content, Vector3 startPos, Vector3 velocity)
        {
            damage = 20;
            currentLevel = 0;
            name = PlayerWeaponName.Wave;
            this.velocity = velocity;
            model = content.Load<Model>(@"models/maincharacter");
            texture = content.Load<Texture2D>(@"textures/missletexture");
            position = startPos;
            this.content = content;
            fireSound = "fireNeedle";
            projectile = true;
        }

        #region Update and Draw
        public override void Update(GameTime gameTime, Vector3 playerPosition, Vector3 playerRotation)
        {
            if (projectile)
            {
                position += velocity;
                sphere = new BoundingSphere(position, 3);
                worldMat = Matrix.CreateScale(0.075f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
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

        }

        public override void fire()
        {
            switch (currentLevel)
            {
                case 0:
                    Level.Add(new Wave(content, new Vector3(position.X - 10, position.Y, position.Z), new Vector3(0, 0, 3)));
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
            }
        }
    }

}
