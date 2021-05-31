package com.company.assembleegameclient.screens.charrects {
import com.company.rotmg.graphics.StarGraphic;

import flash.display.Shape;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.geom.ColorTransform;
import flash.text.TextFieldAutoSize;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;
import io.decagames.rotmg.utils.colors.Tint;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StringBuilder;

import mx.core.BitmapAsset;

import starling.text.BitmapChar;

public class CharacterRect extends Sprite {

    public static const WIDTH:int = 210;
    public static const HEIGHT:int = 210;

    public var color:uint;
    public var overColor:uint;
    private var box_:Sprite;
    protected var taglineIcon:Sprite;
    protected var taglineText:TextFieldDisplayConcrete;
    protected var classNameText:TextFieldDisplayConcrete;
    protected var className:StringBuilder;
    public var selectContainer:Sprite;
    private var container:BitmapAsset;

    public function CharacterRect() {
        super();
    }

    private static function makeDropShadowFilter():Array {
        return ([new DropShadowFilter(0, 0, 0, 1, 8, 8)]);
    }

    public function init():void {
        tabChildren = false;
        this.makeBox();
        this.makeContainer();
        this.makeClassNameText();
        this.addEventListeners();
    }

    private function addEventListeners():void {
        addEventListener("mouseOver",this.onMouseOver);
        addEventListener("rollOut",this.onRollOut);
    }

    public function makeBox() : void
    {
        this.box_ = new Sprite();
        this.box_.graphics.clear();
        this.box_.graphics.beginFill(0, 0);
        this.box_.graphics.drawRect(0, 0, WIDTH, HEIGHT);
        this.box_.graphics.endFill();
        addChild(this.box_);

        this.container = new CharacterRect_Container();
        this.container.y = 16;
        this.box_.addChild(this.container);
    }

    protected function onMouseOver(_arg1:MouseEvent):void {
        this.drawBox(true);
    }

    protected function onRollOut(_arg1:MouseEvent):void {
        this.drawBox(false);
    }

    private function drawBox(over:Boolean) : void
    {
        if(over) {
            Tint.add(this.box_,13224393,0.2);
        }
        else {
            this.box_.transform.colorTransform = new ColorTransform();
        }
        this.box_.scaleX = 1;
        this.box_.scaleY = 1;
        this.box_.x = 0;
        this.box_.y = 0;
    }

    public function makeContainer():void {
        this.selectContainer = new Sprite();
        this.selectContainer.mouseChildren = false;
        this.selectContainer.buttonMode = true;
        this.selectContainer.graphics.beginFill(0xFF00FF, 0);
        this.selectContainer.graphics.drawRoundRect(0, 0, WIDTH, HEIGHT, 20, 20);
        addChild(this.selectContainer);
    }

    protected function makeTaglineIcon():void {
        this.taglineIcon = new StarGraphic();
        this.taglineIcon.transform.colorTransform = new ColorTransform((179 / 0xFF), (179 / 0xFF), (179 / 0xFF));
        this.taglineIcon.scaleX = 0.9;
        this.taglineIcon.scaleY = 0.9;
        this.taglineIcon.x = 100 - (this.taglineIcon.width / 2);
        this.taglineIcon.y = 132;
        this.taglineIcon.filters = [new DropShadowFilter(0, 0, 0)];
        this.selectContainer.addChild(this.taglineIcon);
    }
    protected function makeTagLineCharacterScreen(star:int = 0):void {
        this.taglineIcon = new StarGraphic();
        this.taglineIcon.transform.colorTransform = new ColorTransform((179 / 0xFF), (179 / 0xFF), (179 / 0xFF));
        switch(star){
            case 0:
                this.taglineIcon.transform.colorTransform = new ColorTransform((179 / 0xFF), (179 / 0xFF), (179 / 0xFF));
                break;
            case 1:
                this.taglineIcon.transform.colorTransform = new ColorTransform((137 / 0xFF), (151 / 0xFF), (221 / 0xFF));
                break;
            case 2:
                this.taglineIcon.transform.colorTransform = new ColorTransform((48 / 0xFF), (76 / 0xFF), (218 / 0xFF));
                break;
            case 3:
                this.taglineIcon.transform.colorTransform = new ColorTransform((192 / 0xFF), (38 / 0xFF), (44 / 0xFF));
                break;
            case 4:
                this.taglineIcon.transform.colorTransform = new ColorTransform((246 / 0xFF), (146 / 0xFF), (29 / 0xFF));
                break;
            case 5:
                this.taglineIcon.transform.colorTransform = new ColorTransform((255 / 0xFF), (255 / 0xFF), (0 / 0xFF));
                break;
            case 6:
                this.taglineIcon.transform.colorTransform = new ColorTransform((255 / 0xFF), (255 / 0xFF), (255 / 0xFF));
                break;
            default:
                this.taglineIcon.transform.colorTransform = new ColorTransform((179 / 0xFF), (179 / 0xFF), (179 / 0xFF));
                break;

        }
        this.taglineIcon.x = 100 - (this.taglineIcon.width / 2);
        this.taglineIcon.y = 132;
        this.taglineIcon.filters = [new DropShadowFilter(0, 0, 0)];
        this.selectContainer.addChild(this.taglineIcon);
    }

    protected function makeClassNameText():void {
        this.classNameText = new TextFieldDisplayConcrete().setSize(13).setColor(0xFFFFFF).setAutoSize(TextFieldAutoSize.CENTER);
        this.classNameText.setBold(true);
        this.classNameText.setStringBuilder(this.className);
        this.classNameText.filters = makeDropShadowFilter();
        this.classNameText.x = 100;
        this.classNameText.y = 22;
        this.selectContainer.addChild(this.classNameText);
    }

    protected function makeTaglineText(_arg1:StringBuilder):void {
        this.taglineText = new TextFieldDisplayConcrete().setSize(14).setColor(0xB3B3B3).setAutoSize(TextFieldAutoSize.CENTER).setBold(true);
        this.taglineText.setStringBuilder(_arg1);
        this.taglineText.filters = makeDropShadowFilter();
        this.taglineText.x = 100;
        this.taglineText.y = 115;
        this.selectContainer.addChild(this.taglineText);
    }


}
}
