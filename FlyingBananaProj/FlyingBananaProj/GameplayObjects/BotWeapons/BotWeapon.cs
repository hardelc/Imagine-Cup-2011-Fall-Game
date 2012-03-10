using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace FlyingBananaProj
{
    enum BotWeaponName
    {
        Missile
    }
    class BotWeapon : GameEntity
    {
        #region Class Variables
        protected int damage;
        protected BotWeaponName name; //name assigned from the enum - should never change
        protected SoundEffect fireSound;
        protected ContentManager content;
        protected bool projectile;

        #endregion

        #region Accessors and Mutators

        public void playSound()
        {
            fireSound.Play();
        }

        public int Damage
        {
            get { return damage; }
        }

        public BotWeaponName Name
        {
            get { return name; }
        }
        #endregion

        public virtual void fire() { }

        public virtual void Update(GameTime gameTime, Vector3 playerPosition, Vector3 botRotation)
        {
        }

        public override void Collision(GameEntity target)
        {
            if (target is Player)
            {
                Player p = target as Player;
                if (!(this is BotMissile)) //sheer impact of missile can't hurt the player
                    p.takeDamage(damage);
                Die();
            }
        }
    }
}
