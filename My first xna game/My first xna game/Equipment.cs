using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class Equipment
    {
        public bool alive = false;
        private Window window;
        private Selector selector;
        private Player player;
        private ChoiceInventory armorInventory;

        private Picture head;
        private Picture body;
        private Picture shoes;
        private Picture leftHand;
        private Picture rightHand;

        private int chosenBodyPart;
        private bool useKeyReleased = false;
        private int subUnEquipedOpacity = 60;

        public Equipment(Map map, Player player)
        {
            this.player = player;

            armorInventory = new ChoiceInventory(map, player, HandleItemChoice, Inventory.Side.none);
            armorInventory.Kill();

            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), Vector2.Zero, 140, 220, null);
            window.SetWindowLeft(player.bounds);

            head = new Picture(Item.IconSet, new Vector2(46, 32), window);
            window.AddItem(head);

            body = new Picture(Item.IconSet, new Vector2(46, 32 * 2), window);
            window.AddItem(body);

            leftHand = new Picture(Item.IconSet, new Vector2(14, 32 * 2 + 10), window);
            window.AddItem(leftHand);

            shoes = new Picture(Item.IconSet, new Vector2(46, 32 * 3), window);
            window.AddItem(shoes);

            rightHand = new Picture(Item.IconSet, new Vector2(78, 32 * 2 + 10), window);
            window.AddItem(rightHand);

            selector = new Selector(window, window.itemsList, new Vector2(32, 32), 3, 1);
            selector.player = player;

            DrawTemplate();
        }

        private void DrawTemplate()
        {
            if (player.equipment.head == null)
            {
                head.drawingRect = ItemCollection.hat.getRect;
                head.opacity = subUnEquipedOpacity;
            }
            else
            {
                head.drawingRect = player.equipment.head.getRect;
            }

            if (player.equipment.body == null)
            {
                body.drawingRect = ItemCollection.shirt.getRect;
                body.opacity = subUnEquipedOpacity;
            }
            else
            {
                body.drawingRect = player.equipment.body.getRect;
            }

            if (player.equipment.shoes == null)
            {
                shoes.drawingRect = ItemCollection.leatherShoes.getRect;
                shoes.opacity = subUnEquipedOpacity;
            }
            else
            {
                shoes.drawingRect = player.equipment.shoes.getRect;
            }

            if (player.equipment.leftHand == null)
            {
                leftHand.drawingRect = ItemCollection.ironSword.getRect;
                leftHand.opacity = subUnEquipedOpacity;
            }
            else
            {
                leftHand.drawingRect = player.equipment.leftHand.getRect;
            }

            if (player.equipment.rightHand == null)
            {
                rightHand.drawingRect = ItemCollection.ironSword.getRect;
                rightHand.opacity = subUnEquipedOpacity;
            }
            else
            {
                rightHand.drawingRect = player.equipment.rightHand.getRect;
            }
        }

        public void setPlayerWindowPosition()
        {
            window.SetWindowLeft(player.bounds);
            if (armorInventory.alive)
            {
                armorInventory.setPlayerWindowPosition();
            }
        }

        private void HandleItemChoice(Item item)
        {
            Picture chosenBodyPartPicture = window.itemsList[chosenBodyPart] as Picture;
            Rectangle rect = item.getRect;
            chosenBodyPartPicture.drawingRect = rect;
            chosenBodyPartPicture.opacity = 100;
            player.Equip(item as Armor);
        }

        public bool HandleMenuButtonPress()
        {
            if (armorInventory.alive)
            {
                armorInventory.Kill();
                selector.active = true;
                return false;
            }
            else
            {
                Kill();
                return true;
            }
        }

        public void Kill()
        {
            alive = false;
        }

        public void Revive()
        {
            useKeyReleased = false;
            selector.currentTargetNum = 0;
            window.SetWindowLeft(player.bounds);
            DrawTemplate();
            alive = true;
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }
            window.Update(gameTime);
            selector.Update(newState, oldState, gameTime);
            armorInventory.Update(newState, oldState, gameTime);
            switch (selector.currentTargetNum)
            {
                case 0:
                    selector.itemsInRow = 1;
                    break;

                case 1:
                    selector.itemsInRow = 2;
                    break;

                case 2:
                    selector.itemsInRow = 1;
                    break;

                case 3:
                    selector.itemsInRow = 2;
                    break;

                case 4:
                    selector.itemsInRow = 3;
                    break;
            }

            UpdateInput(newState, oldState);
        }

        private void UpdateInput(KeyboardState newState, KeyboardState oldState)
        {

            if (newState.IsKeyDown(player.kbKeys.attack) && useKeyReleased)
            {
                if (selector.active)
                {
                    Game.content.Load<SoundEffect>("Audio\\Waves\\confirm").Play();
                    chosenBodyPart = selector.currentTargetNum;
                    switch (chosenBodyPart)
                    {
                        case 0: //head
                            armorInventory.filter = Inventory.Filter.head;
                            break;

                        case 1: //body
                            armorInventory.filter = Inventory.Filter.body;
                            break;

                        case 2: //left hand
                            armorInventory.filter = Inventory.Filter.weapon;
                            break;

                        case 3: //shoes
                            armorInventory.filter = Inventory.Filter.shoes;
                            break;

                        case 4: //right hand
                            armorInventory.filter = Inventory.Filter.weapon;
                            break;
                    }
                    armorInventory.SetWindowPosition(new Vector2(window.position.X + window.width + 30, window.position.Y));
                    armorInventory.Revive();
                    selector.active = false;
                    useKeyReleased = false;
                }

            }
            else if (!oldState.IsKeyDown(player.kbKeys.attack))
            {
                useKeyReleased = true;
            }

        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }
            window.Draw(spriteBatch, offsetRect);
            selector.Draw(spriteBatch, offsetRect);
            armorInventory.Draw(spriteBatch, offsetRect);
        }
    }
}
