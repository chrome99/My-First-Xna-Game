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
            PlayerKeys player1Keys = new PlayerKeys();
            player1Keys.attack.key = Keys.J;
            player1Keys.jump.key = Keys.K;
            player1Keys.defend.key = Keys.L;
            player1Keys.mvLeft.key = Keys.A;
            player1Keys.mvRight.key = Keys.D;
            player1Keys.mvUp.key = Keys.W;
            player1Keys.mvDown.key = Keys.S;
            player1Keys.opMenu.key = Keys.Escape;
            player1Keys.run.key = Keys.LeftShift;
            player1Keys.opDebug.key = Keys.F2;
            player1Keys.opCommand.key = Keys.F3;
            player1Keys.useSkill.key = Keys.R;

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
            PlayerKeys player2Keys = new PlayerKeys();
            player2Keys.attack.key = Keys.Home;
            player2Keys.jump.key = Keys.Home;
            player2Keys.defend.key = Keys.Home;
            player2Keys.mvLeft.key = Keys.Home;
            player2Keys.mvRight.key = Keys.Home;
            player2Keys.mvUp.key = Keys.Home;
            player2Keys.mvDown.key = Keys.Home;
            player2Keys.opMenu.key = Keys.Home;
            player2Keys.run.key = Keys.Home;
            player2Keys.opDebug.key = Keys.Home;
            player2Keys.opCommand.key = Keys.Home;
            player2Keys.useSkill.key = Keys.Home;


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
            PlayerKeys player3Keys = new PlayerKeys();
            player3Keys.attack.key = Keys.Home;
            player3Keys.jump.key = Keys.Home;
            player3Keys.defend.key = Keys.Home;
            player3Keys.mvLeft.key = Keys.Home;
            player3Keys.mvRight.key = Keys.Home;
            player3Keys.mvUp.key = Keys.Home;
            player3Keys.mvDown.key = Keys.Home;
            player3Keys.opMenu.key = Keys.Home;
            player3Keys.run.key = Keys.Home;
            player3Keys.opDebug.key = Keys.Home;
            player3Keys.opCommand.key = Keys.Home;
            player3Keys.useSkill.key = Keys.Home;

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
            PlayerKeys player4Keys = new PlayerKeys();
            player4Keys.attack.key = Keys.Home;
            player4Keys.jump.key = Keys.Home;
            player4Keys.defend.key = Keys.Home;
            player4Keys.mvLeft.key = Keys.Home;
            player4Keys.mvRight.key = Keys.Home;
            player4Keys.mvUp.key = Keys.Home;
            player4Keys.mvDown.key = Keys.Home;
            player4Keys.opMenu.key = Keys.Home;
            player4Keys.run.key = Keys.Home;
            player4Keys.opDebug.key = Keys.Home;
            player4Keys.opCommand.key = Keys.Home;
            player4Keys.useSkill.key = Keys.Home;

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
