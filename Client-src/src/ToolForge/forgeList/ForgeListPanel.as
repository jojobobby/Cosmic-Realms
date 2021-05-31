package ToolForge.forgeList {

import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.ui.Scrollbar;
import com.company.assembleegameclient.ui.dropdown.DropDown;
import com.company.util.CachingColorTransformer;

import flash.display.Bitmap;

import flash.display.DisplayObject;
import flash.display.Graphics;

import flash.display.Shape;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.geom.ColorTransform;

import kabam.rotmg.messaging.impl.incoming.ForgeListResult;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import org.osflash.signals.Signal;

public class ForgeListPanel extends Sprite {


    private var titleTxt:TextFieldDisplayConcrete;
    private var closeTxt:TextFieldDisplayConcrete;

    private var listMask:Sprite;
    private var backGround:Sprite;
    private var maskFilter:Shape;

    private var topLine:Shape;

    private var weaponBar:Sprite;
    private var abilBar:Sprite;
    private var armorBar:Sprite;
    private var ringBar:Sprite;
    private var otherBar:Sprite;

    private var scrollBar:Scrollbar;

    public static var resultSignal:Signal;

    private var gameSprite:GameSprite;

    public function ForgeListPanel(gs:GameSprite) {

        graphics.beginFill(0, .8);
        graphics.drawRect(0, 0, 600, 600);
        graphics.lineStyle(2, 0xFFFFFF);
        graphics.endFill();

        gameSprite = gs;

        resultSignal = new Signal(ForgeListResult);
        resultSignal.add(onResult);

        this.titleTxt = new TextFieldDisplayConcrete();
        this.titleTxt.setColor(0xFFFFFF);
        this.titleTxt.setSize(21);
        this.titleTxt.setBold(true);
        this.titleTxt.setStringBuilder(new StaticStringBuilder("Forge List"));
        this.closeTxt = new TextFieldDisplayConcrete();
        this.closeTxt.setColor(0xFFFFFF);
        this.closeTxt.setSize(21);
        this.closeTxt.setBold(true);
        this.closeTxt.setStringBuilder(new StaticStringBuilder("Close"));
        this.closeTxt.setPosition(540, 0);
        this.closeTxt.addEventListener(MouseEvent.CLICK, onClose);

        this.listMask = new Sprite();
        this.listMask.graphics.lineStyle(5, 0xFFFFFF);
        this.listMask.graphics.drawRect(0, 0, 510, 500);

        this.maskFilter = new Shape();
        this.maskFilter.graphics.beginFill(0);
        this.maskFilter.graphics.drawRect(0, 0, 510, 500);
        this.maskFilter.graphics.endFill();

        this.listMask.addChild(this.maskFilter);
        this.listMask.mask = this.maskFilter;
        this.listMask.x = 65;
        this.listMask.y = 50;
        this.backGround = new Sprite();
        this.listMask.addChild(this.backGround);

        this.topLine = new Shape();
        this.topLine.graphics.lineStyle(3, 0xFFFFFF);
        this.topLine.graphics.moveTo(0, 30);
        this.topLine.graphics.lineTo(600, 30);

        addChild(this.topLine);

        addChild(this.listMask);
        addChild(this.titleTxt);
        addChild(this.closeTxt);

        addBar();

        this.gameSprite.gsc_.forgeList(0);
    }

    private function addBar():void {
        this.weaponBar =  barSprite(0xFFFFFF, 0xa6c);
        this.abilBar =  barSprite(0x363636, 0xa08);
        this.armorBar =  barSprite(0x363636, 0xa7b);
        this.ringBar =  barSprite(0x363636, 0xa27);
        this.otherBar =  barSprite(0x363636, 0xccc);

        this.weaponBar.x = 5, this.armorBar.x = 5, this.abilBar.x = 5, this.ringBar.x = 5, this.otherBar.x = 5;
        this.weaponBar.y = 50, this.abilBar.y = 112, this.armorBar.y = 174, this.ringBar.y = 236, this.otherBar.y = 298;

        this.weaponBar.addEventListener(MouseEvent.CLICK, onBarSelect);
        this.abilBar.addEventListener(MouseEvent.CLICK, onBarSelect);
        this.armorBar.addEventListener(MouseEvent.CLICK, onBarSelect);
        this.ringBar.addEventListener(MouseEvent.CLICK, onBarSelect);
        this.otherBar.addEventListener(MouseEvent.CLICK, onBarSelect)

        addChild(this.weaponBar);
        addChild(this.abilBar);
        addChild(this.armorBar);
        addChild(this.ringBar);
        addChild(this.otherBar);
    }

    private var curBar:int = 0;

    private function onBarSelect(e:MouseEvent):void {
        if (e.target == this.weaponBar && this.curBar != 0) {
            changeTab(0);
        } else if (e.target == this.abilBar && this.curBar != 1) {
            changeTab(1);
        } else if (e.target == this.armorBar && this.curBar != 2) {
            changeTab(2);
        } else if (e.target == this.ringBar && this.curBar != 3) {
            changeTab(3);
        } else if (e.target == this.otherBar && this.curBar != 4) {
            changeTab(4);
        }

        resetColor(e.target as Sprite);
    }

    private function resetColor(s:Sprite):void {
        drawGraphics(this.weaponBar, 0x363636);
        drawGraphics(this.abilBar, 0x363636);
        drawGraphics(this.armorBar, 0x363636);
        drawGraphics(this.ringBar, 0x363636);
        drawGraphics(this.otherBar, 0x363636);
        drawGraphics(s, 0xFFFFFF);
    }

    private function changeTab(tab:int):void {
        this.curBar = tab;
        clear();

        this.gameSprite.gsc_.forgeList(this.curBar);
    }

    private function clear():void {
        this.backGround.removeChildren();
        this.stripY = 5;
    }

    private function barSprite(clr:uint = 0x363636, type:uint = 0xa6c):Sprite {
        var s:Sprite = new Sprite();
        var ph:Bitmap = new Bitmap(ObjectLibrary.getRedrawnTextureFromType(type, 80, true));
        drawGraphics(s, clr);
        s.addChild(ph);
        return s;
    }

    private function drawGraphics(s:Sprite, clr:uint):void {
        var g:Graphics = s.graphics;
        g.clear();
        g.beginFill(clr);
        g.drawRect(0, 0, 60, 60);
        g.endFill();
    }

    private var stripY:int = 5;
    private function onResult(result:ForgeListResult):void {
        var strip:ForgeListStrip = new ForgeListStrip(gameSprite, result);
        strip.x = 5;
        strip.y = stripY;
        this.stripY += strip.height + 5;

        var line:Sprite = new Sprite();
        line.graphics.lineStyle(3, 0xFFFFFF);
        line.graphics.moveTo(0, this.stripY);
        line.graphics.lineTo(this.listMask.width, this.stripY);

        this.stripY += 2;

        this.backGround.addChild(strip);
        this.backGround.addChild(line);

        if (this.scrollBar == null && stripY > 750) {
            this.scrollBar = new Scrollbar(6, 514);
            this.scrollBar.x = 580;
            this.scrollBar.y = 41;

            this.scrollBar.setIndicatorSize(this.maskFilter.height, this.stripY + 5);
            this.scrollBar.addEventListener(Event.CHANGE, onScroll);

            addChild(this.scrollBar);
        }
    }

    private function onScroll(e:Event):void {
        this.backGround.y = - this.scrollBar.pos() * (this.backGround.height - 500);
    }

    private function onClose(e:MouseEvent):void {
        clear();
        this.parent.removeChild(this);
    }
}
}
