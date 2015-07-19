using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace My_first_xna_game
{
    class EquipmentMenu
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
        private int subUnEquipedOpacity = 30;

        public EquipmentMenu(Map map, Player player)
        {
            this.player = player;

            armorInventory = new ChoiceInventory(map, player, HandleArmorInventoryChoice, Inventory.Side.none);
            armorInventory.Kill();

            window = new Window(map, Game.content.Load<Texture2D>("Textures\\Windows\\windowskin"), Vector2.Zero, 140, 220, null);
            window.SetWindowLeft(player.bounds);

            head = new Picture(Item.IconSet, new Vector2(46, 32), window);

            body = new Picture(Item.IconSet, new Vector2(46, 32 * 2), window);

            leftHand = new Picture(Item.IconSet, new Vector2(14, 32 * 2 + 10), window);

            shoes = new Picture(Item.IconSet, new Vector2(46, 32 * 3), window);

            rightHand = new Picture(Item.IconSet, new Vector2(78, 32 * 2 + 10), window);

            selector = new Selector(player, window.itemsList, HandleEquipmentChoice, new Vector2(32, 32), 3, 1, window);

            DrawTemplate();
        }

        private void DrawTemplate()
        {
            bool twoHandedWeapon = false;

            if (player.equipment.head == null)
            {
                head.fileDrawingRect = ItemCollection.hat.getRect;
                head.opacity = subUnEquipedOpacity;
            }
            else
            {
                head.fileDrawingRect = player.equipment.head.getRect;
            }

            if (player.equipment.body == null)
            {
                body.fileDrawingRect = ItemCollection.shirt.getRect;
                body.opacity = subUnEquipedOpacity;
            }
            else
            {
                body.fileDrawingRect = player.equipment.body.getRect;
            }

            if (player.equipment.shoes == null)
            {
                shoes.fileDrawingRect = ItemCollection.leatherShoes.getRect;
                shoes.opacity = subUnEquipedOpacity;
            }
            else
            {
                shoes.fileDrawingRect = player.equipment.shoes.getRect;
            }

            if (player.equipment.leftHand == null)
            {
                leftHand.fileDrawingRect = ItemCollection.ironSword.getRect;
                leftHand.opacity = subUnEquipedOpacity;
            }
            else if (player.equipment.leftHand.armorType == Armor.ArmorType.oneHanded)
            {
                leftHand.fileDrawingRect = player.equipment.leftHand.getRect;
            }
            else
            {
                leftHand.fileDrawingRect = player.equipment.leftHand.getRect;
                twoHandedWeapon = true;
            }
            if (!twoHandedWeapon)
            {
                if (player.equipment.rightHand == null)
                {
                    rightHand.fileDrawingRect = ItemCollection.ironSword.getRect;
                    rightHand.opacity = subUnEquipedOpacity;
                }
                else
                {
                    rightHand.fileDrawingRect = player.equipment.rightHand.getRect;
                }
            }
            else
            {
                rightHand.fileDrawingRect = new Rectangle(0, 0, 0, 0);
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

        private void HandleArmorInventoryChoice(Item item)
        {
            Picture chosenBodyPartPicture = window.itemsList[chosenBodyPart] as Picture;

            Armor armor = item as Armor;
            if (armor.armorType == Armor.ArmorType.oneHanded)
            {
                if (player.checkIfEquiped(armor) == 2 || player.checkIfEquiped(armor) == 3)
                {
                    return;
                }
                if (player.leftHandOrRightHand())
                {
                    leftHand.fileDrawingRect = item.getRect;
                    leftHand.opacity = 100;
                    rightHand.fileDrawingRect = ItemCollection.ironSword.getRect;
                    rightHand.opacity = subUnEquipedOpacity;
                }
                else
                {
                    if (player.checkIfEquiped(armor) == 2 || player.checkIfEquiped(armor) == 3)
                    {
                        return;
                    }
                    rightHand.fileDrawingRect = item.getRect;
                    rightHand.opacity = 100;
                }
            }
            else if (armor.armorType == Armor.ArmorType.twoHanded)
            {
                if (player.checkIfEquiped(armor) == 1)
                {
                    return;
                }
                leftHand.fileDrawingRect = item.getRect;
                leftHand.opacity = 100;
                rightHand.fileDrawingRect = new Rectangle(0, 0, 0, 0);
            }
            else
            {
                if (player.checkIfEquiped(armor) == 1) { return; }
                chosenBodyPartPicture.fileDrawingRect = item.getRect;
                chosenBodyPartPicture.opacity = 100;
            }

            player.Equip(armor);
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
            selector.confirmKeyReleased = false;
            selector.currentTargetNum = 0;
            window.SetWindowLeft(player.bounds);
            DrawTemplate();
            alive = true;
        }

        public void Update(KeyboardState newState, KeyboardState oldState, GameTime gameTime)
        {
            if (!alive) { return; }

            window.Update(gameTime);
            window.UpdateSelectorAndTextBox(newState, oldState, gameTime);

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
        }

        private void HandleEquipmentChoice()
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
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle offsetRect)
        {
            if (!alive) { return; }
            window.Draw(spriteBatch, offsetRect);
            armorInventory.Draw(spriteBatch, offsetRect);
        }
    }
}
