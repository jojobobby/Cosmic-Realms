package kabam.rotmg.memMarket {
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.screens.TitleMenuOption;
import com.company.assembleegameclient.ui.options.OptionsTabTitle;
import com.company.rotmg.graphics.ScreenGraphic;
import com.company.ui.BaseSimpleText;

import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.memMarket.tabs.MemMarketBuyTab;
import kabam.rotmg.memMarket.tabs.MemMarketSellTab;
import kabam.rotmg.memMarket.tabs.MemMarketTab;

public class MemMarket extends Sprite
{
    private static const BUY:String = "Buy";
    private static const SELL:String = "Sell";
    private static const TABS:Vector.<String> = new <String>[BUY, SELL];

    private var gameSprite_:GameSprite;
    private var titleText_:BaseSimpleText;
    private var closeButton_:TitleMenuOption;
    private var tabs_:Vector.<OptionsTabTitle>;
    private var content_:Vector.<MemMarketTab>;
    private var selectedTab_:OptionsTabTitle;
    private var creatorText_:BaseSimpleText;

    public function MemMarket(gameSprite:GameSprite)
    {
        this.gameSprite_ = gameSprite;

        /* Draw background */
        graphics.clear();
        graphics.beginFill(2829099,0.8);
        graphics.drawRect(0,0,800,600);
        graphics.endFill();
        graphics.lineStyle(1,6184542);
        graphics.moveTo(0,100);
        graphics.lineTo(800,100);
        graphics.lineStyle();

        /* Draw title */
        this.titleText_ = new BaseSimpleText(36, 0xFFFFFF, false, 800, 0);
        this.titleText_.setBold(true);
        this.titleText_.htmlText = "<p align=\"center\">Market</p>";
        this.titleText_.autoSize = TextFieldAutoSize.CENTER;
        this.titleText_.filters = [new DropShadowFilter(0,0,0)];
        this.titleText_.updateMetrics();
        this.titleText_.x = 800 / 2 - this.titleText_.width / 2;
        this.titleText_.y = 8;
        addChild(this.titleText_);

        /* Draw Menubar */
        addChild(new ScreenGraphic());

        /* Draw buttons */
        this.closeButton_ = new TitleMenuOption("Close", 36, false);
        this.closeButton_.x = 710 / 2 - this.closeButton_.width / 2;
        this.closeButton_.y = 530;
        this.closeButton_.addEventListener(MouseEvent.CLICK, this.onClose);
        addChild(this.closeButton_);

        /* Add tabs */
        this.tabs_ = new Vector.<OptionsTabTitle>();
        var xOffset:int = 14;
        for (var i:int = 0; i < TABS.length; i++)
        {
            var tab:OptionsTabTitle = new OptionsTabTitle(TABS[i]);
            tab.x = xOffset;
            tab.y = 78;
            tab.addEventListener(MouseEvent.CLICK, this.onTab);
            addChild(tab);
            this.tabs_.push(tab);
            xOffset += 108;
        }

        this.content_ = new Vector.<MemMarketTab>();

        /* Set tab to first in list. */
        this.setTab(this.tabs_[0]);

        this.creatorText_ = new BaseSimpleText(16, 0xFFFFFF, false, 200);
        this.creatorText_.setBold(true);
        this.creatorText_.text = "";
        this.creatorText_.y = 582;
        addChild(this.creatorText_);
    }

    /* Change tab */
    private function onTab(event:MouseEvent) : void
    {
        var tab:OptionsTabTitle = (event.currentTarget as OptionsTabTitle);
        this.setTab(tab);
    }

    /* Replace tab content */
    private function setTab(tab:OptionsTabTitle) : void
    {
        if (tab == this.selectedTab_)
        {
            return;
        }

        if (this.selectedTab_ != null)
        {
            this.selectedTab_.setSelected(false);
        }

        this.selectedTab_ = tab;
        this.selectedTab_.setSelected(true);

        this.removeLastContent();

        switch (this.selectedTab_.text_) /* Could potentially make this slightly faster by using pre-made tabs instead of creating new ones */
        {
            case SELL:
                this.addContent(new MemMarketSellTab(this.gameSprite_));
                break;
            case BUY:
                this.addContent(new MemMarketBuyTab(this.gameSprite_));
                break;
        }
    }

    /* Remove last tab content */
    private function removeLastContent() : void
    {
        for each (var i:MemMarketTab in this.content_)
        {
            i.dispose(); /* Clear the tab */
            removeChild(i); /* Remove it */
        }
        this.content_.length = 0;
    }

    /* Add tab content */
    private function addContent(content:MemMarketTab) : void
    {
        addChild(content);
        this.content_.push(content);
    }

    /* Remove */
    private function onClose(event:Event) : void
    {
        this.gameSprite_.mui_.setEnablePlayerInput(true); /* Enable player movement */
        this.gameSprite_ = null;
        this.titleText_ = null;
        this.closeButton_.removeEventListener(MouseEvent.CLICK, this.onClose);
        this.closeButton_ = null;

        for each (var tab:OptionsTabTitle in this.tabs_)
        {
            tab.removeEventListener(MouseEvent.CLICK, this.onTab);
            tab = null;
        }
        this.tabs_.length = 0;
        this.tabs_ = null;

        for each (var content:MemMarketTab in this.content_)
        {
            content.dispose(); /* Clear the tab */
            content = null;
        }
        this.content_.length = 0;
        this.content_ = null;

        this.selectedTab_ = null;

        this.creatorText_ = null;

        /* Remove all children */
        for (var i:int = numChildren - 1; i >= 0; i--)
        {
            removeChildAt(i);
        }

        stage.focus = null;
        parent.removeChild(this);
    }
}
}
