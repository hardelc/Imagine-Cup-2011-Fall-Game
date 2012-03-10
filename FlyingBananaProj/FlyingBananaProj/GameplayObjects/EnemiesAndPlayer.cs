namespace FlyingBananaProj
{
    enum RealName
    {
        RickSheldon,
        StraightCell,
        ChargingCell,
        HomingCell,
        LatchingCell,
        MeleeBot,
        MissileBot,
        Infector,
        ClotSide,
        Boss1
    }

    class EnemiesAndPlayer : GameEntity
    {
        #region Class Variables
            protected bool enabled;
            protected int health;
            protected int damage; //direct damage done by collisions (cells hitting, melee bots punching)
            protected int score; //will not be used by player
            protected int powerup;
            protected RealName name;
            protected bool killedByPlayer = false;
        #endregion

        #region Accessors and Mutators

        public int Health
        {
            get { return Health; }
            set { Health = value; }
        }

        public int getHealth()
        {
            return health;
        }

        public void setHealth(int newHealth)
        {
            health = newHealth;
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public int powerUp
        {
            get { return powerup; }
            set { powerup = value; }
        }
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public bool killedBy()
        { return killedByPlayer; }

        #endregion

        public override void checkBounds()
        {
            if (position.Z < -240 && !(this is Player))
            {
                Die();
            }
        }

        public virtual void takeDamage(int damageTook)
        {
            if (this is Player)
            {
                Player p = this as Player;
                p.takeDamage();
            }
            else
            {
                health -= damageTook;
                if (health <= 0)
                {
                    killedByPlayer = true;
                    Die();
                }
            }    
        }
    }
}
