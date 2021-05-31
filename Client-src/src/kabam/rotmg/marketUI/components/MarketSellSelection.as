package kabam.rotmg.marketUI.components {
import com.company.assembleegameclient.game.AGameSprite;

import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.marketUI.components.items.MarketInventoryItem;

import org.osflash.signals.Signal;

public class MarketSellSelection extends Sprite {

    private var _gameSprite:AGameSprite;
    public var onItemSelected:Signal;

    public function MarketSellSelection(gs:AGameSprite) {
        this._gameSprite = gs;
        this.onItemSelected = new Signal(uint, int);

        initInventory();

        graphics.beginFill(0, .6);
        graphics.drawRect(0,0, 800, 600);
        graphics.endFill();

        addEventListener(MouseEvent.CLICK, onClick);
    }

    private function initInventory():void {
        var localX:int = 120;
        var localY:int = 20;

        for (var i:int = 4; i < this._gameSprite.map.player_.equipment_.length; i++) {
            var type:int = this._gameSprite.map.player_.equipment_[i];

            if (type == -1) {
                continue;
            }

            var invItem:MarketInventoryItem = new MarketInventoryItem(type, i);
            invItem._onSelected.add(onSelected);

            invItem.x = localX;
            invItem.y = localY;

            addChild(invItem);

            localX += invItem.width + 5;
            if (localX + invItem.width + 5 > 680) {
                localX = 120;
                localY += invItem.height + 5;
            }
        }
    }

    private function onClick(e:MouseEvent):void {
        remove();
    }

    private function onSelected(type:uint, slot:int):void {
        this.onItemSelected.dispatch(type, slot);
    }

    private function remove():void {
        while(this.numChildren > 0) {
            removeChildAt(0);
        }
        this._gameSprite.removeChild(this);
    }

}
}
