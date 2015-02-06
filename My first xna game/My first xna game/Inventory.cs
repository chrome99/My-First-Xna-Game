using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Inventory : Scene
    {
        public int spacing = 6;
        public Window window;
        private Map map;
        private Player player;
        private Pack pack;
        private Selector selector;

        public int margin
        {
            get { return (window.bounds.Width - (int)window.thickness.X * 2) / (Item.size + spacing); }
        }
        private bool quitKeyReleased = false;
        private bool useKeyReleased = false;

        public Inventory(Map map, Player player)
        {
            this.map = map;
            this.player = player;

            window = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, 200, 200, null);
            window.thickness = new Vector2(20, 20);
            pack = player.pack;
            pack.CreateItems(this);
            selector = new Selector(window, window.itemsList, new Vector2(Item.size + spacing, Item.size + spacing), spacing / 2, margin);
            selector.player = this.player;
        }

        private void SortItems()
        {
            for (int i = 0; i < pack.items.Count; i++)
            {
                pack.items[i].icon.position = new Vector2(i % margin * (Item.size + spacing), i / margin * (Item.size + spacing));
            }
        }

        public override void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            window.Update(gameTime);
            selector.Update(newState, oldState, gameTime);

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {
            if (newState.IsKeyDown(Keys.Enter) && useKeyReleased)
            {
                pack.items[selector.currentTargetNum].function(player, player);
                if (pack.items[selector.currentTargetNum].oneTime)
                {
                    pack.SubItem(pack.items[selector.currentTargetNum]);
                    window.itemsList.Remove(selector.currentTarget);
                    SortItems();
                    selector.Clamp();
                }

                useKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Enter))
            {
                useKeyReleased = true;
            }

            if (newState.IsKeyDown(Keys.Escape) && quitKeyReleased)
            {
                Game.scene = map;

                quitKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(Keys.Escape))
            {
                quitKeyReleased = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            window.Draw(spriteBatch, new Rectangle(), new Rectangle());
            selector.Draw(spriteBatch);
            map.Draw(spriteBatch);
        }
    }
}
