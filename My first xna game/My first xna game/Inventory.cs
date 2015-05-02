using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace My_first_xna_game
{
    public class Inventory
    {
        // TODO: Can an inventory be alive or dead? (Do we really know?)
        public bool alive = false;
        public int spacing = 6;
        public Window window;
        protected Pack pack;
        protected Selector selector;
        protected Player player;
        protected bool useKeyReleased = false;

        public int margin
        {
            get { return (window.bounds.Width - (int)window.thickness.X * 2) / (Item.size + spacing); }
        }

        public Inventory(Player player, bool createPack = true)
        {
            this.player = player;

            window = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, 200, 200, null);
            window.thickness = new Vector2(20, 20);

            if (createPack)
            {
                pack = player.pack;
                pack.CreateItems(this);
                selector = new Selector(window, window.itemsList, new Vector2(Item.size + spacing, Item.size + spacing), spacing / 2, margin);
                selector.player = this.player;
            }
        }

        protected void SortItems()
        {
            for (int i = 0; i < pack.items.Count; i++)
            {
                pack.items[i].icon.position = new Vector2(i % margin * (Item.size + spacing), i / margin * (Item.size + spacing));
            }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }
            UpdateBuyInventory(newState, oldState, gameTime);

            window.Update(gameTime);
            selector.Update(newState, oldState, gameTime);

            if (selector.visible)
            {
                UpdateInput(newState, oldState);
            }
        }

        protected virtual void UpdateBuyInventory(KeyboardState newState, KeyboardState oldState, GameTime gameTime) { }

        protected void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {

            if (newState.IsKeyDown(player.kbKeys.attack) && useKeyReleased)
            {
                HandleItemChoice();

                useKeyReleased = false;
            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                useKeyReleased = true;
            }

        }

        protected virtual void HandleItemChoice()
        {
            pack.items[selector.currentTargetNum].function(player, player);
            if (pack.items[selector.currentTargetNum].oneTime)
            {
                pack.SubItem(pack.items[selector.currentTargetNum]);
                window.itemsList.Remove(selector.currentTarget);
                SortItems();
                selector.Clamp();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle screenPosition)
        {
            if (!alive) { return; }
            DrawShop(spriteBatch, screenPosition);

            window.Draw(spriteBatch, new Rectangle(), screenPosition);
            selector.Draw(spriteBatch, new Rectangle(), screenPosition);
        }

        protected virtual void DrawShop(SpriteBatch spriteBatch, Rectangle screenPosition) { }
    }
}
