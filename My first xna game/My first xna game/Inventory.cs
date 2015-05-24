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
        public Selector selector; //TODO PRIVATE
        protected Player player;
        protected bool useKeyReleased = false;
        protected Actor sourcePack;
        //private Vector2 savedWindowPosition; //effects
        //private bool positionMaxed = false; //effects

        public int margin
        {
            get { return (window.bounds.Width - (int)window.thickness.X * 2) / (Item.size + spacing); }
        }

        public Inventory(Player player, bool createWindowItems = true)
        {
            this.player = player;

            window = new Window(Game.content.Load<Texture2D>("windowskin"), Vector2.Zero, 200, 200, null);
            window.thickness = new Vector2(20, 20);

            if (createWindowItems)
            {
                sourcePack = player;
                CreateWindowItems();
            }
        }

        protected void CreateWindowItems()
        {
            pack = sourcePack.pack;
            pack.representation = this;
            pack.CreateItems(this);
            SortItems();
            selector = new Selector(window, window.itemsList, new Vector2(Item.size + spacing, Item.size + spacing), spacing / 2, margin);
            selector.player = this.player;
        }

        protected void DeleteWindowItems()
        {
            for (int counter = 0; counter < window.itemsList.Count; )
            {
                RemoveWindowItem(window.itemsList[counter], false);
            }
        }

        public void Revive()
        {
            window.SetWindowAbove(sourcePack.bounds);
            DeleteWindowItems();
            CreateWindowItems();
            ShopInventory shopInventory = this as ShopInventory;
            if (shopInventory != null)
            {
                shopInventory.DeletePriceTexts();
                shopInventory.CreatePriceTexts();
            }
            //savedWindowPosition = window.position; //effects
            alive = true;
        }

        public void Kill()
        {
            alive = false;
        }

        protected void SortItems()
        {
            for (int counter = 0; counter < window.itemsList.Count; counter++)
            {
                window.itemsList[counter].position = new Vector2(counter % margin * (Item.size + spacing), counter / margin * (Item.size + spacing));
            }
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }

            UpdateBuyInventory();
            UpdateShopInventory();

            window.Update(gameTime);
            selector.Update(newState, oldState, gameTime);

            //window speical effects
            /*
            if (!positionMaxed)
            {
                window.position.Y += 0.25f;
                if (window.position.Y >= savedWindowPosition.Y + 5f)
                {
                    positionMaxed = true;
                }
            }
            if (positionMaxed)
            {
                window.position.Y -= 0.25f;
                if (window.position.Y <= savedWindowPosition.Y)
                {
                    positionMaxed = false;
                }
            }*/

            if (selector.visible)
            {
                UpdateInput(newState, oldState);
            }
        }

        protected virtual void UpdateBuyInventory() { }
        protected virtual void UpdateShopInventory() { }

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
            Item currentItem = pack.items[selector.currentTargetNum];
            currentItem.function(player, player);
            if (currentItem.wasted)
            {
                pack.SubItem(currentItem);
            }
        }

        public void RemoveWindowItem(WindowItem windowItem, bool sort = true)
        {
            window.itemsList.Remove(windowItem);
            windowItem = null;

            if (sort)
            {
                SortItems();
                selector.Clamp();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition)
        {
            if (!alive) { return; }

            DrawBuyInventory(spriteBatch, offsetRect, screenPosition);
            DrawShopInventory(spriteBatch, offsetRect, screenPosition);

            window.Draw(spriteBatch, offsetRect, screenPosition);
            selector.Draw(spriteBatch, offsetRect, screenPosition);
        }

        protected virtual void DrawBuyInventory(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition) { }
        protected virtual void DrawShopInventory(SpriteBatch spriteBatch, Rectangle offsetRect, Rectangle screenPosition) { }
    }
}
