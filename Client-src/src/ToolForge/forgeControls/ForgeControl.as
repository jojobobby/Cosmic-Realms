package ToolForge.forgeControls
{
    import flash.display.Sprite;
    import com.company.assembleegameclient.game.GameSprite;
    import ToolForge.ForgeSlotCollection;
    import ToolForge.ForgeItemInformation;
    import flash.events.Event;
    import ToolForge.ForgeInventorySlot;
    import ToolForge.forgeHelp.ForgeHelpButton;
    import ToolForge.forgeHelp.ForgeHelpWindow;
    import org.osflash.signals.Signal;

    public class ForgeControl extends Sprite
    {
        public static const INPUT_TARGET:String = "input_target";
        public static const INPUT_ITEM:String = "input_item";
        public static const OUTPUT:String = "output";
        public static const WEAPON_ANY:int = int.MAX_VALUE;//2147483647
        public static const ABILITY_ANY:int = (int.MAX_VALUE - 1);//2147483646
        public static const ARMOR_ANY:int = (int.MAX_VALUE - 2);//2147483645
        public static const ANY:int = 0;
        public static const EQUIPMENT_ANY:int = -2;
        public static const WIDTH:int = 320;
        public static const HEIGHT:int = 140;
        public var PRICE:int = 0;

        public var forge:Signal;
        public var gs:GameSprite;
        private var slots:ForgeSlotCollection;

        public function ForgeControl(gameSprite:GameSprite)
        {
            this.gs = gameSprite;
            this.forge = new Signal(ForgeItemInformation);
            this.slots = new ForgeSlotCollection(this);
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }

        final protected function drawSlot(string:String, i:int = -1, i2:int = 0) : void
        {
            var forgeInventorySlot:ForgeInventorySlot = new ForgeInventorySlot(string, i, (((string == OUTPUT)) ? -1 : i2));
            forgeInventorySlot.init(this.gs);
            this.slots.addSlot(string, forgeInventorySlot);
            this.slots.align();
            if (string != INPUT_TARGET)
            {
                addChild(forgeInventorySlot);
            }
            if (string == OUTPUT)
            {
                forgeInventorySlot.forgeItemSignal.add(this.onForge);
            }
        }

        final protected function setPrice(i:int = 0) : void
        {
            PRICE = i;
        }

        public function getPrice() : int
        {
            return PRICE;
        }

        final protected function setHelp(cls:Class):void
        {
            var button:ForgeHelpButton;
            button = new ForgeHelpButton(cls);
            button.onInfoClick.add(this.openHelp);
            button.y = HEIGHT;
            button.x = WIDTH - 30;
            addChild(button);
        }

        private function onForge(itemInformation:ForgeItemInformation):void
        {
            var forgeInventorySlot:ForgeInventorySlot;
            var forgeInventorySlot1:ForgeInventorySlot = this.getSlot(INPUT_TARGET);
            var forgeInventorySlot2:ForgeInventorySlot = this.getSlot(OUTPUT);
            var slots:Array = this.slots.getSlots(INPUT_ITEM);
            itemInformation.inputItemSlotId = forgeInventorySlot1.slotId;
            //itemInformation.newDescription = ((_local_3.getItemData().Description) || (""));
            itemInformation.isPresent = (this is PresentControl);
            itemInformation.modifiers = [];
            for each (forgeInventorySlot in slots)
            {
                if (forgeInventorySlot.slotId != -1)
                {
                    itemInformation.modifiers.push(forgeInventorySlot.slotId);
                }
            }
            itemInformation.price = PRICE;
            this.slots.freeAll();
            this.forge.dispatch(itemInformation);
        }

        final public function drawArrow(i:int, i2:int, i3:int, i4:int):void
        {
            var i5:int = i3 - i;
            var i6:int = i4 - i2;
            var i7:int = Math.sqrt((i5 * i5) + (i6 * i6));
            var number:Number = i3 - ((20 * i5) / i7);
            var number2:Number = i4 - ((20 * i6) / i7);
            graphics.clear();
            graphics.beginFill(0xFFFFFF);
            graphics.lineStyle(5, 0xFFFFFF);
            graphics.moveTo(i, i2);
            graphics.lineTo(i3, i4);
            graphics.lineTo(number + ((i4 - number2) / 2), number2 + ((number - i3) / 2));
            graphics.lineTo(((2 * number) + i3) / 3, ((2 * number2) + i4) / 3);
            graphics.lineTo(number - ((i4 - number2) / 2), number2 - ((number - i3) / 2));
            graphics.lineTo(i3, i4);
            graphics.endFill();
        }

        public function update(object:Object, s:String):void
        {
        }

        protected function getSlot(s:String, i:int=0):ForgeInventorySlot
        {
            var slots:Array = this.slots.getSlots(s);
            if ((((i > -1)) && ((i < slots.length))))
            {
                return slots[i];
            }
            throw new RangeError("The index cannot be bigger than the length");
        }

        private function onRemovedFromStage(event:Event):void
        {
            this.getSlot(OUTPUT).forgeItemSignal.remove(this.onForge);
        }

        private function openHelp(helpButton:ForgeHelpButton):void
        {
            var helpWindow:ForgeHelpWindow = helpButton.createHelp();
            helpWindow.onClose.add(this.onHelpRemoved);
            var sprite:Sprite = new Sprite();
            sprite.graphics.beginFill(0, 0.7);
            sprite.graphics.drawRect(0, 0, 800, 600);
            sprite.graphics.endFill();
            sprite.addChild(helpWindow);
            parent.parent.addChild(sprite);
        }

        private function onHelpRemoved(helpWindow:ForgeHelpWindow):void
        {
            helpWindow.parent.parent.removeChild(helpWindow.parent);
        }

    }
}