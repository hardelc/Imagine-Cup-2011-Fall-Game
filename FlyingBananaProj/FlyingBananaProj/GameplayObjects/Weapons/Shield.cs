using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace FlyingBananaProj
{
    class Shield : PlayerWeapon
    {
        Vector3 restPos;
        Vector3 altRestPos;
        float velocityFactor;
        Vector3 weapFirePos;

        public Shield(ContentManager content, Vector3 restPos, Vector3 altRestPos)
        {
            damage = 9;
            fireTime = TimeSpan.FromSeconds(.15f);
            currentLevel = 0;
            name = PlayerWeaponName.Shield;
            this.content = content;
            model = content.Load<Model>(@"models/shield1");
            texture = content.Load<Texture2D>(@"textures/needletexture");
            position = restPos;
            this.restPos = restPos;
            this.altRestPos = altRestPos;
            fireSound = "fireNeedle";
            velocityFactor = 1;
            avc = new AudioVideoController();
        }
        #region Update and Draw

        public void UpdateWeapFirePosition(Vector3 playerPosition, Vector3 playerRotation)
        {
            weapFirePos = playerPosition;
            playerRot = playerRotation;
        }

        public void Update(GameTime gameTime, Vector3 playerVel, bool firing)
        {                
                restPos += playerVel;
                altRestPos += playerVel;
                Vector3 homePos = (firing) ? altRestPos : restPos;
                if (position != homePos)
                {
                    Vector3 v = homePos - position;
                    v /= 5;
                    v *= velocityFactor;
                    velocity = v;
                    position += velocity;
                }
                sphere = new BoundingSphere(position, 1.2f);
                worldMat = Matrix.CreateScale(0.04f) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
                checkBounds();
        }

        public float VelocityFactor
        {
            get { return velocityFactor; }
            set { velocityFactor = value; }
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }
        #endregion
        public override void Collision(GameEntity target)
        {
            if (target is LatchingCell)
            {
                target.Collision(this);
            }
            //base.Collision(target);
        }

        public override void fire()
        {
            switch (currentLevel)
            {                  
                case 0:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X - 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X + 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 1:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X - 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X + 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 2:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X - 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X + 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    break;
                case 3:
                    avc.playSoundEffect(fireSound, 0.1f);
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X - 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    Level.Add(new Needle(content, new Vector3(weapFirePos.X + 1.6f, weapFirePos.Y, weapFirePos.Z), new Vector3(0, 0, 8 * (float)Math.Cos(playerRot.Y))));
                    break;
            }
        }
    }

}
