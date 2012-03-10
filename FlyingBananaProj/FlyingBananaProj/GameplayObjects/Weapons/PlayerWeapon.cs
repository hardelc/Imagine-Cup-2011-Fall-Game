using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace FlyingBananaProj
{
    enum PlayerWeaponName
    {
        Needle,
        Missile,
        Shield,
        Wave
    }
    class PlayerWeapon : GameEntity
    {
        #region Class Variables
        protected int damage;
        protected TimeSpan fireTime; //time before firing the next projectile
        protected int currentLevel;
        protected PlayerWeaponName name; //name assigned from the enum - should never change
        protected string fireSound;
        protected ContentManager content;
        protected bool projectile;
        protected Vector3 playerRot;
        protected AudioVideoController avc;
        #endregion

        #region Accessors and Mutators
        public TimeSpan FireTime
        {
            get { return fireTime; }
            set { fireTime = value; }
        }

        public int Damage
        {
            get { return damage; }
        }

        public int CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public PlayerWeaponName Name
        {
            get { return name; }
        }
        #endregion

        public virtual void fire(){}

        public virtual void Update(GameTime gameTime, Vector3 playerPosition, Vector3 playerRotation)
        { 
        }

        public override void Collision(GameEntity target)
        {
            if (target is EnemiesAndPlayer)
            {
                EnemiesAndPlayer ep = target as EnemiesAndPlayer;
                ep.takeDamage(damage);
                Die();
            }
            else if (target is BotMissile)
            {
                BotMissile bm = target as BotMissile;
                bm.Die(); //should missiles be shot down in one hit?
            }
        }

    }
}
