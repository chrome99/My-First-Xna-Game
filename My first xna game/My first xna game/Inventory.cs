using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace My_first_xna_game
{
    public class Inventory
    {
        // TODO: Can an inventory be alive or dead? (Do we really know?)
        public bool alive = false;
        public int spacing = 6;
        protected Window window;
        protected Pack pack;
        public Selector selector; //TODO PRIVATE
        public List<Text> amountTexts = new List<Text>();
        protected Player player;
        protected bool useKeyReleased = false;
        protected Actor sourcePack;
        //private Vector2 savedWindowPosition; //effects
        //private bool positionMaxed = false; //effects
        public enum Filter { all, armor, head, body, shoes, weapon, oneHanded, twoHanded }
        public enum Side { right, left, up, down, none }
        public Filter filter;
        private Side side;

        public int margin
        {
            get { return (window.bounds.Width - (int)window.thickness.X * 2) / (Item.size + spacing); }
        }

        public Vector2 windowPosition
        {
            get { return window.position; }
        }

        public void SetWindowPosition(Side side)
        {
            if (side == Side.none)
            {
                throw new System.AccessViolationException("Cant use SetWindowPosition unless side != Side.none", null);
            }
            setWindowPosition(side);
        }

        public void SetWindowPosition(Vector2 newPosition)
        {
            if (side != Side.none)
            {
                throw new System.AccessViolationException("Cant use this SetWindowPosition unless side == Side.none", null);
            }
            window.position = newPosition;
        }

        public Inventory(Map map, Player player, bool createWindowItems = true, Side side = Side.up, Filter filter = Filter.all)
        {
            this.player = player;
            this.side = side;
            this.filter = filter;

            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), Vector2.Zero, 200, 200, null);
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
            pack.CreateItems(this, window);
            SortItems();
            selector = new Selector(window, window.itemsList, new Vector2(Item.size + spacing, Item.size + spacing), spacing / 2, margin);
            selector.player = this.player;
            if (pack.items.Count == 0)
            {
                selector.visible = false;
            }
        }

        protected void DeleteWindowItems()
        {
            for (int counter = 0; counter < window.itemsList.Count; )
            {
                RemoveWindowItem(window.itemsList[counter], false);
            }
            for (int counter = 0; counter < amountTexts.Count;)
            {
                amountTexts[counter] = null;
                amountTexts.Remove(amountTexts[counter]);
            }
        }

        private void setWindowPosition(Side newSide = Side.none)
        {
            Side sideToSetOn;
            if (newSide == Side.none)
            {
                sideToSetOn = side;
            }
            else
            {
                sideToSetOn = newSide;
            }
            switch (sideToSetOn)
            {
                case Side.down:
                    window.SetWindowBelow(sourcePack.bounds);
                    break;

                case Side.up:
                    window.SetWindowAbove(sourcePack.bounds);
                    break;

                case Side.left:
                    window.SetWindowLeft(sourcePack.bounds);
                    break;

                case Side.right:
                    window.SetWindowRight(sourcePack.bounds);
                    break;
            }
        }

        public void setPlayerWindowPosition()
        {
            setWindowPosition();
        }

        public void Revive()
        {
            useKeyReleased = false;
            setWindowPosition();
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
            //sort amount
            List<WindowItem> result = new List<WindowItem>();
            foreach (Text amount in amountTexts)
            {
                foreach (WindowItem windowItem in window.itemsList)
                {
                    if (windowItem.position == amount.position)
                    {
                        result.Add(windowItem);
                    }
                }
            }

            //sort window items
            for (int counter = 0; counter < window.itemsList.Count; counter++)
            {
                window.itemsList[counter].position = new Vector2(counter % margin * (Item.size + spacing), counter / margin * (Item.size + spacing));
                if (result.Contains(window.itemsList[counter]))
                {
                    int index = result.IndexOf(window.itemsList[counter]);
                    amountTexts[index].position = window.itemsList[counter].position;
                }
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

            //sound
            if (currentItem.function != null || currentItem.wasted)
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
            }
            else
            {
                Game.content.Load<SoundEffect>("Audio\\Waves\\cancel").Play();
            }

            //function
            if (currentItem.function != null)
            {
                currentItem.function(player, player);
            }

            //waste item
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

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }

            DrawBuyInventory(spriteBatch, offsetRect);
            DrawShopInventory(spriteBatch, offsetRect);

            window.Draw(spriteBatch, offsetRect);
            selector.Draw(spriteBatch, offsetRect);

            foreach (Text amount in amountTexts)
            {
                amount.Draw(spriteBatch, offsetRect);
            }
        }

        protected virtual void DrawBuyInventory(SpriteBatch spriteBatch, Rectangle offsetRect) { }
        protected virtual void DrawShopInventory(SpriteBatch spriteBatch, Rectangle offsetRect) { }
    }
}
