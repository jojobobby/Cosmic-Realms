package kabam.rotmg.marketUI.components.items {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;

import flash.events.MouseEvent;

public class MarketSellItem extends MarketItemBase {

    private var _removeButton:DeprecatedTextButton;

    public function MarketSellItem(gs:AGameSprite, type:int, price:int, id:int) {

        this._removeButton = new DeprecatedTextButton(16, "Remove");

        this._removeButton.x = 520;

        this._removeButton.y = 15;

        addChild(this._removeButton);

        super(gs, type, price, id);

        this._removeButton.addEventListener(MouseEvent.CLICK, onRemove);
    }

    private function onRemove(e:MouseEvent):void {
        this._gs.gsc_.marketRemove(this._id);
    }
}
}
