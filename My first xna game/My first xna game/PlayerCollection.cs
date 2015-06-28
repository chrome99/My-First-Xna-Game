using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace My_first_xna_game
{
    class PlayerCollection
    {
        static public Player player1;
        static public Player player2;
        static public Player player3;
        static public Player player4;

        static PlayerCollection()
        {
            Player.PlayerKeys player1Keys;
            player1Keys.attack = Keys.Space;
            player1Keys.jump = Keys.Q;
            player1Keys.defend = Keys.E;
            player1Keys.mvLeft = Keys.A;
            player1Keys.mvRight = Keys.D;
            player1Keys.mvUp = Keys.W;
            player1Keys.mvDown = Keys.S;
            player1Keys.opMenu = Keys.LeftControl;
            player1Keys.run = Keys.LeftShift;
            player1Keys.opDebug = Keys.F2;
            player1Keys.useSkill = Keys.R;

            Hostile.Stats player1Stats;
            player1Stats.maxHealth = 26;
            player1Stats.health = 26;
            player1Stats.maxMana = 16;
            player1Stats.mana = 16;
            player1Stats.strength = 4;
            player1Stats.knockback = 30;
            player1Stats.defence = 2;
            player1Stats.agility = 1;
            player1Stats.exp = 0;
            player1Stats.level = 1;

            player1 = new Player(Game.content.Load<Texture2D>("Textures\\Spritesheets\\starlord"), new Vector2(250f, 260f), player1Keys, player1Stats);
            player1.gold = 25;


            player1.pack.AddItem(new List<Item> {
                ItemCollection.mine, ItemCollection.woodenStaff, ItemCollection.ironChestArmor, ItemCollection.ironSword,
                /*ItemCollection.ironSword, ItemCollection.ironSword, ItemCollection.leatherShoes, ItemCollection.mask,
                ItemCollection.shirt, ItemCollection.copperChestArmor, ItemCollection.apple, ItemCollection.apple, ItemCollection.apple*/
            });

            //intialize player
            Player.PlayerKeys player2Keys;
            player2Keys.attack = Keys.RightControl;
            player2Keys.jump = Keys.Delete;
            player2Keys.defend = Keys.PageDown;
            player2Keys.mvLeft = Keys.Left;
            player2Keys.mvRight = Keys.Right;
            player2Keys.mvUp = Keys.Up;
            player2Keys.mvDown = Keys.Down;
            player2Keys.opMenu = Keys.Back;
            player2Keys.run = Keys.RightShift;
            player2Keys.opDebug = Keys.F4;
            player2Keys.useSkill = Keys.Home;


            Hostile.Stats player2Stats;
            player2Stats.maxHealth = 7;
            player2Stats.health = 7;
            player2Stats.maxMana = 16;
            player2Stats.mana = 16;
            player2Stats.strength = 4;
            player2Stats.knockback = 30;
            player2Stats.defence = 2;
            player2Stats.agility = 1;
            player2Stats.exp = 0;
            player2Stats.level = 1;

            player2 = new Player(Game.content.Load<Texture2D>("Textures\\Spritesheets\\rocket"), new Vector2(300f, 260f), player2Keys, player2Stats);
            player2.gold = 25;

            //intialize player
            Player.PlayerKeys player3Keys;
            player3Keys.attack = Keys.R;
            player3Keys.jump = Keys.Delete;
            player3Keys.defend = Keys.PageDown;
            player3Keys.mvLeft = Keys.F;
            player3Keys.mvRight = Keys.H;
            player3Keys.mvUp = Keys.T;
            player3Keys.mvDown = Keys.G;
            player3Keys.opMenu = Keys.B;
            player3Keys.run = Keys.Y;
            player3Keys.opDebug = Keys.F6;
            player3Keys.useSkill = Keys.Home;

            Hostile.Stats player3Stats;
            player3Stats.maxHealth = 16;
            player3Stats.health = 16;
            player3Stats.maxMana = 16;
            player3Stats.mana = 16;
            player3Stats.strength = 4;
            player3Stats.knockback = 30;
            player3Stats.defence = 2;
            player3Stats.agility = 1;
            player3Stats.exp = 0;
            player3Stats.level = 1;

            player3 = new Player(Game.content.Load<Texture2D>("Textures\\Spritesheets\\drax"), new Vector2(350f, 260f), player3Keys, player3Stats);
            player3.gold = 25;

            //intialize player
            Player.PlayerKeys player4Keys;
            player4Keys.attack = Keys.U;
            player4Keys.jump = Keys.Delete;
            player4Keys.defend = Keys.PageDown;
            player4Keys.mvLeft = Keys.J;
            player4Keys.mvRight = Keys.L;
            player4Keys.mvUp = Keys.I;
            player4Keys.mvDown = Keys.K;
            player4Keys.opMenu = Keys.M;
            player4Keys.run = Keys.O;
            player4Keys.opDebug = Keys.F8;
            player4Keys.useSkill = Keys.Home;

            Hostile.Stats player4Stats;
            player4Stats.maxHealth = 16;
            player4Stats.health = 16;
            player4Stats.maxMana = 16;
            player4Stats.mana = 16;
            player4Stats.strength = 4;
            player4Stats.knockback = 30;
            player4Stats.defence = 2;
            player4Stats.agility = 1;
            player4Stats.exp = 0;
            player4Stats.level = 1;

            player4 = new Player(Game.content.Load<Texture2D>("Textures\\Spritesheets\\gamora"), new Vector2(400f, 260f), player4Keys, player4Stats);
            player4.gold = 25;

            /*player1.coreCollision.Y = 4;
            player2.coreCollision.Y = 4;
            player3.coreCollision.Y = 4;
            player4.coreCollision.Y = 4;*/
        }
    }
}
