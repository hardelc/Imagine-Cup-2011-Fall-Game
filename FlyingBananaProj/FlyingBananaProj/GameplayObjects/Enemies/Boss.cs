using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class Boss : EnemiesAndPlayer
    {
        protected bool activated;
        protected BotWeapon weapon;
        public Boss(ContentManager content, Vector3 position)
        {
            this.position = position;
            velocity = Vector3.Zero;
            health = 100;
            Model = content.Load<Model>(@"models/boss1");
            texture = content.Load<Texture2D>(@"models/BossTexture");
            name = RealName.Boss1;
            score = 15;
            weapon = new BotMissile(content);
            activated = false;
            rotate(new Vector3(0, -MathHelper.ToRadians(45), 0));
        }

        public void Activate()
        {
            activated = true;
        }

        public void Update(GameTime gameTime, Vector3 playerPosition)
        {
            sphere = new BoundingSphere(position, 5);
            worldMat = Matrix.CreateScale(20) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateTranslation(position);
        }

        public override void Draw()
        {
            DrawModel(model, worldMat, Camera.Instance.View, Camera.Instance.Projection);
        }

        public override void Collision(GameEntity target)
        { 
        }
    }
}
