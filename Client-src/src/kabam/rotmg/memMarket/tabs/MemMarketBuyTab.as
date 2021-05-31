package kabam.rotmg.memMarket.tabs {

import com.company.assembleegameclient.ui.MarketInput;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.dropdown.DropDown;

import flash.display.Shape;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.KeyboardEvent;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.memMarket.content.MemMarketBuyItem;
import kabam.rotmg.memMarket.signals.MemMarketSearchSignal;
import kabam.rotmg.messaging.impl.data.MarketData;
import kabam.rotmg.messaging.impl.incoming.market.MarketSearchResult;

import robotlegs.bender.framework.api.ILogger;

public class MemMarketBuyTab extends MemMarketTab {

        private static const DROPDOWN_LIST:Vector.<String> = new <String>[
          "Weapon",
          "Ability",
          "Armor",
          "Ring",
          "Misc"
        ];

        private static const DROPDOWN2_LIST:Vector.<String> = new <String> [
          "Least > Greatest", "Greatest > Least"
        ];

        public const SEARCH_RESULT_SIGNAL:MemMarketSearchSignal = new MemMarketSearchSignal();

        private static const X_OFFSET:int = 5;
        private static const Y_OFFSET:int = 110;

        private static const MASK_Y_OFFSET:int = 145;
        private static const MASK_WIDTH:int = 770;
        private static const MASK_HEIGHT:int = 370;

        private var _searchBar:MarketInput;

        private var _sortDrop:DropDown; //Type
        private var _sort2Drop:DropDown; //Price

        private var _sortType:int;
        private var _sortBy:String;

        private var _offers:Vector.<MemMarketBuyItem>;

        private var _offerBG:Sprite;
        private var _bgMask:Sprite;
        private var _bgShape:Shape;

        private var _scrollBar:Scrollbar;

        public function MemMarketBuyTab(gameSprite:GameSprite) {
            super(gameSprite, 0);

            this._offers = new Vector.<MemMarketBuyItem>();

            this._searchBar = new MarketInput("", false, "");

            this._sortDrop = new DropDown(DROPDOWN_LIST, 100, 30);
            this._sort2Drop = new DropDown(DROPDOWN2_LIST, 120, 30);

            this._sortDrop.x = X_OFFSET;
            this._sortDrop.y = Y_OFFSET;

            this._sort2Drop.x = X_OFFSET + this._sortDrop.width + 2;
            this._sort2Drop.y = Y_OFFSET;

            this._searchBar.x = (X_OFFSET * 2) + (this._sortDrop.width + this._sort2Drop.width);
            this._searchBar.y = Y_OFFSET;

            this._sortDrop.addCallBack(onSort);
            this._sort2Drop.addCallBack(onSort2);
            this._searchBar.addEventListener(KeyboardEvent.KEY_UP, onKeyUp);

            this._bgShape = new Shape();
            this._bgShape.graphics.beginFill(0);
            this._bgShape.graphics.drawRect(X_OFFSET, MASK_Y_OFFSET, MASK_WIDTH, MASK_HEIGHT);
            this._bgShape.graphics.endFill();

            this._bgMask = new Sprite();
            this._bgMask.graphics.beginFill(0x0, .3);
            this._bgMask.graphics.drawRect(X_OFFSET, MASK_Y_OFFSET, MASK_WIDTH, MASK_HEIGHT);
            this._bgMask.graphics.endFill();
            this._bgMask.addChild(this._bgShape);
            addChild(this._bgMask);
            this._bgMask.mask = this._bgShape;

            this._offerBG = new Sprite();
            this._bgMask.addChild(this._offerBG);

            addChild(this._searchBar);
            addChild(this._sortDrop);
            addChild(this._sort2Drop);

            this.SEARCH_RESULT_SIGNAL.add(onSearchResult);

            this._sortType = sortValue(this._sortDrop.getValue());
            this._sortBy = this._sort2Drop.getValue();

            //SENDS THE INITIAL PAGE REQUEST
            sendSearch();
        }

        private function onSearchResult(data:MarketSearchResult):void {
            clear();
            this._offers.length = 0;

            //HANDLE RESULT DATA
            for each(var offer:MarketData in data.results_) {
                var item:MemMarketBuyItem = new MemMarketBuyItem(this.gameSprite_, offer);

                this._offers.push(item);
            }

            sort();
            draw();
        }

        private function onSort(sortTo:String):void {
            var _sortTo:int = sortValue(sortTo);

            if (this._sortType != _sortTo) {
                this._sortType = _sortTo;

                sendSearch();
            }
        }

        private function onSort2(sortTo:String):void {
            this._sortBy = sortTo;
            sort();
            //draw
            draw();
        }

        private function sort():void {
            if (this._sortBy == DROPDOWN2_LIST[0]) {
                if (tempOffer != null) {
                    tempOffer.sort(sortFuncAscending);
                }
                this._offers.sort(sortFuncAscending);
            } else {
                if (tempOffer != null) {
                    tempOffer.sort(sortFuncDescending);
                }
                this._offers.sort(sortFuncDescending);
            }
        }

        private function sortFuncDescending(a:MemMarketBuyItem, b:MemMarketBuyItem):Number {
            if (a.price_ < b.price_) {
                return 1;
            } else if (a.price_ > b.price_) {
                return -1;
            }

            return 0;
        }
        private function sortFuncAscending(a:MemMarketBuyItem, b:MemMarketBuyItem):Number {
            if (a.price_ < b.price_) {
                return -1;
            } else if (a.price_ > b.price_) {
                return 1;
            }

            return 0;
        }

        private var tempOffer:Vector.<MemMarketBuyItem>;

        private function onKeyUp(e:KeyboardEvent): void {
            clear();

            var searchTxt:String = this._searchBar.inputText_.text;
            this.tempOffer = new Vector.<MemMarketBuyItem>();

            if (searchTxt.length > 0) {
                for each(var offer:MemMarketBuyItem in this._offers) {
                    var id:String = ObjectLibrary.typeToDisplayId_[offer.itemType_].toLowerCase();

                    if (id.indexOf(searchTxt.toLowerCase()) > -1) {
                        this.tempOffer.push(offer);
                    }
                }
            } else {
                this.tempOffer = this._offers;
            }

            sort();
            draw();
        }

        private function draw(): void {
            var offers:Vector.<MemMarketBuyItem>;

            if (this.tempOffer != null && this.tempOffer.length > 0) {
                offers = this.tempOffer;
            } else {
                offers = this._offers;
            }

            //RESET SCROLL BAR
            if (this._scrollBar != null) {
                this._scrollBar.removeEventListener(Event.CHANGE, onScrollChange);
                removeChild(this._scrollBar);
                this._scrollBar = null;
            }

            var localX:int = X_OFFSET + 5;
            var localY:int = MASK_Y_OFFSET + 5;

            var maxX:int = X_OFFSET + MASK_WIDTH;

            for each(var offer:MemMarketBuyItem in offers) {
                if (localX > maxX) {
                    localX = X_OFFSET + 5;
                    localY = localY + offer.height + 5;
                }
                //DRAW THE CURRENT OFFERS
                offer.x = localX;
                offer.y = localY;

                this._offerBG.addChild(offer);

                localX = localX + offer.width + 4;
            }

            if (localY > MASK_HEIGHT) {
                this._scrollBar = new Scrollbar(6, MASK_HEIGHT);

                this._scrollBar.x = MASK_WIDTH + 2;
                this._scrollBar.y = MASK_Y_OFFSET + 5;

                this._scrollBar.setIndicatorSize(MASK_HEIGHT, this._offerBG.height);
                this._scrollBar.addEventListener(Event.CHANGE, onScrollChange);

                addChild(this._scrollBar);
            }
        }

        private function onScrollChange(e:Event): void {
            this._offerBG.y = -this._scrollBar.pos() * (this._offerBG.height - 356);
        }

        private function clear(): void {
            this._offerBG.removeChildren();
        }

        private function sortValue(sortTo:String): int {
            switch(sortTo) {
                case "Weapon":
                    return 0;
                case "Ability":
                    return 1;
                case "Armor":
                    return 2;
                case "Ring":
                    return 3;
                case "Misc":
                    return 4;
            }
            return -1;
        }

        private function sendSearch():void {
            //this.gameSprite_.gsc_.marketSearch(this._sortType);
        }
    }
}


