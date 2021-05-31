package ToolForge.forgeList {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.tooltip.EquipmentToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;

import flash.display.Bitmap;

import flash.display.Sprite;
import flash.events.MouseEvent;

import org.osflash.signals.Signal;

public class ForgeListItem extends Sprite{

    private var toolTip:ToolTip;
    public var toolTipSignal:Signal = new Signal(ToolTip);
    private var objectType:int;
    private var gameSprite:GameSprite;

    public function ForgeListItem(gs:GameSprite, itemID:String, isResult:Boolean = false) {
        this.gameSprite = gs;

        if (isResult) {
            graphics.beginFill(0xFFFFFF);
        } else {
            graphics.beginFill(0xbababa)
        }
        graphics.drawRoundRect(0, 0, 50, 50, 5, 5);
        graphics.endFill();

        this.objectType = ObjectLibrary.idToType_[itemID];
        var itemSprite:Bitmap = new Bitmap(ObjectLibrary.getRedrawnTextureForItemGlowFromType(this.objectType, 60, true));
        addChild(itemSprite);

        addEventListener(MouseEvent.MOUSE_OVER, mouseOver);
        addEventListener(MouseEvent.MOUSE_OUT, mouseOut);
    }

    private function mouseOver(e:MouseEvent):void {
        this.toolTip = new EquipmentToolTip(this.objectType, this.gameSprite.map.player_, this.gameSprite.map.player_.objectType_, InventoryOwnerTypes.NPC);
        this.toolTip.attachToTarget(this);
        this.toolTipSignal.dispatch(toolTip);
    }

    private function mouseOut(e:MouseEvent):void {
        this.toolTip.detachFromTarget();
        this.toolTip = null;
    }
}
}
