package com.company.assembleegameclient.screens.charrects {
import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.display.Shape;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;

import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.assets.services.IconFactory;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

public class BuyCharacterRect extends CharacterRect {

    public static const BUY_CHARACTER_RECT_CLASS_NAME_TEXT:String = "BuyCharacterRect.classNameText";

    private var model:PlayerModel;

    public function BuyCharacterRect(_arg1:PlayerModel) {
        this.model = _arg1;
        super.color = 0x1F1F1F;
        super.overColor = 0x424242;
        className = new StaticStringBuilder("Char. Slot #" + (this.model.getMaxCharacters() + 1));
        super.init();
        this.makeIcon();
        this.makePriceText();
        this.makeCurrency();
    }

    private function makeCurrency():void {
        var dat:BitmapData = this.model.getCharSlotCurrency() == 0 ?
                IconFactory.makeCoin() :
                IconFactory.makeFame();
        var cur:Bitmap = new Bitmap(dat);
        cur.scaleX = cur.scaleY = 1.5;
        cur.x = 100 - (cur.width / 2);
        cur.y = 115;
        selectContainer.addChild(cur);
    }

    private function makePriceText():void {
        var _local1:TextFieldDisplayConcrete;
        _local1 = new TextFieldDisplayConcrete().setSize(18).setBold(true).setColor(0xFFFFFF).setAutoSize(TextFieldAutoSize.CENTER);
        _local1.setStringBuilder(new StaticStringBuilder(this.model.getCharSlotPrice().toString()));
        _local1.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
        _local1.x = 100;
        _local1.y = 90;
        selectContainer.addChild(_local1);
    }

    private function makeIcon() : void
    {
        var _loc1_:* = null;
        _loc1_ = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","add_button"));
        _loc1_.scaleX = 0.6;
        _loc1_.scaleY = 0.6;
        _loc1_.x = 170;
        _loc1_.y = 50;
        addChild(_loc1_);
    }


}
}
