using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Menu
    {
        public Choice choice;
        public bool alive = false;
        private Player player;
        private Inventory inventory;
        private EquipmentMenu equipment;
        private Options options;

        public Menu(Map map, Player player)
        {
            this.player = player;
            inventory = new Inventory(map, player);
            equipment = new EquipmentMenu(map, player);
            options = new Options(map, player);

            Text returnText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Return", null, new Vector2(2, 5));
            Text inventoryText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Inventory", null, new Vector2(2, 5));
            Text equipmentText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Equipment", null, new Vector2(2, 5));
            Text optionsText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Options", null, new Vector2(2, 5));
            Text saveText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Save", null, new Vector2(2, 5));
            Text loadText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Load", null, new Vector2(2, 5));
            Text exitText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Exit", null, new Vector2(2, 5));
            choice = new Choice(map, player.bounds, player, new List<WindowItem> { returnText, inventoryText, equipmentText, optionsText, saveText, loadText, exitText }, HandleChoice, Choice.Arrangement.column);
        }

        public void Kill()
        {
            alive = false;
        }

        public void Revive()
        {
            choice.ResetSelector();
            choice.window.SetWindowCenter(player.bounds);
            alive = true;
        }

        public void setPlayerWindowPosition()
        {
            choice.window.SetWindowCenter(player.bounds);

            inventory.setPlayerWindowPosition();
            equipment.setPlayerWindowPosition();
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }
            inventory.Update(newState, oldState, gameTime);
            equipment.Update(newState, oldState, gameTime);
            choice.Update(newState, oldState, gameTime);
            options.Update(newState, oldState, gameTime);
        }

        private void HandleChoice()
        {
            Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
            switch (choice.currentTargetNum)
            {
                case 0: //Return
                    alive = false;
                    player.holdUpdateInput = true;
                    return;

                case 1: //Inventory
                    choice.alive = false;
                    inventory.Revive();
                    break;

                case 2: //Equipment
                    choice.alive = false;
                    equipment.Revive();
                    break;

                case 3: //Options
                    choice.alive = false;
                    options.Revive();
                    break;

                case 4: //Save
                    Game.InitiateSave();
                    alive = false;
                    break;

                case 5: //Load
                    Game.InitiateLoad();
                    alive = false;
                    break;

                case 6: //Exit
                    Game.endGame = true;
                    break;
            }
        }

        public void HandleMenuButtonPress()
        {
            if (inventory.alive)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                inventory.Kill();
                choice.alive = true;
                return;
            }
            else if (equipment.alive)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                if (equipment.HandleMenuButtonPress())
                {
                    choice.alive = true;
                }
                return;
            }
            else if (options.alive)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                options.Kill();
                choice.alive = true;
                return;
            }
            else if (alive)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
                Kill();
            }
            else
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\menu").Play();
                Revive();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }
            inventory.Draw(spriteBatch, offsetRect);
            equipment.Draw(spriteBatch, offsetRect);
            options.Draw(spriteBatch);
            choice.Draw(spriteBatch, offsetRect);
        }
    }
}