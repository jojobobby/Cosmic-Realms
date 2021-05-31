package kabam.rotmg.potionStorage
{
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.ui.BaseSimpleText;

import flash.display.Bitmap;
import flash.display.Graphics;
import flash.display.Sprite;

import flash.events.MouseEvent;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import kabam.rotmg.ui.view.PotionStorage_bg;

import spark.primitives.Graphic;

public class PotionStorage extends Sprite
{
    private var gameSprite_:GameSprite;
    private var titleText_:BaseSimpleText;
    private var closeButton_:SliceScalingButton;
    private var output:int;

    private var consumeButton_ = new Vector.<Sprite>;
    private var maxButton_ = new Vector.<Sprite>;
    private var icon_ = new Vector.<Bitmap>;
    private var objId_ = new Vector.<uint>;
    private var text_ = new Vector.<TextFieldDisplayConcrete>;

    private var bar_ = new Vector.<Sprite>;
    private var barColor_ = new Vector.<uint>;
    private var type_ = new Vector.<int>;

    public function PotionStorage(gameSprite:GameSprite)
    {
        this.gameSprite_ = gameSprite;
        this.makeScreenElements();
        this.makeUpgrades();
        this.makePotions();
        this.addListeners();
    }

    public function makeUpgrades(width:int = 120): void
    {
        var _loc1_ = TextureParser.instance.getSliceScalingBitmap("UI","main_button_decoration",170);
        _loc1_.x = 315;
        _loc1_.y = 224;
        addChild(_loc1_);

        var _loc2_ = TextureParser.instance.getSliceScalingBitmap("UI","main_button_decoration",170);
        _loc2_.x = 315;
        _loc2_.y = 324;
        addChild(_loc2_);

        var _loc3_ = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        _loc3_.width = width;
        _loc3_.x = 340;
        _loc3_.y = 230;
        _loc3_.setLabel("Earth++",DefaultLabelFormat.questButtonCompleteLabel);
        _loc3_.addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { upgrade(false, true, false) });

        var _loc4_ = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        _loc4_.width = width;
        _loc4_.x = 340;
        _loc4_.y = 330;
        _loc4_.setLabel("Lunar++",DefaultLabelFormat.questButtonCompleteLabel);
        _loc4_.addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { upgrade(true, false, false) });

        addChild(_loc3_);
        addChild(_loc4_);
    }

    public function makePotions(): void {
        /* icon IDS */
        this.objId_[0] = 0xae9;
        this.objId_[1] = 0xaea;
        this.objId_[2] = 0xa1f;
        this.objId_[3] = 0xa20;
        this.objId_[4] = 0xa21;
        this.objId_[5] = 0xa4c;
        this.objId_[6] = 0xa34;
        this.objId_[7] = 0xa35;
        this.objId_[8] = 0x70a1;
        this.objId_[9] = 0x4103;
        this.objId_[10] = 0x4104;
        this.objId_[11] = 0x4408;

        /* bar colors */
        this.barColor_[0] = 0x5edddd;
        this.barColor_[1] = 0xffea55;
        this.barColor_[2] = 0xe87ee8;
        this.barColor_[3] = 0x686868;
        this.barColor_[4] = 0x70e08c;
        this.barColor_[5] = 0xfd9d3e;
        this.barColor_[6] = 0xf53434;
        this.barColor_[7] = 0x77b5f2;
        this.barColor_[8] = 0x9ec526;
        this.barColor_[9] = 0x470e2a;
        this.barColor_[10] = 0x2d1b65;
        this.barColor_[11] = 0xf5cbf5;

        /* create buttons */
        for (var h:int = 0; h < 12; h++) {
            this.consumeButton_.push(new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "add_button")));
            this.maxButton_.push(new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI", "upgrade_button")));
            this.icon_.push(new Bitmap(ObjectLibrary.getRedrawnTextureFromType(this.objId_[h], 40, true, true, 6)));
            this.text_.push(new TextFieldDisplayConcrete().setBold(true).setColor(0xffffff).setSize(10));
            this.bar_.push(new PotionStorageBar(this.barColor_[h], this.output));
        }

        switch (this.output)
        {
            case 0:
                this.consumeButton_[0].x = 363;
                this.consumeButton_[0].y = 77;
                this.maxButton_[0].x = 403;
                this.maxButton_[0].y = 77;
                this.icon_[0].x = 366;
                this.icon_[0].y = 25;
                this.text_[0].x = 398;
                this.text_[0].y = 41;
                this.text_[0].setStringBuilder(new StaticStringBuilder("HP"));
                this.bar_[0].x = 363;
                this.bar_[0].y = 57;
            case 1:
                this.consumeButton_[1].x = 251;
                this.consumeButton_[1].y = 107;
                this.maxButton_[1].x = 291;
                this.maxButton_[1].y = 107;
                this.icon_[1].x = 254;
                this.icon_[1].y = 55;
                this.text_[1].x = 286;
                this.text_[1].y = 71;
                this.text_[1].setStringBuilder(new StaticStringBuilder("MP"));
                this.bar_[1].x = 251;
                this.bar_[1].y = 87;
            case 2:
                this.consumeButton_[2].x = 168;
                this.consumeButton_[2].y = 189;
                this.maxButton_[2].x = 208;
                this.maxButton_[2].y = 189;
                this.icon_[2].x = 171;
                this.icon_[2].y = 137;
                this.text_[2].x = 203;
                this.text_[2].y = 153;
                this.text_[2].setStringBuilder(new StaticStringBuilder("ATT"));
                this.bar_[2].x = 168;
                this.bar_[2].y = 169;
            case 3:
                this.consumeButton_[3].x = 137;
                this.consumeButton_[3].y = 302;
                this.maxButton_[3].x = 177;
                this.maxButton_[3].y = 302;
                this.icon_[3].x = 140;
                this.icon_[3].y = 250;
                this.text_[3].x = 172;
                this.text_[3].y = 266;
                this.text_[3].setStringBuilder(new StaticStringBuilder("DEF"));
                this.bar_[3].x = 137;
                this.bar_[3].y = 282;
            case 4:
                this.consumeButton_[4].x = 169;
                this.consumeButton_[4].y = 414;
                this.maxButton_[4].x = 209;
                this.maxButton_[4].y = 414;
                this.icon_[4].x = 172;
                this.icon_[4].y = 362;
                this.text_[4].x = 204;
                this.text_[4].y = 378;
                this.text_[4].setStringBuilder(new StaticStringBuilder("SPD"));
                this.bar_[4].x = 169;
                this.bar_[4].y = 394;
            case 5:
                this.consumeButton_[5].x = 251;
                this.consumeButton_[5].y = 497;
                this.maxButton_[5].x = 291;
                this.maxButton_[5].y = 497;
                this.icon_[5].x = 254;
                this.icon_[5].y = 445;
                this.text_[5].x = 286;
                this.text_[5].y = 461;
                this.text_[5].setStringBuilder(new StaticStringBuilder("DEX"));
                this.bar_[5].x = 251;
                this.bar_[5].y = 477;
            case 6:
                this.consumeButton_[6].x = 363;
                this.consumeButton_[6].y = 527;
                this.maxButton_[6].x = 403;
                this.maxButton_[6].y = 527;
                this.icon_[6].x = 368;
                this.icon_[6].y = 475;
                this.text_[6].x = 400;
                this.text_[6].y = 491;
                this.text_[6].setStringBuilder(new StaticStringBuilder("VIT"));
                this.bar_[6].x = 363;
                this.bar_[6].y = 507;
            case 7:
                this.consumeButton_[7].x = 475;
                this.consumeButton_[7].y = 497;
                this.maxButton_[7].x = 515;
                this.maxButton_[7].y = 497;
                this.icon_[7].x = 478;
                this.icon_[7].y = 445;
                this.text_[7].x = 510;
                this.text_[7].y = 461;
                this.text_[7].setStringBuilder(new StaticStringBuilder("WIS"));
                this.bar_[7].x = 475;
                this.bar_[7].y = 477;
            case 8:
                this.consumeButton_[8].x = 558;
                this.consumeButton_[8].y = 414;
                this.maxButton_[8].x = 598;
                this.maxButton_[8].y = 414;
                this.icon_[8].x = 561;
                this.icon_[8].y = 362;
                this.text_[8].x = 593;
                this.text_[8].y = 378;
                this.text_[8].setStringBuilder(new StaticStringBuilder("LUC"));
                this.bar_[8].x = 558;
                this.bar_[8].y = 394;
            case 9:
                this.consumeButton_[9].x = 588;
                this.consumeButton_[9].y = 302;
                this.maxButton_[9].x = 628;
                this.maxButton_[9].y = 302;
                this.icon_[9].x = 591;
                this.icon_[9].y = 250;
                this.text_[9].x = 621;
                this.text_[9].y = 266;
                this.text_[9].setStringBuilder(new StaticStringBuilder("CDMG"));
                this.bar_[9].x = 588;
                this.bar_[9].y = 282;
            case 10:
                this.consumeButton_[10].x = 558;
                this.consumeButton_[10].y = 190;
                this.maxButton_[10].x = 598;
                this.maxButton_[10].y = 190;
                this.icon_[10].x = 561;
                this.icon_[10].y = 138;
                this.text_[10].x = 593;
                this.text_[10].y = 154;
                this.text_[10].setStringBuilder(new StaticStringBuilder("CHIT"));
                this.bar_[10].x = 558;
                this.bar_[10].y = 170;
            case 11:
                this.consumeButton_[11].x = 496;
                this.consumeButton_[11].y = 107;
                this.icon_[11].x = 476;
                this.icon_[11].y = 55;
                this.text_[11].x = 507;
                this.text_[11].y = 71;
                this.text_[11].setStringBuilder(new StaticStringBuilder("ENH"));
                this.bar_[11].x = 476;
                this.bar_[11].y = 87;
        }

        for (var i:int = 0; i < 12; i++) {
            /* add to stage */
            addChild(this.consumeButton_[i]);
            addChild(this.maxButton_[i]);
            addChild(this.icon_[i]);
            addChild(this.text_[i]);
            addChild(this.bar_[i]);
        }
        removeChild(this.maxButton_[11]);
    }

    public function addListeners(): void
    {
        /* consumes */
        this.consumeButton_[0].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(0, false) });
        this.consumeButton_[1].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(1, false) });
        this.consumeButton_[2].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(2, false) });
        this.consumeButton_[3].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(3, false) });
        this.consumeButton_[4].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(4, false) });
        this.consumeButton_[5].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(5, false) });
        this.consumeButton_[6].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(6, false) });
        this.consumeButton_[7].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(7, false) });
        this.consumeButton_[8].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(10, false) });
        this.consumeButton_[9].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(11, false) });
        this.consumeButton_[10].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(12, false) });
        this.consumeButton_[11].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { upgrade(false, false, true) });

        /* maxes */
        this.maxButton_[0].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(0, true) });
        this.maxButton_[1].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(1, true) });
        this.maxButton_[2].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(2, true) });
        this.maxButton_[3].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(3, true) });
        this.maxButton_[4].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(4, true) });
        this.maxButton_[5].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(5, true) });
        this.maxButton_[6].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(6, true) });
        this.maxButton_[7].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(7, true) });
        this.maxButton_[8].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(10, true) });
        this.maxButton_[9].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(11, true) });
        this.maxButton_[10].addEventListener(MouseEvent.CLICK, function(e:MouseEvent) : void { consume(12, true) });
    }

    public function makeScreenElements(): void
    {
        /* background */
        graphics.clear();
        graphics.beginFill(2829099,0.6);
        graphics.drawRect(0,0,800,600);
        graphics.endFill();

        /* bitmap */
        addChild(new PotionStorage_bg());

        /* close button */
        this.closeButton_ = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
        this.closeButton_.x = 730;
        this.closeButton_.y = 35;
        addChild(this.closeButton_);
        this.closeButton_.addEventListener(MouseEvent.CLICK, this.onClose);

        /* title */
        this.titleText_ = new BaseSimpleText(24, 0xFFFFFF, false, 800, 0);
        this.titleText_.setBold(true);
        this.titleText_.setText("  Potion\n Storage");
        this.titleText_.x = 40;
        this.titleText_.y = 40;
        addChild(this.titleText_);
    }

    private function consume(t:int, m:Boolean)
    {
        /* send packet */
        this.gameSprite_.gsc_.potions(t, m);
    }

    private function upgrade(l:Boolean, e:Boolean, en:Boolean)
    {
        /* send packet */
        this.gameSprite_.gsc_.upgrade(l, e, en);
    }

    private function onClose(e:MouseEvent):void {
        /* remove UI */
        this.parent.removeChild(this);
        this.gameSprite_.mui_.setEnablePlayerInput(true);
        this.closeButton_.removeEventListener(MouseEvent.CLICK, this.onClose);
    }
}
}