package kabam.rotmg.marketUI.components.items {
import com.company.assembleegameclient.objects.ObjectLibrary;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Sprite;
import flash.events.MouseEvent;

import org.osflash.signals.Signal;

public class MarketInventoryItem extends  Sprite {

    private var _itemType:uint;
    private var _slot:int;
    private var _itemBitmap:Bitmap;
    public var _onSelected:Signal;

    public function MarketInventoryItem(type:uint, slot:int) {
        this._onSelected = new Signal(uint, int);
        this._itemType = type;
        this._slot = slot;

        var bData:BitmapData = ObjectLibrary.getRedrawnTextureFromType(this._itemType, 250, false);

        this._itemBitmap = new Bitmap(bData);
        this._itemBitmap.x = 0;
        this._itemBitmap.y = 0;

        addChild(this._itemBitmap);

        graphics.lineStyle(2, 0xFFFFFF);
        graphics.beginFill(0);
        graphics.drawRect(0, 0, 125, 125);
        graphics.endFill();

        addEventListener(MouseEvent.CLICK, onClick);
        addEventListener(MouseEvent.MOUSE_OVER, onOver);
        addEventListener(MouseEvent.MOUSE_OUT, onOut);
    }

    private function onOver(e:MouseEvent):void {
        graphics.clear();
        graphics.lineStyle(2, 0xFFFFFF);
        graphics.beginFill(0x212121);
        graphics.drawRect(0, 0, 125, 125);
        graphics.endFill();
    }

    private function onOut(e:MouseEvent):void {
        graphics.clear();
        graphics.lineStyle(2, 0xFFFFFF);
        graphics.beginFill(0);
        graphics.drawRect(0, 0, 125, 125);
        graphics.endFill();
    }

    private function onClick(e:MouseEvent):void {
        this._onSelected.dispatch(this._itemType, this._slot);
    }
}
}
