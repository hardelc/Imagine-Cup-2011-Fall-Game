using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class ChargingCell : EnemiesAndPlayer
    {
        public ChargingCell(ContentManager content, Vector3 position, Vector3 playerPosition)
        {
            this.position = position;
            velocity = playerPosition - position;
            velocity.Normalize();
            health = 10;
            Model = content.Load<Model>(@"models/whitebloodcell");
            texture = content.Load<Texture2D>(@"textures/whiteBloodCellTexture");
            name = RealName.ChargingCell;
            score = 15;
        }

        public override void Update(GameTime gameTime)
        {
            position += velocity;
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
                Die();
                Player p = target as Player;
                p.takeDamage();
            }
            else if (target is Missle)
            {
                target.Collision(this);
            }
            else if (target is Needle)
            {
                target.Collision(this);
            }
            else if (target is Explosion)
            {
                target.Collision(this);
            }
        }
    }
}