/*public class MemMarketBuyTab extends MemMarketTab
{
    private static const SEARCH_X_OFFSET:int = 4;
    private static const SEARCH_Y_OFFSET:int = 170;
    private static const SEARCH_ITEM_SIZE:int = 50;

    private static const RESULT_X_OFFSET:int = 270;
    private static const RESULT_Y_OFFSET:int = 105;

    /!* Signals *!/
    public const buySignal_:MemMarketBuySignal = new MemMarketBuySignal();
    public const searchSignal_:MemMarketSearchSignal = new MemMarketSearchSignal();

    private var searchField_:MarketInput;
    private var shape_:Shape;
    private var searchMask_:Sprite;
    private var searchBackground:Sprite;
    private var searchItems:Vector.<MemMarketItem>;
    private var searchScroll:Scrollbar;

    private var resultMask_:Sprite;
    private var resultBackground_:Sprite;
    private var resultItems_:Vector.<MemMarketBuyItem>;
    private var resultScroll_:Scrollbar;

    private var sortChoices_:DropDown;

    public function MemMarketBuyTab(gameSprite:GameSprite)
    {
        super(gameSprite);

        /!* Initialize signals *!/
        this.buySignal_.add(this.onBuy);
        this.searchSignal_.add(this.onSearch);

        this.searchField_ = new MarketInput("Search", false, "");
        this.searchField_.x = SEARCH_X_OFFSET + 9;
        this.searchField_.y = 101;
        this.searchField_.addEventListener(KeyboardEvent.KEY_UP, this.onKeyUp);
        addChild(this.searchField_);

        this.shape_ = new Shape();
        this.shape_.graphics.beginFill(0);
        this.shape_.graphics.drawRect(SEARCH_X_OFFSET, SEARCH_Y_OFFSET, 250, 350);
        this.shape_.graphics.endFill();
        this.searchMask_ = new Sprite();
        this.searchMask_.addChild(this.shape_);
        this.searchMask_.mask = this.shape_;
        addChild(this.searchMask_);
        this.searchBackground = new Sprite();
        this.searchMask_.addChild(this.searchBackground);

        this.searchItems = new Vector.<MemMarketItem>();

        this.shape_ = new Shape();
        this.shape_.graphics.beginFill(0);
        this.shape_.graphics.drawRect(RESULT_X_OFFSET, RESULT_Y_OFFSET, 500, 415);
        this.shape_.graphics.endFill();
        this.resultMask_ = new Sprite();
        this.resultMask_.addChild(this.shape_);
        this.resultMask_.mask = this.shape_;
        addChild(this.resultMask_);
        this.resultBackground_ = new Sprite();
        this.resultMask_.addChild(this.resultBackground_);

        this.resultItems_ = new Vector.<MemMarketBuyItem>();

        this.sortChoices_ = new DropDown(SortUtils.SORT_CHOICES, 200, 26);
        this.sortChoices_.x = 597;
        this.sortChoices_.y = 71;
        this.sortChoices_.setValue(SortUtils.LOWEST_TO_HIGHEST);
        this.sortChoices_.addEventListener(Event.CHANGE, this.onSortChoicesChanged);
        addChild(this.sortChoices_);

        this.searchItemsFunc(true);
    }

    private function onSortChoicesChanged(event:Event) : void
    {
        this.sortOffers();
    }

    private function onKeyUp(event:KeyboardEvent) : void
    {
        if (event.keyCode == KeyCodes.ENTER)
        {
            this.searchItemsFunc();
        }
    }


    private function onSearchClick(event:MouseEvent) : void
    {
        var item:MemMarketItem = event.currentTarget as MemMarketItem;
        this.gameSprite_.gsc_.marketSearch(item.itemType_);
    }

    private function onSearchScrollChanged(event:Event) : void
    {
        this.searchBackground.y = -this.searchScroll.pos() * (this.searchBackground.height - 356);
    }

    /!* Clear previous results *!/
    private function clearPreviousResults(result:Boolean) : void
    {
        if (result)
        {
            for each (var i:MemMarketBuyItem in this.resultItems_)
            {
                i.dispose();
                this.resultBackground_.removeChild(i);
                i = null;
            }
            this.resultItems_.length = 0;
        }
        else
        {
            for each (var o:MemMarketItem in this.searchItems)
            {
                o.removeEventListener(MouseEvent.CLICK, this.onSearchClick);
                o.dispose();
                this.searchBackground.removeChild(o);
                o = null;
            }
            this.searchItems.length = 0;
        }
    }

    /!* Removes an offer from resultItems and sorts *!/
    private function removeOffer(id:int) : void
    {
        var index:int = 0;
        for each (var o:MemMarketBuyItem in this.resultItems_)
        {
            if (o.id_ == id) /!* Item matched, remove *!/
            {
                this.resultItems_.splice(index, 1);
                o.dispose();
                o.parent.removeChild(o);
                o = null;
                break; /!* No need to continue the loop after we got what we looked for *!/
            }
            index++;
        }

        this.sortOffers();
    }

    /!* Sorts and positions offers *!/
    private function sortOffers() : void
    {
        switch (SortUtils.SORT_CHOICES[this.sortChoices_.getIndex()])
        {
            case SortUtils.LOWEST_TO_HIGHEST:
                this.resultItems_.sort(SortUtils.lowestToHighest);
                break;
            case SortUtils.HIGHEST_TO_LOWEST:
                this.resultItems_.sort(SortUtils.highestToLowest);
                break;
            /!*case SortUtils.FAME_TO_GOLD:
                this.resultItems_.sort(SortUtils.fameToGold);
                break;
            case SortUtils.GOLD_TO_FAME:
                this.resultItems_.sort(SortUtils.goldToFame);
                break; *!/
            case SortUtils.JUST_ADDED:
                this.resultItems_.sort(SortUtils.justAdded);
                break;
            case SortUtils.ENDING_SOON:
                this.resultItems_.sort(SortUtils.endingSoon);
                break;
        }

        var index:int = 0;
        for each (var i:MemMarketBuyItem in this.resultItems_)
        {
            i.x = MemMarketItem.OFFER_WIDTH * int(index % 5) + RESULT_X_OFFSET;
            i.y = MemMarketItem.OFFER_HEIGHT * int(index / 5) + RESULT_Y_OFFSET;
            index++;
        }
    }
    private function searchItemsFunc(first:Boolean = false) : void
    {
        /!* Remove old scrollbar *!/
        if (this.searchScroll != null)
        {
            this.searchScroll.removeEventListener(Event.CHANGE, this.onSearchScrollChanged);
            removeChild(this.searchScroll);
            this.searchScroll = null;
        }

        if (!StringUtil.trim(this.searchField_.text()) && !first) /!* Clear results if empty *!/
        {
            this.clearPreviousResults(false);
            return;
        }

        this.clearPreviousResults(false);

        var index:int = 0;
        if (first)
        {
            for each (var w:String in ObjectLibrary.preloadedCustom_)
            {
                if (ItemUtils.isBanned(ObjectLibrary.idToTypeItems_[w])) /!* Skip on banned items *!/
                {
                    continue;
                }

                var preloaded:MemMarketItem = new MemMarketItem(this.gameSprite_, SEARCH_ITEM_SIZE, SEARCH_ITEM_SIZE, 80, ObjectLibrary.idToTypeItems_[w], null);
                preloaded.x = SEARCH_ITEM_SIZE * int(index % 5) + SEARCH_X_OFFSET;
                preloaded.y = SEARCH_ITEM_SIZE * int(index / 5) + SEARCH_Y_OFFSET;
                preloaded.addEventListener(MouseEvent.CLICK, this.onSearchClick);
                this.searchItems.push(preloaded);
                index++;
            }
        }
        else
        {
            for each (var i:String in ObjectLibrary.typeToIdItems_)
            {
                if (i.indexOf(this.searchField_.text().toLowerCase()) >= 0)
                {
                    if (ItemUtils.isBanned(ObjectLibrary.idToTypeItems_[i])) /!* Skip on banned items *!/
                    {
                        continue;
                    }

                    var item:MemMarketItem = new MemMarketItem(this.gameSprite_, SEARCH_ITEM_SIZE, SEARCH_ITEM_SIZE, 80, ObjectLibrary.idToTypeItems_[i], null);
                    item.x = SEARCH_ITEM_SIZE * int(index % 5) + SEARCH_X_OFFSET;
                    item.y = SEARCH_ITEM_SIZE * int(index / 5) + SEARCH_Y_OFFSET;
                    item.addEventListener(MouseEvent.CLICK, this.onSearchClick);
                    this.searchItems.push(item);
                    index++;
                }
            }
        }

        for each (var x:MemMarketItem in this.searchItems) /!* Draw our results *!/
        {
            this.searchBackground.addChild(x);
        }

        this.searchBackground.y = 0; /!* Reset height *!/
        if (this.searchBackground.height > 350)
        {
            this.searchScroll = new Scrollbar(6, 350);
            this.searchScroll.x = 258;
            this.searchScroll.y = SEARCH_Y_OFFSET;
            this.searchScroll.setIndicatorSize(350, this.searchBackground.height);
            this.searchScroll.addEventListener(Event.CHANGE, this.onSearchScrollChanged);
            addChild(this.searchScroll);
        }
    }
    private function refreshOffers() : void
    {
        for each (var o:MemMarketBuyItem in this.resultItems_)
        {
            o.updateButton();
        }
    }
    /!* Buy and refresh offers *!/
    private function onBuy(result:MarketBuyResult) : void
    {
        if (result.code_ != -1)
        {
            DialogUtils.makeSimpleDialog(this.gameSprite_, "Error", result.description_);
            return;
        }

        this.removeOffer(result.offerId_);
        this.refreshOffers();

        DialogUtils.makeSimpleDialog(this.gameSprite_, "Success", result.description_);
    }

    /!* Refresh and add found offers *!/
    private function onSearch(result:MarketSearchResult) : void
    {
        if (result.description_ != "")
        {
            this.clearPreviousResults(true);
            DialogUtils.makeSimpleDialog(this.gameSprite_, "Error", result.description_);
            return;
        }

        /!* Remove old scrollbar *!/
        if (this.resultScroll_ != null)
        {
            this.resultScroll_.removeEventListener(Event.CHANGE, this.onResultScrollChanged);
            removeChild(this.resultScroll_);
            this.resultScroll_ = null;
        }

        this.clearPreviousResults(true);

        for each (var i:MarketData in result.results_)
        {
            var item:MemMarketBuyItem = new MemMarketBuyItem(this.gameSprite_, i);
            this.resultItems_.push(item);
        }

        this.sortOffers();

        for each (var o:MemMarketBuyItem in this.resultItems_)
        {
            this.resultBackground_.addChild(o);
        }

        this.resultBackground_.y = 0; /!* Reset height *!/
        if (this.resultBackground_.height > 436)
        {
            this.resultScroll_ = new Scrollbar(22, 415);
            this.resultScroll_.x = 774;
            this.resultScroll_.y = RESULT_Y_OFFSET;
            this.resultScroll_.setIndicatorSize(415, this.resultBackground_.height);
            this.resultScroll_.addEventListener(Event.CHANGE, this.onResultScrollChanged);
            addChild(this.resultScroll_);
        }
    }

    private function onResultScrollChanged(event:Event) : void
    {
        this.resultBackground_.y = -this.resultScroll_.pos() * (this.resultBackground_.height - 418);
    }

    /!* Clear *!/
    public override function dispose() : void
    {
        this.buySignal_.remove(this.onBuy);
        this.searchSignal_.remove(this.onSearch);

        this.searchField_.removeEventListener(KeyboardEvent.KEY_UP, this.onKeyUp);
        this.searchField_ = null;

        this.shape_.parent.removeChild(this.shape_);
        this.shape_ = null;

        this.clearPreviousResults(false);
        this.searchItems = null;
        this.searchMask_.removeChild(this.searchBackground);
        this.searchMask_ = null;
        this.searchBackground = null;

        if (this.searchScroll != null)
        {
            this.searchScroll.removeEventListener(Event.CHANGE, this.onSearchScrollChanged);
            this.searchScroll = null;
        }

        this.clearPreviousResults(true);
        this.resultItems_ = null;

        this.resultMask_.removeChild(this.resultBackground_);
        this.resultMask_ = null;
        this.resultBackground_ = null;

        if (this.resultScroll_ != null)
        {
            this.resultScroll_.removeEventListener(Event.CHANGE, this.onResultScrollChanged);
            this.resultScroll_ = null;
        }

        this.sortChoices_.removeEventListener(Event.CHANGE, this.onSortChoicesChanged);
        this.sortChoices_ = null;

        super.dispose();
    }
}
}*/
