using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class TokenPickup : GameEntity
    {
        protected String name;
        Random random;
        public TokenPickup(ContentManager content, Vector3 position)
        {
            this.position = position;
            velocity = -Vector3.UnitZ;           
            Model = content.Load<Model>(@"models/tokenbox");

            random = new Random();

            int randomEnum = random.Next(Player.weapList.Length);

            name = Player.weapList[randomEnum];

            switch (name)
            {
                case "Missile":
                    texture = content.Load<Texture2D>(@"textures/MissileBoxTexture");
                    break;
                case "Needle":
                    texture = content.Load<Texture2D>(@"textures/NeedlesBoxTexture");
                    break;
                case "Shield":
                    texture = content.Load<Texture2D>(@"textures/ShieldBoxTexture");
                    break;
                case "Wave":
                    texture = content.Load<Texture2D>(@"textures/WaveBoxTexture");
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            position += velocity;
            sphere = new BoundingSphere(position, 2);
            worldMat = Matrix.CreateScale(0.4f)  * Matrix.CreateTranslation(position);
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
                Player p = target as Player;
                p.changeWeapon((PlayerWeaponName)Enum.Parse(typeof(PlayerWeaponName), name, true));
                Die();
            }
        }
    }
}
