using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace FlyingBananaProj
{
    class EnemySpawner
    {
        protected double enemySpawnTimeSeconds = 1.5;
        protected double rbcSpawnTimeSeconds = 0.5;
        protected TimeSpan enemySpawnTime;
        protected TimeSpan rbcSpawnTime;
        protected string[] cellList;
        protected string[] botList;
        protected string[] enemyList;
        protected int[] specialEncounterList;
        protected bool readyToSpawn;
        protected Random rand;
        protected ContentManager content;
        protected Vector3 playerPosition;
        protected float levelZ;
        protected int botCheckpoint;
        protected int chargingCellCheckpoint;
        protected int latchingCellCheckpoint;
        protected int meleeBotCheckpoint;
        protected int activeLevel;
        public EnemySpawner(int level, ContentManager content)
        {
            switch (level)
            {
                case 1:
                    cellList = new string[]
                {"StraightCell",
                "ChargingCell",
                "LatchingCell"};
                    botList = new string[] 
                {"MeleeBot",
                "MissileBot"};
                    var list = new List<string>();
                    list.AddRange(cellList);
                    list.AddRange(botList);
                    enemyList = list.ToArray();
                    botCheckpoint = 4000; //point at which nanobots appear
                    chargingCellCheckpoint = 7500;
                    latchingCellCheckpoint = 6000;
                    meleeBotCheckpoint = 4500;
                    break;
            }
            rand = new Random();
            this.content = content;
            readyToSpawn = false;
            enemySpawnTime = TimeSpan.FromSeconds(enemySpawnTimeSeconds);
            rbcSpawnTime = TimeSpan.FromSeconds(rbcSpawnTimeSeconds);
            activeLevel = level;
        }

        public void Update(GameTime gameTime, Vector3 playerPos, float levelCurrentZ, bool enemies, bool cells)
        {
            playerPosition = playerPos;
            levelZ = levelCurrentZ;
            enemySpawnTime = enemySpawnTime.Subtract(gameTime.ElapsedGameTime);
            if (enemySpawnTime.TotalSeconds <= 0)
            {
                readyToSpawn = true;
                enemySpawnTime = TimeSpan.FromSeconds(enemySpawnTimeSeconds);
            }

            if (activeLevel == 1)
            {
                rbcSpawnTime = rbcSpawnTime.Subtract(gameTime.ElapsedGameTime);
                if (rbcSpawnTime.TotalSeconds <= 0)
                {
                    if (levelCurrentZ < chargingCellCheckpoint || cells)
                    {
                        if (cells)
                            AddEnemyAt("StraightCell", new Vector3(rand.Next(-80, 80), 0, 350));
                        else AddEnemyAt("StraightCell", new Vector3(rand.Next(-80, 80), 0, 50));
                    }
                    else
                    {
                        AddEnemyAt("StraightCell", new Vector3(rand.Next(-80, 80), rand.Next(-80, 80), 250));
                    }
                    rbcSpawnTime = TimeSpan.FromSeconds(rbcSpawnTimeSeconds);
                }
                if (readyToSpawn && enemies)
                {
                    if (levelCurrentZ < botCheckpoint)
                    {
                        if (rand.Next(5) > 0)
                            AddRandomBot();
                        else AddRandomCell();
                    }
                    else
                    {
                        if (levelCurrentZ < meleeBotCheckpoint)
                        {
                            AddEnemy("MeleeBot");
                        }
                        else
                        {
                            if (levelCurrentZ < chargingCellCheckpoint)
                            {
                                AddEnemy("ChargingCell");
                            }
                            if (levelCurrentZ < latchingCellCheckpoint)
                            {
                                AddEnemy("LatchingCell");
                            }
                        }
                        
                    }
                    readyToSpawn = false;
                }
            }
        }

        public void AddEnemy(string enemyName)
        {
            switch (enemyName)
            {
                case "StraightCell":
                    Level.Add(new StraightCell(content, new Vector3(rand.Next(-80, 80), 0, 50)));
                    break;
                case "ChargingCell":
                    Level.Add(new ChargingCell(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "LatchingCell":
                    Level.Add(new LatchingCell(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "MeleeBot":
                    Level.Add(new MeleeBot(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "MissileBot":
                    Level.Add(new MissileBot(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
            }
        }

        public void AddEnemyAt(string enemyName, Vector3 location)
        {
            switch (enemyName)
            {
                case "StraightCell":
                    Level.Add(new StraightCell(content, location));
                    break;
                case "ChargingCell":
                    Level.Add(new ChargingCell(content, location, playerPosition));
                    break;
                case "LatchingCell":
                    Level.Add(new LatchingCell(content, location, playerPosition));
                    break;
                case "MeleeBot":
                    Level.Add(new MeleeBot(content, location, playerPosition));
                    break;
                case "MissileBot":
                    Level.Add(new MissileBot(content, location, playerPosition));
                    break;
            }
        }

        public void AddRandomCell()
        {
            int randIndex = rand.Next(cellList.Length);
            switch (cellList[randIndex])
            {
                case "StraightCell":
                    Level.Add(new StraightCell(content, new Vector3(rand.Next(-80, 80), 0, 50)));
                    break;
                case "ChargingCell":
                    Level.Add(new ChargingCell(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "LatchingCell":
                    Level.Add(new LatchingCell(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
            }
        }

        public void AddRandomBot()
        {
            int randIndex = rand.Next(botList.Length);
            switch (botList[randIndex])
            {
                case "MeleeBot":
                    Level.Add(new MeleeBot(content, new Vector3(rand.Next(-80, 80), 0, 20), playerPosition));
                    break;
                case "MissileBot":
                    Level.Add(new MissileBot(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
            }
        }

        public void AddRandomEnemy()
        {
            int randIndex = rand.Next(enemyList.Length);

            switch (enemyList[randIndex])
            {
                case "StraightCell":
                    Level.Add(new StraightCell(content, new Vector3(rand.Next(-80, 80), 0, 50)));
                    break;
                case "ChargingCell":
                    Level.Add(new ChargingCell(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "LatchingCell":
                    Level.Add(new LatchingCell(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "MeleeBot":
                    Level.Add(new MeleeBot(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
                case "MissileBot":
                    Level.Add(new MissileBot(content, new Vector3(rand.Next(-80, 80), 0, 50), playerPosition));
                    break;
            }
        }
    }
}

