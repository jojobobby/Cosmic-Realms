package kabam.rotmg.memMarket.content {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.assembleegameclient.util.Currency;

import flash.display.Bitmap;

import flash.display.Graphics;
import flash.display.Shape;

import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.system.System;

import kabam.rotmg.assets.services.IconFactory;

import kabam.rotmg.messaging.impl.data.MarketData;

public class MemMarketItem extends Sprite
{
    /* Offers */
    public static const OFFER_WIDTH:int = 100;
    public static const OFFER_HEIGHT:int = 83;

    /* Inventory slots */
    public static const SLOT_WIDTH:int = 50;
    public static const SLOT_HEIGHT:int = 50;

    public var gameSprite_:GameSprite;
    public var itemType_:int;
    public var id_:int;
    public var data_:MarketData;
    public var shape_:Shape;
    public  var icon_:Sprite;
    public var toolTip_:ToolTip;

    /* Provides the base features for a Market item */
    public function MemMarketItem(gameSprite:GameSprite, width:int, height:int, iconSize:int, itemType:int, data:MarketData, isBg:Boolean = false)
    {
        this.gameSprite_ = gameSprite;
        this.itemType_ = itemType;
        this.id_ = data == null ? -1 : data.id_;
        this.data_ = data;

        /* Draw background */
        this.shape_ = new Shape();
        drawRoundRectAsFill(this.shape_.graphics, 0, 0, width, height, 0);
        addChild(this.shape_);

        if (this.itemType_ != -1)
        {
            var icon:Bitmap = new Bitmap(ObjectLibrary.getRedrawnTextureFromType(this.itemType_, iconSize, true));

            if (isBg) {
                icon.x = 0;
                icon.y = 7;
            } else {
                icon.x = -3;
                icon.y = -3;
            }
            this.icon_ = new Sprite();

            if (isBg) {
                this.icon_.graphics.beginFill(0x2D2D2D);
                this.icon_.graphics.lineStyle(2, 0x3F3F3F);
                this.icon_.graphics.drawRoundRect(3, 10, 50, 50, 4, 4);
                this.icon_.graphics.endFill();
            }

            this.icon_.addChild(icon);
            addChild(this.icon_);

            addEventListener(MouseEvent.MOUSE_OVER, this.onOver);
            addEventListener(MouseEvent.MOUSE_OUT, this.onOut);
        }
    }

    protected function removeListeners() : void
    {
        removeEventListener(MouseEvent.MOUSE_OVER, this.onOver);
        removeEventListener(MouseEvent.MOUSE_OUT, this.onOut);
    }

    /* Mouse over */
    private function onOver(event:MouseEvent) : void
    {
        this.toolTip_ = new EquipmentToolTip(this.itemType_, this.gameSprite_.map.player_, this.gameSprite_.map.player_.objectType_, InventoryOwnerTypes.NPC);
        this.gameSprite_.addChild(this.toolTip_); /* Add it to the overlay, adding it to the Shape makes it have a wrong position */
        this.icon_.alpha = 0.7;
    }

    /* Mouse out */
    private function onOut(event:MouseEvent) : void
    {
        this.toolTip_.parent.removeChild(this.toolTip_);
        this.toolTip_ = null;
        this.icon_.alpha = 1.0;
    }

    /* Clear */
    public function dispose() : void
    {
        this.gameSprite_ = null;
        this.shape_ = null;
        this.icon_ = null;

        if (this.toolTip_ != null)
        {
            this.toolTip_.parent.removeChild(this.toolTip_);
            this.toolTip_ = null;
        }

        this.removeListeners();

        /* Remove all children */
        for (var i:int = numChildren - 1; i >= 0; i--)
        {
            removeChildAt(i);
        }
    }

    /* Taken from https://stackoverflow.com/a/25118121 */
    /* Used to draw rectangles with rounded edges */
    public static function drawRoundRectAsFill(graphics:Graphics, x:Number, y:Number, w:Number, h:Number, radius:Number, lineColor:uint=0x676767, fillColor:uint=0x454545, lineThickness:Number=1, lineAlpha:Number=1, fillAlpha:Number=1) : void
    {
        graphics.lineStyle(0,0,0);
        graphics.beginFill(lineColor, lineAlpha);
        graphics.drawRoundRect(x, y, w, h, 2*radius, 2*radius);
        graphics.drawRoundRect(x+lineThickness, y+lineThickness, w-2*lineThickness, h-2*lineThickness, 2*radius-2*lineThickness, 2*radius-2*lineThickness);
        graphics.endFill();
        graphics.beginFill(fillColor,fillAlpha);
        graphics.drawRoundRect(x+lineThickness, y+lineThickness, w-2*lineThickness, h-2*lineThickness, 2*radius-2*lineThickness, 2*radius-2*lineThickness);
        graphics.endFill();
    }
}
}