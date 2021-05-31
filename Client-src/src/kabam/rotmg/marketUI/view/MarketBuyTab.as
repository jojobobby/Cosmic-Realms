package kabam.rotmg.marketUI.view {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;

import kabam.rotmg.marketUI.components.items.MarketBuyItem;
import kabam.rotmg.marketUI.signals.MarketBuySignal;

import kabam.rotmg.marketUI.signals.MarketSearchSignal;
import kabam.rotmg.marketUI.signals.NewPageSignal;
import kabam.rotmg.marketUI.signals.PriceSortSignal;
import kabam.rotmg.marketUI.signals.SearchSortSignal;
import kabam.rotmg.marketUI.signals.UpdatePageNagivator;
import kabam.rotmg.marketUI.util.MarketUtil;
import kabam.rotmg.memMarket.utils.DialogUtils;
import kabam.rotmg.messaging.impl.data.MarketData;
import kabam.rotmg.messaging.impl.incoming.market.MarketBuyResult;
import kabam.rotmg.messaging.impl.incoming.market.MarketSearchResult;

public class MarketBuyTab extends MarketTab {

    public const marketSearchSignal:MarketSearchSignal = new MarketSearchSignal();
    public const marketBuySignal:MarketBuySignal = new MarketBuySignal();
    public const priceSortSignal:PriceSortSignal = new PriceSortSignal();
    public const searchSortSignal:SearchSortSignal = new SearchSortSignal();
    public static const newPageSignal:NewPageSignal = new NewPageSignal();

    private var _curPage:int;
    private var _maxPage:int;

    private var _marketOffers:Vector.<MarketBuyItem>;

    private var _result:MarketSearchResult;
    private var _filteredResult:Vector.<MarketData>;

    private var _searchWord:String;
    private var _priceSort:int;

    public function MarketBuyTab(gs:AGameSprite, title:String) {
        this._marketOffers = new Vector.<MarketBuyItem>();
        this._filteredResult = new Vector.<MarketData>();
        marketSearchSignal.add(onSearchResult);
        marketBuySignal.add(onBuyResult);
        priceSortSignal.add(sortByPrice);
        searchSortSignal.add(sortBySearch);
        super(gs, title);
    }

    private function newPage(direction:int):void {
        switch(direction) {
            case NewPageSignal.forward: {
                this._curPage++;
            } break;
            case NewPageSignal.backward: {
                this._curPage--;
            } break;
        }

        UpdatePageNagivator._staticInstance.dispatch(this._curPage + 1, this._maxPage + 1);
        draw();
    }
    private function sortByPrice(type:int):void {
        this._priceSort = type;
        sortAndDraw(this._searchWord, this._priceSort);
    }
    private function sortBySearch(search:String):void {
        this._searchWord = search;
        sortAndDraw(this._searchWord, this._priceSort);
    }

    private function onBuyResult(result:MarketBuyResult):void {
        if (result.code_ != -1) {
            DialogUtils.makeSimpleDialog(this._gameSprite,  "Error", result.description_);
            return;
        }

        DialogUtils.makeSimpleDialog(this._gameSprite,  "Success", result.description_);
        this._gameSprite.gsc_.marketSearch(MarketUtil.CURTYPESORT, MarketUtil.CURRARITYSORT);
    }

    private function sortAndDraw(search:String, sort:int):void {
        this._filteredResult.length = 0;

        for (var i:int = 0; i < this._result.results_.length; i++) {
            var data:MarketData = this._result.results_[i];
            if (search.length > 0) {
                var name:String = ObjectLibrary.getIdFromType(data.itemType_).toLowerCase();

                if (name.indexOf(search.toLowerCase()) > -1) {
                    this._filteredResult.push(data);
                }
            } else {
                this._filteredResult.push(data);
            }
        }

        if (sort == 0) {
            this._filteredResult.sort(MarketUtil.HighToLow);
        } else{
            this._filteredResult.sort(MarketUtil.LowToHigh);
        }

        this._curPage = 0;
        this._maxPage = this._filteredResult.length / MarketUtil.MAX_OFFERS;
        UpdatePageNagivator._staticInstance.dispatch(this._curPage + 1, this._maxPage + 1);

        draw();
    }

    private function draw():void {
        clear();

        var maxOffers:int = this._filteredResult.length;
        var toDisplay:int = MarketUtil.MAX_OFFERS;

        if (this._curPage  == this._maxPage) {
            toDisplay = maxOffers - (this._maxPage * MarketUtil.MAX_OFFERS);
        }

        var offsetY:int = 3;
        for (var i:int = 0; i < toDisplay; i++) {
            var offerData = this._filteredResult[i + (this._curPage * MarketUtil.MAX_OFFERS)];
            var offerItem:MarketBuyItem = new MarketBuyItem(this._gameSprite, offerData.sellerName_, offerData.itemType_, offerData.price_, offerData.id_);
            offerItem.x = 0;
            offerItem.y = offsetY;
            offsetY += offerItem.height + 3;

            addChild(offerItem);
            this._marketOffers.push(offerItem);
        }
    }

    private function onSearchResult(result:MarketSearchResult):void {
        this._result = result;
        sortAndDraw(this._searchWord, this._priceSort);
    }

    private function clear():void {
        for each(var offer:MarketBuyItem in this._marketOffers) {
            removeChild(offer);
        }

        this._marketOffers.length = 0;
    }

    override public function OnTabOpen():void {
        this._curPage = 0;
        this._maxPage = 0;
        this._priceSort = 0;
        this._searchWord = "";
        newPageSignal.add(newPage);
        this._gameSprite.gsc_.marketSearch(0, 0); //search for all options
    }

    override public function OnTabClosed():void {
        clear();
        this._curPage = 0;
        this._maxPage = 0;
        this._priceSort = 0;
        this._searchWord = "";
        this._filteredResult.length = 0;
        newPageSignal.remove(newPage);
    }

    override public function Dispose():void {
        OnTabClosed();
        marketSearchSignal.remove(onSearchResult);
        searchSortSignal.remove(sortBySearch);
        priceSortSignal.remove(sortByPrice);
        marketBuySignal.remove(onBuyResult);
    }
}
}
