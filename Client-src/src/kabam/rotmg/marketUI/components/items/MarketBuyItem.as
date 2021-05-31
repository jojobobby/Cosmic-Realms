package kabam.rotmg.marketUI.components.items {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.ui.DeprecatedTextButton;
import com.company.ui.BaseSimpleText;

import flash.events.MouseEvent;

public class MarketBuyItem extends MarketItemBase {

    private var _buyButton:DeprecatedTextButton;
    private var _seller:BaseSimpleText;

    public function MarketBuyItem(gs:AGameSprite, seller:String, type:int, price:int, id:int) {

        this._buyButton = new DeprecatedTextButton(16, "Purchase");
        this._buyButton.x = 500;
        this._buyButton.y = 15;

        addChild(this._buyButton);

        this._seller = new BaseSimpleText(14, 0xFFFFFF);
        this._seller.setText("from: " + seller);
        this._seller.updateMetrics();
        this._seller.x = 130;
        this._seller.y = 27;

        addChild(this._seller);

        super(gs, type, price, id);

        this._buyButton.addEventListener(MouseEvent.CLICK, onBuy);
    }

    private function onBuy(e:MouseEvent):void {
        this._gs.gsc_.marketBuy(this._id);
    }

}
}
