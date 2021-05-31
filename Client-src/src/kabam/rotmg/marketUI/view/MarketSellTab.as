package kabam.rotmg.marketUI.view {
import com.company.assembleegameclient.game.AGameSprite;

import kabam.rotmg.marketUI.components.items.MarketSellItem;
import kabam.rotmg.marketUI.signals.MarketOffersSignal;
import kabam.rotmg.marketUI.signals.MarketRemoveSignal;
import kabam.rotmg.marketUI.signals.NewPageSignal;
import kabam.rotmg.marketUI.signals.UpdatePageNagivator;
import kabam.rotmg.marketUI.util.MarketUtil;
import kabam.rotmg.memMarket.utils.DialogUtils;
import kabam.rotmg.messaging.impl.data.MarketData;

import kabam.rotmg.messaging.impl.incoming.market.MarketMyOffersResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketRemoveResult;

public class MarketSellTab extends MarketTab {
    private const _marketOffersSignal:MarketOffersSignal = new MarketOffersSignal();
    private const _marketRemoveSignal:MarketRemoveSignal = new MarketRemoveSignal();

    private var _marketOffers:Vector.<MarketSellItem>;

    private var _result:MarketMyOffersResult;

    private var _curPage:int;
    private var _maxPage:int;

    public function MarketSellTab(gs:AGameSprite, title:String) {
        this._marketOffers = new Vector.<MarketSellItem>();
        this._marketOffersSignal.add(showOffers);
        this._marketRemoveSignal.add(onRemove);
        super(gs, title);

    }

    private function onNewPage(direction:int):void {
        switch(direction) {
            case NewPageSignal.forward: {
                this._curPage++;
            }   break;
            case NewPageSignal.backward: {
                this._curPage--;
            } break;
        }

        UpdatePageNagivator._staticInstance.dispatch(this._curPage + 1, this._maxPage + 1);
        draw();
    }

    override public function OnTabOpen():void {
        MarketBuyTab.newPageSignal.add(onNewPage);
        this._gameSprite.gsc_.marketMyOffers();
    }

    private function onRemove(remove:MarketRemoveResult):void {
        DialogUtils.makeSimpleDialog(this._gameSprite,  "Error", remove.description_);
    }

    private function draw():void {
        for each(var prevOffer:MarketSellItem in this._marketOffers) {
            removeChild(prevOffer);
        }

        this._marketOffers.length = 0;

        var maxOffers:int = this._result.results_.length;
        var toDisplay:int = MarketUtil.MAX_OFFERS;

        if (this._curPage == this._maxPage) {
            toDisplay = maxOffers - (this._maxPage * MarketUtil.MAX_OFFERS);
        }

        var yOffset:int = 3;
        for (var i:int = 0; i < toDisplay; i++) {
            var data:MarketData = this._result.results_[i + (this._curPage * MarketUtil.MAX_OFFERS)];
            var offer:MarketSellItem = new MarketSellItem(this._gameSprite, data.itemType_, data.price_, data.id_);

            offer.x = 0;
            offer.y = yOffset;
            yOffset += offer.height + 3;

            addChild(offer);
            this._marketOffers.push(offer);
        }
    }

    private function showOffers(offer:MarketMyOffersResult):void {
        this._result = offer;
        this._curPage = 0;
        this._maxPage = offer.results_.length / MarketUtil.MAX_OFFERS;
        this._marketOffers.length = 0;
        UpdatePageNagivator._staticInstance.dispatch(this._curPage + 1, this._maxPage + 1);

        draw();
    }

    override public function OnTabClosed():void {
        for each(var offer:MarketSellItem in this._marketOffers) {
            removeChild(offer);
        }
        this._marketOffers.length = 0;
        this._curPage = 0;
        this._maxPage = 0;
        MarketBuyTab.newPageSignal.remove(onNewPage);
    }

    override public function Dispose():void {
        OnTabClosed();
        this._marketOffersSignal.remove(showOffers);
        this._marketRemoveSignal.remove(onRemove);
    }
}
}
