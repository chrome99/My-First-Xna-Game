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
            player1Keys.attack = Keys.J;
            player1Keys.jump = Keys.K;
            player1Keys.defend = Keys.L;
            player1Keys.mvLeft = Keys.A;
            player1Keys.mvRight = Keys.D;
            player1Keys.mvUp = Keys.W;
            player1Keys.mvDown = Keys.S;
            player1Keys.opMenu = Keys.Escape;
            player1Keys.run = Keys.LeftShift;
            player1Keys.opDebug = Keys.F2;
            player1Keys.opCommand = Keys.F3;
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
            player1.core = new Rectangle(7, 30, 18, 16);
            Color color = Color.OrangeRed;
            int raduis = 60;
            LightSource light1 = new LightSource(raduis, color);
            LightSource light2 = new LightSource(raduis, color);
            LightSource light3 = new LightSource(raduis, color);
            LightSource light4 = new LightSource(raduis, color);
            LightSource light5 = new LightSource(raduis, color);
            light1.position = new Vector2(32, 0);
            light2.position = new Vector2(-32, 0);
            light3.position = new Vector2(0, 0);
            light4.position = new Vector2(0, 32);
            light5.position = new Vector2(0, -32);

            player1.AddLight(new LightStructure(player1, new List<LightSource>() { light1, light2, light3, light4, light5 }));


            player1.pack.AddItem(new List<Item> {
                ItemCollection.apple ,ItemCollection.woodenStaff, ItemCollection.mine, ItemCollection.ironChestArmor, ItemCollection.ironSword,
                /*ItemCollection.ironSword, ItemCollection.ironSword, ItemCollection.leatherShoes, ItemCollection.mask,
                ItemCollection.shirt, ItemCollection.copperChestArmor, ItemCollection.apple, ItemCollection.apple, ItemCollection.apple*/
            });

            //intialize player
            Player.PlayerKeys player2Keys;
            player2Keys.attack = Keys.Home;
            player2Keys.jump = Keys.Home;
            player2Keys.defend = Keys.Home;
            player2Keys.mvLeft = Keys.Home;
            player2Keys.mvRight = Keys.Home;
            player2Keys.mvUp = Keys.Home;
            player2Keys.mvDown = Keys.Home;
            player2Keys.opMenu = Keys.Home;
            player2Keys.run = Keys.Home;
            player2Keys.opDebug = Keys.Home;
            player2Keys.opCommand = Keys.Home;
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
            player3Keys.attack = Keys.Home;
            player3Keys.jump = Keys.Home;
            player3Keys.defend = Keys.Home;
            player3Keys.mvLeft = Keys.Home;
            player3Keys.mvRight = Keys.Home;
            player3Keys.mvUp = Keys.Home;
            player3Keys.mvDown = Keys.Home;
            player3Keys.opMenu = Keys.Home;
            player3Keys.run = Keys.Home;
            player3Keys.opDebug = Keys.Home;
            player3Keys.opCommand = Keys.Home;
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
            player4Keys.attack = Keys.Home;
            player4Keys.jump = Keys.Home;
            player4Keys.defend = Keys.Home;
            player4Keys.mvLeft = Keys.Home;
            player4Keys.mvRight = Keys.Home;
            player4Keys.mvUp = Keys.Home;
            player4Keys.mvDown = Keys.Home;
            player4Keys.opMenu = Keys.Home;
            player4Keys.run = Keys.Home;
            player4Keys.opDebug = Keys.Home;
            player4Keys.opCommand = Keys.Home;
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
