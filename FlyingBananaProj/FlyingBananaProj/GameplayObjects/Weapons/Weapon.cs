using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace FlyingBananaProj
{
    enum WeaponName
    {
        Needle,
        Missile,
        Shield,
        Wave
    }
    class Weapon : GameEntity
    {
        #region Class Variables
        protected int damage;
        protected TimeSpan fireTime; //time before firing the next projectile
        protected int currentLevel;
        protected WeaponName name; //name assigned from the enum - should never change
        protected SoundEffect fireSound;
        protected ContentManager content;
        protected bool projectile;

        #endregion

        #region Accessors and Mutators
        public TimeSpan FireTime
        {
            get { return fireTime; }
            set { fireTime = value; }
        }

        public void playSound()
        {
            fireSound.Play();
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

        public WeaponName Name
        {
            get { return name; }
        }
        #endregion

        public virtual void fire(){}

        public virtual void Update(GameTime gameTime, Vector3 playerPosition)
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
        }

    }
}
