﻿using System.Collections.Generic;
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
        private Equipment equipment;
        private bool confirmKeyReleased = false;

        public Menu(Map map, Player player)
        {
            this.player = player;
            inventory = new Inventory(map, player);
            equipment = new Equipment(map, player);

            Text returnText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Return");
            Text inventoryText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Inventory");
            Text equipmentText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Equipment");
            Text exitText = new Text(Game.content.Load<SpriteFont>("Fonts\\medival1"), Vector2.Zero, Color.White, "Exit");
            choice = new Choice(map, player.bounds, player, new List<WindowItem> { returnText, inventoryText, equipmentText, exitText }, Choice.Arrangement.column);
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

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (!choice.alive) { return; }
            if (newState.IsKeyDown(player.kbKeys.attack) && confirmKeyReleased)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                switch (choice.currentTargetNum)
                {
                    case 0: //Return
                        alive = false;
                        break;

                    case 1: //Inventory
                        choice.alive = false;
                        inventory.Revive();
                        break;

                    case 2: //Equipment
                        choice.alive = false;
                        equipment.Revive();
                        break;

                    case 3: //Exit
                        Game.endGame = true;
                        break;
                }
                confirmKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                confirmKeyReleased = true;
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
            choice.Draw(spriteBatch, offsetRect);
        }
    }
}
