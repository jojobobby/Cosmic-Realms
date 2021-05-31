package com.company.assembleegameclient.screens
{
    import com.company.assembleegameclient.constants.InventoryOwnerTypes;
    import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
    import com.company.assembleegameclient.ui.tooltip.TextToolTip;
    import com.company.assembleegameclient.ui.tooltip.ToolTip;
    import flash.display.Sprite;
    import flash.events.MouseEvent;
    import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
    import io.decagames.rotmg.ui.texture.TextureParser;
    import kabam.rotmg.core.StaticInjectorContext;
    import kabam.rotmg.core.signals.ShowTooltipSignal;
    import kabam.rotmg.text.model.TextKey;
    import kabam.rotmg.tooltips.HoverTooltipDelegate;

    public class VaultPreview extends Sprite
    {
        private const padding:uint = 4;
        private const rowLength:uint = 4;
        private const BACKGROUND_WIDTH:int = 200;
        private const BACKGROUND_HEIGHT:int = 100;

        private var tooltipFocusSlot:VaultSlot;
        private var tooltip:ToolTip;
        private var hoverTooltipDelegate:HoverTooltipDelegate;
        private var background:SliceScalingBitmap;
        public var slots:Vector.<VaultSlot>;
        public var showTooltipSignal:ShowTooltipSignal;

        public function VaultPreview(_arg_1:Vector.<VaultSlot>)
        {
            this.slots = _arg_1;
            this.setBackground("popup_background_simple");
            this.showTooltipSignal = StaticInjectorContext.getInjector().getInstance(ShowTooltipSignal);
            this.hoverTooltipDelegate = new HoverTooltipDelegate();
            this.hoverTooltipDelegate.setShowToolTipSignal(this.showTooltipSignal);
            this.hoverTooltipDelegate.tooltip = this.tooltip;
            this.createGrid();
        }

        public function createGrid():void
        {
            var slot:VaultSlot;
            var i:int;
            var slotsLength:int = slots.length;
            i = 0;
            while (i < slotsLength)
            {
                slot = slots[i];
                slot.addEventListener(MouseEvent.ROLL_OVER, this.onTileHover);
                slot.x = ((i % rowLength) * (45 + rowLength)) + padding;
                slot.y = (int((i / rowLength)) * (45 + rowLength)) + padding;
                addChild(slot);
                i++;
            }
        }

        private function setBackground(s:String):void
        {
            this.background = TextureParser.instance.getSliceScalingBitmap("UI", s);
            this.background.width = BACKGROUND_WIDTH;
            this.background.height = BACKGROUND_HEIGHT;
            super.addChildAt(this.background, 0);
        }

        private function onTileHover(event:MouseEvent):void
        {
            if (!stage)
            {
                return;
            }
            var slot:VaultSlot = (event.currentTarget as VaultSlot);
            this.addTooltipToSlot(slot);
            this.tooltipFocusSlot = slot;
        }

        private function addTooltipToSlot(vaultSlot:VaultSlot):void
        {
            var s:String;
            if (vaultSlot.itemType > 0)
            {
                this.tooltip = new EquipmentToolTip(vaultSlot.itemType, null, -1, InventoryOwnerTypes.NPC);
            }
            else
            {
                s = TextKey.ITEM;
                this.tooltip = new TextToolTip(0x363636, 0x9B9B9B, null, TextKey.ITEM_EMPTY_SLOT, BACKGROUND_WIDTH, {"itemType": TextKey.wrapForTokenResolution(s)});
            }
            this.tooltip.attachToTarget(vaultSlot);
            this.showTooltipSignal.dispatch(this.tooltip);
        }
    }
}