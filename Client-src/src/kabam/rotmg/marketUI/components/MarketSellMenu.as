package kabam.rotmg.marketUI.components {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.assembleegameclient.ui.MarketInput;

import flash.display.Bitmap;

import flash.display.Sprite;
import flash.events.MouseEvent;

import kabam.rotmg.marketUI.signals.MarketAddSignal;
import kabam.rotmg.memMarket.utils.DialogUtils;
import kabam.rotmg.messaging.impl.incoming.market.MarketAddResult;

public class MarketSellMenu extends Sprite {

    public const _marketAddSignal:MarketAddSignal = new MarketAddSignal();

    private var _sellSlot:Sprite;
    private var _priceField:MarketInput;
    private var _sellButton:DeprecatedTextButton;
    private var _sellIcon:Bitmap;

    private var _gameSprite:AGameSprite;

    private var _currentType:uint;
    private var _curSlot:int;

    public function MarketSellMenu(gs:AGameSprite) {
        this._gameSprite = gs;
        this._curSlot = -1;
        this._sellSlot = new Sprite(); //for now
        this._priceField = new MarketInput("Price:", false, "", 16, 140, 20);
        this._sellButton = new DeprecatedTextButton(16, "Sell");

        this._sellSlot.graphics.lineStyle(2, 0);
        this._sellSlot.graphics.beginFill(0x232323);
        this._sellSlot.graphics.drawRect(0, 0, 50, 50);
        this._sellSlot.graphics.endFill();

        this._sellSlot.x = (150 - this._sellSlot.width) / 2;
        this._sellSlot.y = 20;
        this._priceField.x = 5;
        this._priceField.y = 100;
        this._sellButton.x = 55;
        this._sellButton.y = 160;

        addChild(this._sellSlot);
        addChild(this._priceField);
        addChild(this._sellButton);

        this._sellSlot.addEventListener(MouseEvent.CLICK, onSellClick);
        this._sellButton.addEventListener(MouseEvent.CLICK, onSell);
        this._marketAddSignal.add(onSellResult);
    }

    public function reset():void {
        this._priceField.inputText_.setText("");
        this._priceField.inputText_.updateMetrics();
        if (this._curSlot != -1 && this._sellSlot != null) {
            this._sellSlot.removeChild(this._sellIcon);
            this._curSlot = -1;
        }
        this._sellIcon = null;
    }

    private function onSellResult(result:MarketAddResult):void {
        if (result.code_ != -1) { //invalid result
            DialogUtils.makeSimpleDialog(this._gameSprite,  "Error", result.description_);
            return;
        }

        //success
        reset();

        this._gameSprite.gsc_.marketMyOffers();
        DialogUtils.makeSimpleDialog(this._gameSprite,  "Success", result.description_);
    }

    private function onSell(e:MouseEvent):void {
        this._gameSprite.gsc_.marketAdd(this._curSlot, int(this._priceField.text()));
    }

    private function onSellClick(e:MouseEvent):void {
        //open up item selection tab
        var selection:MarketSellSelection = new MarketSellSelection(this._gameSprite);
        selection.x = 0;
        selection.y = 0;
        selection.onItemSelected.add(onItemSelected);

        this._gameSprite.addChild(selection);
    }

    private function onItemSelected(type:uint, slot:int):void {
        if (this._sellIcon) {
            this._sellSlot.removeChild(this._sellIcon);
        }

        this._currentType = type;
        this._curSlot = slot;
        this._sellIcon = new Bitmap(ObjectLibrary.getRedrawnTextureFromType(this._currentType, 75, false));
        this._sellSlot.addChild(this._sellIcon);
    }
}
}
