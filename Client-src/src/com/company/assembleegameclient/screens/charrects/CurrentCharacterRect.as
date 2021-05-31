package com.company.assembleegameclient.screens.charrects {
import com.company.assembleegameclient.appengine.CharacterStats;
import com.company.assembleegameclient.appengine.SavedCharacter;
import com.company.assembleegameclient.screens.events.DeleteCharacterEvent;
import com.company.assembleegameclient.ui.tooltip.MyPlayerToolTip;
import com.company.assembleegameclient.util.FameUtil;
import com.company.assembleegameclient.util.TierUtil;
import com.company.rotmg.graphics.DeleteXGraphic;
import com.company.ui.BaseSimpleText;

import flash.display.DisplayObject;
import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;
import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.classes.model.CharacterClass;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import org.osflash.signals.Signal;

public class CurrentCharacterRect extends CharacterRect {

    private static var toolTip_:MyPlayerToolTip = null;

    public const selected:Signal = new Signal();
    public const deleteCharacter:Signal = new Signal();
    public const showToolTip:Signal = new Signal(Sprite);
    public const hideTooltip:Signal = new Signal();

    public var charName:String;
    public var charStats:CharacterStats;
    public var char:SavedCharacter;
    public var myPlayerToolTipFactory:MyPlayerToolTipFactory;
    private var charType:CharacterClass;
    private var deleteButton:Sprite;
    private var icon:DisplayObject;
    protected var statsMaxedText:TextFieldDisplayConcrete;
    protected var fameText:BaseSimpleText;
    private var selectButton:SliceScalingButton;
    private var _stars:int;
    public function CurrentCharacterRect(_arg1:String, _arg2:CharacterClass, _arg3:SavedCharacter, _arg4:CharacterStats, _stars:int=0) {
        this.myPlayerToolTipFactory = new MyPlayerToolTipFactory();
        super();
        this.charName = _arg1;
        this.charType = _arg2;
        this.char = _arg3;
        this.charStats = _arg4;
        this._stars = _stars;
        var _local5:String = _arg2.name;
        var _local6:int = _arg3.charXML_.Level;
        super.className = new LineBuilder().setParams(TextKey.CURRENT_CHARACTER_DESCRIPTION, {
            "className": _local5,
            "level": _local6
        });
        super.color = 0x5C5C5C;
        super.overColor = 0x7F7F7F;
        super.init();
        this.makeTagline();
        this.makeDeleteButton();
        this.makeStatsMaxedText();
        this.makeSelectButton();
        this.addEventListeners();
    }


    private function addEventListeners():void {
        addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        selectContainer.addEventListener(MouseEvent.CLICK, this.onSelect);
        this.selectButton.addEventListener(MouseEvent.CLICK, this.onSelect)
        this.deleteButton.addEventListener(MouseEvent.CLICK, this.onDelete);
    }

    private function onSelect(_arg1:MouseEvent):void {
        this.selected.dispatch(this.char);
    }

    private function onDelete(_arg1:MouseEvent):void {
        this.deleteCharacter.dispatch(this.char);
    }

    public function makeSelectButton(): void
    {
        this.selectButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        var local2:SliceScalingBitmap = TextureParser.instance.getSliceScalingBitmap("UI", "main_button_decoration");

        this.selectButton.setLabel("Select", DefaultLabelFormat.questButtonCompleteLabel);
        this.selectButton.width = 100;
        this.selectButton.x = 50;
        this.selectButton.y = 155;
        addChild(this.selectButton);

        local2.width = 150;
        local2.x = 25;
        local2.y = 150;
        addChild(local2);
    }

    public function setIcon(_arg1:DisplayObject):void {
        ((this.icon) && (selectContainer.removeChild(this.icon)));
        this.icon = _arg1;
        this.icon.x = 100 - (this.icon.width / 2);
        this.icon.y = 65;
        ((this.icon) && (selectContainer.addChild(this.icon)));
    }

    private function makeTagline():void {
        if (this.getNextStarFame() > 0) {
            if (this.char.fame() > 2000)
                super.makeTaglineText(new StaticStringBuilder(this.char.fame() + ""));
            else
                super.makeTaglineText(new StaticStringBuilder(this.char.fame() + "/" + this.getNextStarFame()));
            super.makeTagLineCharacterScreen(getStarByFame());
        }
        else {
            super.makeTaglineText(new LineBuilder().setParams(TextKey.CURRENT_CHARACTER_TAGLINE_NOQUEST, {"fame": this.char.fame()}));
            super.makeTagLineCharacterScreen(getStarByFame());
        }
    }
    private function getStarByFame():int{
        var fame:int = this.charStats.bestFame();
        var star:int;
        if(_stars >= 80)
            star = 6;
        else if (fame < 20)
            star = 0;
        else if (fame >= 20 && fame < 150)
            star = 1;
        else if (fame >= 150 && fame < 400)
            star = 2;
        else if (fame >= 400 && fame < 800)
            star = 3;
        else if (fame >= 800 && fame < 2000)
            star = 4;
        else if (fame >= 2000)
            star = 5;
        else
            star = 0;
        return star;
    }
    private function getNextStarFame():int {
        return (FameUtil.nextStarFame((((this.charStats == null)) ? 0 : this.charStats.bestFame()), this.char.fame()));
    }

    private function makeStatsMaxedText():void
    {
        var maxTxt:int = this.MaxText();
        var color:* = 11776947;
        this.statsMaxedText = new TextFieldDisplayConcrete().setSize(15).setColor(color).setBold(true).setAutoSize(TextFieldAutoSize.CENTER);
        this.statsMaxedText.x = 100;
        this.statsMaxedText.y = 50;
        if(maxTxt <= 10){
            this.statsMaxedText.setStringBuilder(new StaticStringBuilder(maxTxt+"/10"));
            color = uint(11776947);
        }
        else if(maxTxt > 10 && maxTxt < 20){
            this.statsMaxedText.setStringBuilder(new StaticStringBuilder(maxTxt+"/20"));
            color = uint(0xffd700);
        }
        else if(maxTxt >= 20){
            this.statsMaxedText.setStringBuilder(new StaticStringBuilder("MAXED"));
            color = uint(0x8672b2);
        }
        this.statsMaxedText.setColor(color);
        this.statsMaxedText && selectContainer.addChild(this.statsMaxedText);
    }

    private function MaxText():int {
        var locl:int = 0;
        if ((this.char.hp() >= this.charType.hp.max) && (this.char.hp() < this.charType.hp.max + 50)) {
            locl++;
        }
        if ((this.char.mp() >= this.charType.mp.max) && (this.char.mp() < this.charType.mp.max + 50)) {
            locl++;
        }
        if ((this.char.att() >= this.charType.attack.max) && (this.char.att() < this.charType.attack.max +10)) {
            locl++;
        }
        if ((this.char.def() >= this.charType.defense.max) && (this.char.def() < this.charType.defense.max +10)) {
            locl++;
        }
        if ((this.char.wis() >= this.charType.mpRegeneration.max) && (this.char.wis() < this.charType.mpRegeneration.max +10)) {
            locl++;
        }
        if ((this.char.vit() >= this.charType.hpRegeneration.max) && (this.char.vit() < this.charType.hpRegeneration.max + 10)) {
            locl++;
        }
        if ((this.char.dex() >= this.charType.dexterity.max) && (this.char.dex() < this.charType.dexterity.max +10)) {
            locl++;
        }
        if ((this.char.spd() >= this.charType.speed.max) && (this.char.spd() < this.charType.speed.max + 10)) {
            locl++;
        }
        if ((this.char.critDmg() == this.charType.CriticalDmg.max) && (this.char.critDmg() < this.charType.CriticalDmg.max +10)) {
            locl++;
        }
        if ((this.char.critHit() == this.charType.CriticalHit.max) && (this.char.critHit() < this.charType.CriticalHit.max +10)) {
            locl++;
        }
        if (this.char.hp() >= (this.charType.hp.max + 50)) {
            locl+=2;
        }
        if (this.char.mp() >= (this.charType.mp.max + 50)) {
            locl+=2;
        }
        if (this.char.att() >= (this.charType.attack.max+10)) {
            locl+=2;
        }
        if (this.char.def() >= (this.charType.defense.max+10)) {
            locl+=2;
        }
        if (this.char.wis() >= (this.charType.mpRegeneration.max+10)) {
            locl+=2;
        }
        if (this.char.vit() >= (this.charType.hpRegeneration.max+10)) {
            locl+=2;
        }
        if (this.char.dex() >= (this.charType.dexterity.max+10)) {
            locl+=2;
        }
        if (this.char.spd() >= (this.charType.speed.max+10)) {
            locl+=2;
        }
        if (this.char.critDmg() >= (this.charType.CriticalDmg.max+10)) {
            locl+=2;
        }
        if (this.char.critHit() >= (this.charType.CriticalHit.max+10)) {
            locl+=2;
        }
        return locl;
    }

    private function makeDeleteButton() : void
    {
        this.deleteButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","close_button"));
        this.deleteButton.scaleX = 0.8;
        this.deleteButton.scaleY = 0.8;
        this.deleteButton.x = 165;
        this.deleteButton.y = 48;
        addChild(this.deleteButton);
    }

    override protected function onMouseOver(_arg1:MouseEvent):void {
        super.onMouseOver(_arg1);
        this.removeToolTip();
        toolTip_ = this.myPlayerToolTipFactory.create(this.charName, this.char.charXML_, this.charStats);
        toolTip_.createUI();
        this.showToolTip.dispatch(toolTip_);
    }

    override protected function onRollOut(_arg1:MouseEvent):void {
        super.onRollOut(_arg1);
        this.removeToolTip();
    }

    private function onRemovedFromStage(_arg1:Event):void {
        this.removeToolTip();
    }

    private function removeToolTip():void {
        this.hideTooltip.dispatch();
    }

    private function onDeleteDown(_arg1:MouseEvent):void {
        _arg1.stopImmediatePropagation();
        dispatchEvent(new DeleteCharacterEvent(this.char));
    }


}
}
