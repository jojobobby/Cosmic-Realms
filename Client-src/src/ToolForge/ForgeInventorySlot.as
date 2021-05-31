package ToolForge
{
    import com.company.assembleegameclient.constants.InventoryOwnerTypes;
    import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.EquipmentTile;
    import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.InventoryTile;
    import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
    import com.company.assembleegameclient.ui.tooltip.TextToolTip;
    import com.company.assembleegameclient.ui.tooltip.ToolTip;
    import com.company.assembleegameclient.util.DisplayHierarchy;
    import com.company.util.CachingColorTransformer;
    import flash.display.Bitmap;
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.objects.ObjectLibrary;
    import flash.geom.ColorTransform;
    import flash.events.MouseEvent;
    import flash.events.Event;
    import ToolForge.forgeControls.ForgeControl;
    import kabam.rotmg.constants.ItemConstants;
    import kabam.rotmg.text.model.TextKey;
    import org.osflash.signals.Signal;

    public class ForgeInventorySlot extends ForgeSlotBase
    {
        public var tooltipSignal:Signal;
        private var tooltip:ToolTip;
        private var unblockInventoryCallback:Function;
        private var targetSlotType:int;
        private var slotType:int;
        private var placeholderIcon:Bitmap;
        private var type:String;
        private var gs:GameSprite;
        public var forgeItemSignal:Signal;

        public function ForgeInventorySlot(s:String, i:int=-1, i1:int=0)
        {
            this.type = s;
            this.targetSlotType = i1;
            this.forgeItemSignal = new Signal(ForgeItemInformation);
            if (i != -1)
            {
                this.placeholderIcon = new Bitmap(CachingColorTransformer.transformBitmapData(ObjectLibrary.getRedrawnTextureFromType(i, 80, true), new ColorTransform(0, 0, 0, 0.25, 0, 0, 0, 0)));
                this.placeholderIcon.x = ((100 - this.placeholderIcon.width) * 0.5);
                this.placeholderIcon.y = ((46 - this.placeholderIcon.height) * 0.5);
                addChild(this.placeholderIcon);
            }
            itemSprite.addEventListener(MouseEvent.MOUSE_DOWN, this.onMouseDown);
            addEventListener(MouseEvent.ROLL_OVER, this.showToolTip);
            this.tooltipSignal = new Signal(ToolTip);
        }

        public function init(gameSprite:GameSprite):void
        {
            this.gs = gameSprite;
        }

        public function setItem(i:int, i1:int, i2:int, f:Function):Boolean
        {
            this.unblockInventory();
            this.unblockInventoryCallback = f;
            this.slotType = ObjectLibrary.xmlLibrary_[i].SlotType;
            if (!this.isSlotIdOkay())
            {
                this.unblockInventory();
                this.slotType = -1;
                return false;
            }
            this.internal_setItem(i, i1, i2);
            return true;
        }

        private function internal_setItem(i:int, i1:int, i2:int):void
        {
            if (this.itemId != i)
            {
                this.itemId = i;
                this.slotId = i1;
                this.objectId = i2;
                slotIcon.bitmapData = (((this.itemId == -1)) ? null : ObjectLibrary.getRedrawnTextureFromType(i, 80, true));
                positionIcon();
                if (this.placeholderIcon != null)
                {
                    this.placeholderIcon.visible = (i == -1);
                }
            }
        }

        private function isSlotIdOkay():Boolean
        {
            if (this.targetSlotType == 0)
            {
                return true;
            }
            if (this.targetSlotType == int.MAX_VALUE)
            {
                return ItemConstants.isWeapon(this.slotType);
            }
            if (this.targetSlotType == (int.MAX_VALUE - 1))
            {
                return ItemConstants.isAbility(this.slotType);
            }
            if (this.targetSlotType == (int.MAX_VALUE - 2))
            {
                return ItemConstants.isArmor(this.slotType);
            }
            if (this.targetSlotType == -2)
            {
                return ItemConstants.isEquipment(this.slotType);
            }
            return (this.targetSlotType == this.slotType);
        }

        private function posMouseFix(i:int, i1:int):void
        {
            slotIcon.x = (-(slotIcon.width) / 2);
            slotIcon.y = (-(slotIcon.height) / 2);
            itemSprite.x = i;
            itemSprite.y = i1;
        }

        private function onMouseDown(event:MouseEvent):void
        {
            if (this.placeholderIcon != null)
            {
                this.placeholderIcon.visible = true;
            }
            this.hideToolTip();
            this.posMouseFix(event.stageX, event.stageY);
            itemSprite.startDrag(true);
            itemSprite.addEventListener(MouseEvent.MOUSE_UP, this.item_onMouseUp);
            if (((!((itemSprite.parent == null))) && (!((itemSprite.parent == stage)))))
            {
                removeChild(itemSprite);
                stage.addChild(itemSprite);
            }
        }
        private function unblockInventory():void
        {
            ((this.unblockInventoryCallback) && (this.unblockInventoryCallback()));
            this.unblockInventoryCallback = null;
        }

        override protected function onRemovedFromStage(event:Event):void
        {
            super.onRemovedFromStage(event);
            this.unblockInventory();
        }

        private function item_onMouseUp(event:MouseEvent):void
        {
            var itemInformation:ForgeItemInformation;
            var inventoryTile:InventoryTile;
            var equipmentTile:EquipmentTile;
            itemSprite.stopDrag();
            itemSprite.removeEventListener(MouseEvent.MOUSE_UP, this.item_onMouseUp);
            stage.removeChild(itemSprite);
            addChild(itemSprite);
            positionIcon();
            var displayObject:* = DisplayHierarchy.getParentWithTypeArray(itemSprite.dropTarget, EquipmentTile, InventoryTile, ForgeInventorySlot, ToolForgeFrame);
            if ((displayObject is ForgeInventorySlot))
            {
                if (displayObject == this)
                {
                    if (this.placeholderIcon != null)
                    {
                        this.placeholderIcon.visible = false;
                    }
                    return;
                }
                (displayObject as ForgeInventorySlot).setItem(this.itemId, this.slotId, this.objectId, this.unblockInventoryCallback);
                this.reset();
            }
            else
            {
                if (!(((displayObject is ToolForgeFrame)) && ((((displayObject is InventoryTile)) || ((displayObject is EquipmentTile))))))
                {
                    if (this.type == ForgeControl.OUTPUT)
                    {
                        if ((displayObject is InventoryTile))
                        {
                            inventoryTile = (displayObject as InventoryTile);
                            if ((((inventoryTile.getItemId() == -1)) && (inventoryTile)))
                            {
                                itemInformation = new ForgeItemInformation();
                                itemInformation.targetSlotId = inventoryTile.tileId;
                            }
                        }
                        else
                        {
                            if ((displayObject is EquipmentTile))
                            {
                                equipmentTile = (displayObject as EquipmentTile);
                                if (((equipmentTile.canHoldItem(this.itemId)) && ((equipmentTile.getItemId() == -1))))
                                {
                                    itemInformation = new ForgeItemInformation();
                                    itemInformation.targetSlotId = equipmentTile.tileId;
                                }
                            }
                        }
                        if (itemInformation != null)
                        {
                            this.forgeItemSignal.dispatch(itemInformation);
                        }
                    }
                    this.resetFully();
                }
            }
        }

        public function resetFully():void
        {
            this.unblockInventory();
            this.reset();
        }

        public function reset():void
        {
            this.itemId = -1;
            this.slotId = -1;
            this.objectId = -1;
            this.unblockInventoryCallback = null;
            slotIcon.bitmapData = null;
            if (this.placeholderIcon != null)
            {
                this.placeholderIcon.visible = true;
            }
            this.setTitle("", {});
            this.setDescription("", {});
        }

        private function hideToolTip():void
        {
            if (((((!(stage)) || (!(this.tooltip)))) || (!(this.tooltip.stage))))
            {
                return;
            }
            if (this.tooltip != null)
            {
                this.tooltip.detachFromTarget();
                this.tooltip = null;
            }
        }

        private function showToolTip(event:MouseEvent):void
        {
            if (this.itemId > 0)
            {
                this.tooltip = new EquipmentToolTip(this.itemId, this.gs.map.player_, this.objectId, InventoryOwnerTypes.NPC);
            }
            else
            {
                this.tooltip = new TextToolTip(0x363636, 0x9B9B9B, null, TextKey.ITEM_EMPTY_SLOT, 200, {"itemType":this.itemTypeName()});
            }
            this.tooltip.attachToTarget(this);
            this.tooltipSignal.dispatch(this.tooltip);
        }

        private function itemTypeName():String
        {
            if (this.targetSlotType == 0)
            {
                return "Item";
            }
            if (this.targetSlotType == int.MAX_VALUE)
            {
                return "Weapon";
            }
            if (this.targetSlotType == (int.MAX_VALUE - 1))
            {
                return "Ability";
            }
            if (this.targetSlotType == (int.MAX_VALUE - 2))
            {
                return "Armor";
            }
            if (this.targetSlotType == -1)
            {
                return "Output";
            }
            if (this.targetSlotType == -2)
            {
                return "Equipment";
            }
            return "Item";
        }

        public function updateItemData(inventorySlot:ForgeInventorySlot):void
        {
            this.internal_setItem(inventorySlot.itemId, inventorySlot.slotId, inventorySlot.objectId);
        }

        public function updateItemDataRaw(i:int, i1:int, i2:int):void
        {
            this.internal_setItem(i, i1, i2);
        }

        private function hasModifier(s:String):Boolean
        {
            var xml:XML = ObjectLibrary.xmlLibrary_[this.itemId];
            for each (var modifierElement:XML in xml.Modifier)
            {
                if (modifierElement.@name == s)
                {
                    return true;
                }
            }
            return false;
        }
    }
}