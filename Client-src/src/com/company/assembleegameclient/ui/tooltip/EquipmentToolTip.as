package com.company.assembleegameclient.ui.tooltip {
import com.company.assembleegameclient.constants.InventoryOwnerTypes;
import com.company.assembleegameclient.game.events.KeyInfoResponseSignal;
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.LineBreakDesign;
import com.company.util.BitmapUtil;
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;
import com.company.assembleegameclient.util.ConditionEffect;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import com.company.util.KeyCodes;
import com.company.assembleegameclient.ui.tooltip.slotcomparisons.GeneralProjectileComparison
import com.company.assembleegameclient.objects.ObjectLibrary;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.panels.itemgrids.ItemGrid;
import com.company.assembleegameclient.util.FilterUtil;
import com.company.assembleegameclient.util.TierUtil;
import com.company.util.GraphicsUtil;
import flash.display.GraphicsPath;
import flash.display.GraphicsSolidFill;
import flash.display.IGraphicsData;
import flash.display.Shape;
import flash.display.Sprite;
import io.decagames.rotmg.ui.labels.UILabel;
import kabam.rotmg.constants.ItemConstants;

import flash.display.Bitmap;
import flash.display.BitmapData;
import flash.filters.DropShadowFilter;
import flash.utils.Dictionary;

import kabam.rotmg.constants.ActivationType;
import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.messaging.impl.data.StatData;
import kabam.rotmg.messaging.impl.incoming.KeyInfoResponse;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.text.view.stringBuilder.StringBuilder;
import kabam.rotmg.ui.model.HUDModel;

public class EquipmentToolTip extends ToolTip {

    private static const MAX_WIDTH:int = 240;

    public static var keyInfo:Dictionary = new Dictionary();
    public var processedTags:Dictionary;
    public var comparisonStringBuilder:AppendingLineBuilder;

    private var icon:Bitmap;
    public var titleText:TextFieldDisplayConcrete;
    private var tierText:TextFieldDisplayConcrete;
    private var descText:TextFieldDisplayConcrete;
    private var line1:LineBreakDesign;
    private var effectsText:TextFieldDisplayConcrete;
    private var line2:LineBreakDesign;
    private var restrictionsText:TextFieldDisplayConcrete;
    private var player:Player;
    private var isEquippable:Boolean = false;
    private var objectType:int;
    private var titleOverride:String;
    private var descriptionOverride:String;
    private var curItemXML:XML = null;
    private var objectXML:XML = null;
    private var slotTypeToTextBuilder:SlotComparisonFactory;
    private var restrictions:Vector.<Restriction>;
    private var effects:Vector.<Effect>;
    private var uniqueEffects:Vector.<Effect>;
    private var itemSlotTypeId:int;
    private var invType:int;
    private var inventoryOwnerType:String;
    private var isInventoryFull:Boolean;
    private var playerCanUse:Boolean;
    private var comparisonResults:SlotComparisonResult;
    private var powerText:TextFieldDisplayConcrete;
    private var keyInfoResponse:KeyInfoResponseSignal;
    private var originalObjectType:int;
    public var Outlineorno;
    public var WisHealNova;
    public var WisHealNovarange;

    public function EquipmentToolTip(_arg1:int, _arg2:Player, _arg3:int, _arg4:String) {
        var _local8:HUDModel;
        this.uniqueEffects = new Vector.<Effect>();
        this.objectType = _arg1;
        this.originalObjectType = this.objectType;
        this.player = _arg2;
        this.invType = _arg3;
        this.inventoryOwnerType = _arg4;
        this.isInventoryFull = ((_arg2) ? _arg2.isInventoryFull() : false);
        if ((((this.objectType >= 0x9000)) && ((this.objectType <= 0xF000)))) {
            this.objectType = 36863;
        }
        this.playerCanUse = ((_arg2) ? ObjectLibrary.isUsableByPlayer(this.objectType, _arg2) : true);
        var _local5:int = ((_arg2) ? ObjectLibrary.getMatchingSlotIndex(this.objectType, _arg2) : -1);

        var _local6:uint = ((((this.playerCanUse) || ((this.player == null)))) ? 0x363030 : 6036765);
        var _local7:uint = ((((this.playerCanUse) || ((_arg2 == null)))) ? 0x9B9B9B : 10965039);
        super(_local6, 1, _local7, 1, true);

        this.slotTypeToTextBuilder = new SlotComparisonFactory();
        this.objectXML = ObjectLibrary.xmlLibrary_[this.objectType];
        this.isEquippable = !((_local5 == -1));
        this.effects = new Vector.<Effect>();
        this.itemSlotTypeId = int(this.objectXML.SlotType);
        if (this.player == null) {
            this.curItemXML = this.objectXML;
        }
        else {
            if (this.isEquippable) {
                if (this.player.equipment_[_local5] != -1) {
                    this.curItemXML = ObjectLibrary.xmlLibrary_[this.player.equipment_[_local5]];
                }
            }
        }
        if(Parameters.data_.rarityBag) {
            this.addBagBackground();
        }
        this.addBackground();
        this.addIcon();

        if ((((this.originalObjectType >= 0x9000)) && ((this.originalObjectType <= 0xF000)))) {
            if (keyInfo[this.originalObjectType] == null) {
                this.addTitle();
                this.addDescriptionText();
                this.keyInfoResponse = StaticInjectorContext.getInjector().getInstance(KeyInfoResponseSignal);
                this.keyInfoResponse.add(this.onKeyInfoResponse);
                _local8 = StaticInjectorContext.getInjector().getInstance(HUDModel);
                _local8.gameSprite.gsc_.keyInfoRequest(this.originalObjectType);
            }
            else {
                this.titleOverride = (keyInfo[this.originalObjectType][0] + " Key");
                this.descriptionOverride = (((keyInfo[this.originalObjectType][1] + "\n") + "Created By: ") + keyInfo[this.originalObjectType][2]);
                this.addTitle();
                this.addDescriptionText();
            }
        }
        else {
            this.addTitle();
            this.addDescriptionText();
        }

        this.addTierText();
        this.handleWisMod();
        this.buildCategorySpecificText();
        this.addUniqueEffectsToList();
        this.addNumProjectilesTagsToEffectsList();
        this.addProjectileTagsToEffectsList();
        this.addActivateTagsToEffectsList();
        this.addActivateOnEquipTagsToEffectsList();
        this.addDoseTagsToEffectsList();
        this.addMpCostTagToEffectsList();
        this.addFameBonusTagToEffectsList();
        this.addCooldownBonusTagToEffectsList();
        this.makeEffectsList();
        this.makeLineTwo();
        this.makeRestrictionList();
        this.makeRestrictionText();
    }

    private function getoutlinetext() {
        if(Parameters.data_.itemTextOutline) {

            Outlineorno = FilterUtil.getTextOutlineFilter();
        }
        else {
            Outlineorno = FilterUtil.getStandardDropShadowFilter();
        }
        return (Outlineorno);
    }

    private function onKeyInfoResponse(_arg1:KeyInfoResponse):void {
        this.keyInfoResponse.remove(this.onKeyInfoResponse);
        this.removeTitle();
        this.removeDesc();
        this.titleOverride = _arg1.name;
        this.descriptionOverride = _arg1.description;
        keyInfo[this.originalObjectType] = [_arg1.name, _arg1.description, _arg1.creator];
        this.addTitle();
        this.addDescriptionText();
    }

    private function addUniqueEffectsToList():void {
        var _local1:XMLList;
        var _local2:XML;
        var _local3:String;
        var _local4:String;
        var _local5:String;
        var _local6:AppendingLineBuilder;
        if (this.objectXML.hasOwnProperty("ExtraTooltipData")) {
            _local1 = this.objectXML.ExtraTooltipData.EffectInfo;
            for each (_local2 in _local1) {
                _local3 = _local2.attribute("name");
                _local4 = _local2.attribute("description");
                _local5 = ((((_local3) && (_local4))) ? ": " : "\n");
                _local6 = new AppendingLineBuilder();
                if (_local3) {
                    _local6.pushParams(_local3);
                }
                if (_local4) {
                    _local6.pushParams(_local4, {}, TooltipHelper.getOpenTag(16777103), TooltipHelper.getCloseTag());
                }
                _local6.setDelimiter(_local5);
                this.uniqueEffects.push(new Effect(TextKey.BLANK, {"data": _local6}));
            }
        }
    }
    protected function getTextColor(_arg1:Number):uint {
        if (_arg1 < 0) {
            return (TooltipHelper.WORSE_COLOR);
        }
        if (_arg1 > 0) {
            return (TooltipHelper.BETTER_COLOR);
        }
        return (TooltipHelper.NO_DIFF_COLOR);
    }

    private function addIcon():void {
        var _local1:XML = ObjectLibrary.xmlLibrary_[0x4018];
        var _local2:int = 5;

        if ((((this.objectType == 4874)) || ((this.objectType == 4618)))) {
            _local2 = 8;
        }
        if (_local1.hasOwnProperty("ScaleValue")) {
            _local2 = _local1.ScaleValue;
        }

        var _local4:BitmapData = ObjectLibrary.getRedrawnTextureForItemGlowFromType(this.objectType, 80, true, true, 6);
        _local4 = BitmapUtil.cropToBitmapData(_local4, 5, 6.8, (_local4.width - 10), (_local4.height - 10), 0x800080);
        this.icon = new Bitmap(_local4);
        addChild(this.icon);

    }

    private function addBackground():void {
        var _local1:XML = ObjectLibrary.xmlLibrary_[this.objectType];
        var _local2:int = 5;

        if ((((this.objectType == 4874)) || ((this.objectType == 4618)))) {
            _local2 = 8;
        }
        if (_local1.hasOwnProperty("ScaleValue")) {
            _local2 = _local1.ScaleValue;
        }
        if (this.objectXML.hasOwnProperty("BG")) {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8429, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("LG")) {//0xFEFCD7
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8428, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("ST")) {//0xFEFCD7
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8431, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("MLG")) {//0xFEFCD7
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8430, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("MY")) {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8428, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("Tier")) {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8424, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("Lunar")) {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8426, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("@setType")) {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8425, 100, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("Epic")) {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x6274, 100, true, true, 6);
        }
        else {
            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x8427, 100, true, true, 6);

        }
        if(!Parameters.data_.rarityBackground) {

            var _local3:BitmapData = ObjectLibrary.getRedrawnTextureFromType(0x4a02, 100, true, true, 6);
        }

        _local3 = BitmapUtil.cropToBitmapData(_local3, 10.6 , 11.7, (_local3.width - 20), (_local3.height- 20), 0x800080);
        this.icon = new Bitmap(_local3);
        addChild(this.icon);

    }
    private function addBagBackground():void{
        var _local1:XML = ObjectLibrary.xmlLibrary_[this.objectType];
        var _local2:int = 32;
        var _local3:BitmapData;
        if (_local1.hasOwnProperty("ScaleValue")) {
            _local2 = _local1.ScaleValue
        }
        if (this.objectXML.BagType == 11) { //These are not color codes, these are object types. EquipCXML.as and look up "0x3119" for more information
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x3119, _local2, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("LG")) {
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x4339, _local2, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("ST")) {
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x4340, _local2, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("MLG")) {
            _local2        }
        else if (this.objectXML.hasOwnProperty("MY")) {
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x4339, _local2, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("Lunar")) {
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x4338, _local2, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("@setType")) {
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x4336, _local2, true, true, 6);
        }
        else if (this.objectXML.hasOwnProperty("Epic")) {
            _local3 = ObjectLibrary.getRedrawnTextureFromType(0x4338, _local2, true, true, 6);
        }
        var icon2:Bitmap;
        icon2 = new Bitmap(_local3);
        icon2.x = 204;
        icon2.y = (icon2.height / 2);
        addChild(icon2);
    }

    private function addTierText():void {
        var x = (this.objectXML.SlotType);
        var x2 = (this.objectXML.BagType);
        var _local1:Boolean = !this.isPet();
        var _local2:Boolean = !this.objectXML.hasOwnProperty("Consumable");
        var _local3:Boolean = !this.objectXML.hasOwnProperty("Treasure");
        var _local4:Boolean = this.objectXML.hasOwnProperty("Tier");
        if (((((_local1) && (_local2))) && (_local3))) {
            this.tierText = new TextFieldDisplayConcrete().setSize(16).setColor(0xFFFFFF).setTextWidth(30).setBold(true);
            if (_local4) {
                this.tierText.setStringBuilder(new LineBuilder().setParams(TextKey.TIER_ABBR, {"tier": this.objectXML.Tier}));
            }
            else {

                if (x2 == 11) {
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0x00ff00);
                    this.tierText.setStringBuilder(new StaticStringBuilder("  F "));
                }
                else if (this.objectXML.hasOwnProperty("@setType")) {
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xFF9900);
                    this.tierText.setStringBuilder(new StaticStringBuilder("ST"));
                }
                else if(this.objectXML.hasOwnProperty("LG")) {//0xCACACA 0xFEFCD7
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xfce303);
                    this.tierText.setStringBuilder(new StaticStringBuilder("LG"));
                }
                else if(this.objectXML.hasOwnProperty("ST")) {//0xCACACA 0xFEFCD7
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xE90000);
                    this.tierText.setStringBuilder(new StaticStringBuilder("MY"));
                }
                else if(this.objectXML.hasOwnProperty("MLG")) {//0xCACACA 0xFEFCD7
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xD5FBFC);
                    this.tierText.setStringBuilder(new StaticStringBuilder("LG"));
                }
                else if(this.objectXML.hasOwnProperty("Lunar")) {//0xCACACA
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xCACACA);
                    this.tierText.setStringBuilder(new StaticStringBuilder("LT"));
                }
                else if(this.objectXML.hasOwnProperty("MY")) {
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xfce303);
                    this.tierText.setStringBuilder(new StaticStringBuilder("LG"));
                }
                else if(this.objectXML.hasOwnProperty("BG")) {
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xDC143C);
                    this.tierText.setStringBuilder(new StaticStringBuilder("BG"));
                }
                else if (this.objectXML.hasOwnProperty("Epic"))
                {
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0x8A2BE2);
                    this.tierText.setStringBuilder(new StaticStringBuilder("UT"));
                }
                else if(x != 10) {
                    this.tierText.filters = getoutlinetext();
                    this.tierText.setColor(0xB200FF);
                    this.tierText.setStringBuilder(new LineBuilder().setParams(TextKey.UNTIERED_ABBR));
                }
            }
            addChild(this.tierText);
        }
    }

    private function isPet():Boolean {
        var activateTags:XMLList;
        activateTags = this.objectXML.Activate.(text() == "PermaPet");
        return ((activateTags.length() >= 1));
    }

    private function removeTitle():void {
        removeChild(this.titleText);
    }

    private function removeDesc():void {
        removeChild(this.descText);
    }

    private function addTitle():void {
        var _local1:int = ((((this.playerCanUse) || ((this.player == null)))) ? 0xFFFFFF : 16549442);
        this.titleText = new TextFieldDisplayConcrete().setSize(16).setColor(_local1).setBold(true).setTextWidth((((MAX_WIDTH - this.icon.width) - 4) - 30)).setWordWrap(true);
        if (this.titleOverride) {
            this.titleText.setStringBuilder(new StaticStringBuilder(this.titleOverride));
        }
        else {
            this.titleText.setStringBuilder(new LineBuilder().setParams(ObjectLibrary.typeToDisplayId_[this.objectType]));
        }
        this.titleText.filters = getoutlinetext();
        waiter.push(this.titleText.textChanged);
        addChild(this.titleText);
    }

    private function buildUniqueTooltipData():String { //Very Useful, Sadly not used.
        var _local1:XMLList;
        var _local2:Vector.<Effect>;
        var _local3:XML;
        if (this.objectXML.hasOwnProperty("ExtraTooltipData")) {
            _local1 = this.objectXML.ExtraTooltipData.EffectInfo;
            _local2 = new Vector.<Effect>();
            for each (_local3 in _local1) {
                _local2.push(new Effect(_local3.attribute("name"), _local3.attribute("description")));
            }
        }
        return ("");
    }

    private function makeSpecialList():void { //Also very useful
        var _local1:AppendingLineBuilder;
        if (((((!((this.effects.length == 0))) || (!((this.comparisonResults.lineBuilder == null))))) || (this.objectXML.hasOwnProperty("ExtraTooltipData")))) {
            this.line1 = new LineBreakDesign((MAX_WIDTH - 12), 0);
            this.effectsText = new TextFieldDisplayConcrete().setSize(12).setColor(0xB3B3B3).setTextWidth(MAX_WIDTH).setWordWrap(true).setHTML(true);
            _local1 = this.getEffectsStringBuilder();
            this.effectsText.setStringBuilder(_local1);
            this.effectsText.filters = getoutlinetext();
            if (_local1.hasLines()) {
                addChild(this.line1);
                addChild(this.effectsText);
            }
        }
    }

    private function makeEffectsList():void {
        var _local1:AppendingLineBuilder;
        if (((((!((this.effects.length == 0))) || (!((this.comparisonResults.lineBuilder == null))))) || (this.objectXML.hasOwnProperty("ExtraTooltipData")))) {
            this.line1 = new LineBreakDesign((MAX_WIDTH - 12), 0);
            this.effectsText = new TextFieldDisplayConcrete().setSize(14).setColor(0xB3B3B3).setTextWidth(MAX_WIDTH).setWordWrap(true).setHTML(true);
            _local1 = this.getEffectsStringBuilder();
            this.effectsText.setStringBuilder(_local1);
            this.effectsText.filters = getoutlinetext();
            if (_local1.hasLines()) {
                addChild(this.line1);
                addChild(this.effectsText);
            }
        }
    }

    private function getEffectsStringBuilder():AppendingLineBuilder {
        var _local1:AppendingLineBuilder = new AppendingLineBuilder();
        this.appendEffects(this.uniqueEffects, _local1);
        if (this.comparisonResults.lineBuilder.hasLines()) {
            _local1.pushParams(TextKey.BLANK, {"data": this.comparisonResults.lineBuilder});
        }
        this.appendEffects(this.effects, _local1);
        return (_local1);
    }

    private function appendEffects(_arg1:Vector.<Effect>, _arg2:AppendingLineBuilder):void {
        var _local3:Effect;
        var _local4:String;
        var _local5:String;
        for each (_local3 in _arg1) {
            _local4 = "";
            _local5 = "";
            if (_local3.color_) {
                _local4 = (('<font color="#' + _local3.color_.toString(16)) + '">');
                _local5 = "</font>";
            }
            _arg2.pushParams(_local3.name_, _local3.getValueReplacementsWithColor(), _local4, _local5);
        }
    }

    private function addNumProjectilesTagsToEffectsList():void {
        if (((this.objectXML.hasOwnProperty("NumProjectiles")) && !this.comparisonResults.processedTags.hasOwnProperty(this.objectXML.NumProjectiles.toXMLString()))) {
            this.effects.push(new Effect(TextKey.SHOTS, {"numShots": this.objectXML.NumProjectiles}));
        }
    }

    private function addFameBonusTagToEffectsList():void {
        var _local1:int;
        var _local2:uint;
        var _local3:int;

        if (this.objectXML.hasOwnProperty("FameBonus")) {
            _local1 = int(this.objectXML.FameBonus);
            _local2 = ((this.playerCanUse) ? TooltipHelper.BETTER_COLOR : TooltipHelper.NO_DIFF_COLOR);
            if (((!((this.curItemXML == null))) && (this.curItemXML.hasOwnProperty("FameBonus")))) {
                _local3 = int(this.curItemXML.FameBonus.text());
                _local2 = TooltipHelper.getTextColor((_local1 - _local3));
            }
            this.effects.push(new Effect(TextKey.FAME_BONUS, {"percent": (this.objectXML.FameBonus + "%")}).setReplacementsColor(_local2));
        }
    }
    private function addCooldownBonusTagToEffectsList():void {
        var _local1:Number;
        var _local2:uint;
        var _local3:Number;

        if (this.objectXML.hasOwnProperty("Cooldown")) {
            _local1 = this.objectXML.Cooldown;
            _local2 = ((this.playerCanUse) ? TooltipHelper.BETTER_COLOR : TooltipHelper.NO_DIFF_COLOR);
            if (((!((this.curItemXML == null))) && (this.curItemXML.hasOwnProperty("Cooldown")))) {
                _local3 = this.curItemXML.Cooldown;
                _local2 = TooltipHelper.getTextColor(_local3 - _local1);
            }
            this.effects.push(new Effect(TextKey.COOLDOWN, {"amount": (this.objectXML.Cooldown) + " seconds"}).setReplacementsColor(_local2));
        }
    }

    private function addMpCostTagToEffectsList():void {
        if (this.objectXML.hasOwnProperty("HpCost")) {
            if (!this.comparisonResults.processedTags[this.objectXML.HpCost[0].toXMLString()]) {
                this.effects.push(new Effect(TextKey.HP_COST, {"cost": this.objectXML.HpCost}));
            }
        }

        if (this.objectXML.hasOwnProperty("MpEndCost")) {
            if (!this.comparisonResults.processedTags[this.objectXML.MpEndCost[0].toXMLString()]) {
                this.effects.push(new Effect(TextKey.MP_COST, {"cost": this.objectXML.MpEndCost}));
            }
        }
        else {
            if (((this.objectXML.hasOwnProperty("MpCost")) && (!(this.comparisonResults.processedTags[this.objectXML.MpCost[0].toXMLString()])))) {
                if (!this.comparisonResults.processedTags[this.objectXML.MpCost[0].toXMLString()]) {
                    this.effects.push(new Effect(TextKey.MP_COST, {"cost": this.objectXML.MpCost}));
                }
            }
        }
    }

    private function addDoseTagsToEffectsList():void {
        if (this.objectXML.hasOwnProperty("Doses")) {
            this.effects.push(new Effect("Quantity", this.objectXML.Doses));
        }

        if (this.objectXML.hasOwnProperty("Quantity")) {
            this.effects.push(new Effect("Quantity: {quantity}", {"quantity": this.objectXML.Quantity}));
        }
    }

    private function addProjectileTagsToEffectsList():void {
        var _local1:XML;
        var _local2:int;
        var _local3:int;
        var _local4:Number;
        var _local5:XML;

        if (((this.objectXML.hasOwnProperty("Projectile")) && (!(this.comparisonResults.processedTags.hasOwnProperty(this.objectXML.Projectile.toXMLString()))))) {
            _local1 = XML(this.objectXML.Projectile);
            _local2 = int(_local1.MinDamage);
            _local3 = int(_local1.MaxDamage);
            this.effects.push(new Effect(TextKey.DAMAGE, {"damage": (((_local2 == _local3)) ? _local2 : ((_local2 + " - ") + _local3)).toString()}));
            _local4 = ((Number(_local1.Speed) * Number(_local1.LifetimeMS)) / 10000);
            this.effects.push(new Effect(TextKey.RANGE, {"range": TooltipHelper.getFormattedRangeString(_local4)}));
            if (this.objectXML.Projectile.hasOwnProperty("MultiHit")) {
                this.effects.push(new Effect(TextKey.MULTIHIT, {}).setColor(TooltipHelper.NO_DIFF_COLOR));
            }
            if (this.objectXML.Projectile.hasOwnProperty("PassesCover")) {
                this.effects.push(new Effect(TextKey.PASSES_COVER, {}).setColor(TooltipHelper.NO_DIFF_COLOR));
            }
            if (this.objectXML.Projectile.hasOwnProperty("ArmorPiercing")) {
                this.effects.push(new Effect(TextKey.ARMOR_PIERCING, {}).setColor(TooltipHelper.NO_DIFF_COLOR));
            }
            for each (_local5 in _local1.ConditionEffect) {
                if (this.comparisonResults.processedTags[_local5.toXMLString()] == null) {
                    this.effects.push(new Effect(TextKey.SHOT_EFFECT, {"effect": ""}));
                    this.effects.push(new Effect(TextKey.EFFECT_FOR_DURATION, {
                        "effect": this.objectXML.Projectile.ConditionEffect,
                        "duration": this.objectXML.Projectile.ConditionEffect.@duration
                    }).setColor(TooltipHelper.NO_DIFF_COLOR));
                }
            }
        }
    }

    private function addActivateTagsToEffectsList():void {
        var _local1:XML;
        var _local2:String;
        var _local3:int;
        var _local4:int;
        var _local5:String;
        var _local6:String;
        var _local7:Object;
        var _local8:String;
        var _local9:uint;
        var _local10:XML;
        var _local11:Object;
        var _local12:String;
        var _local13:uint;
        var _local14:XML;
        var _local15:String;
        var _local16:Object;
        var _local17:String;
        var _local18:Object;
        var _local19:Number;
        var _local20:Number;
        var _local21:Number;
        var _local22:Number;
        var _local23:Number;
        var _local24:Number;
        var _local25:Number;
        var _local26:Number;
        var _local27:Number;
        var _local28:Number;
        var _local29:Number;
        var _local30:Number;
        var _local31:AppendingLineBuilder;
        for each (_local1 in this.objectXML.Activate) {
            _local5 = this.comparisonResults.processedTags[_local1.toXMLString()];
            if (!this.comparisonResults.processedTags[_local1.toXMLString()]) {
                _local6 = _local1.toString();
                switch (_local6) {

                    case ActivationType.HEAL:
                        this.effects.push(new Effect(TextKey.INCREMENT_STAT, {
                            "statAmount": (("+" + _local1.@amount) + " "),
                            "statName": new LineBuilder().setParams(TextKey.STATUS_BAR_HEALTH_POINTS)
                        }));
                        break;
                    case ActivationType.HEAL_NOVA:
                        this.effects.push(new Effect("AoE Heal: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{amount} {WisHeal} HP within {range} {Wisrange} sqrs", {
                                "amount": _local1.@amount,
                                "range": _local1.@range,
                                "WisHeal": "<font color=\"#4063E3\">(+" + int(WisHealNova - _local1.@amount) + ")</font>",
                                "Wisrange": "<font color=\"#4063E3\">(+" + int(WisHealNovarange - _local1.@range)+ ")</font>"
                            }, TooltipHelper.getOpenTag(TooltipHelper.NO_DIFF_COLOR), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.MAGIC:
                        this.effects.push(new Effect(TextKey.INCREMENT_STAT, {
                            "statAmount": (("+" + _local1.@amount) + " "),
                            "statName": new LineBuilder().setParams(TextKey.STATUS_BAR_MANA_POINTS)
                        }));
                        break;
                    case ActivationType.MAGIC_NOVA:
                        this.effects.push(new Effect(TextKey.FILL_PARTY_MAGIC, (((_local1.@amount + " MP at ") + _local1.@range) + " sqrs")));
                        break;
                    case ActivationType.TELEPORT:
                        this.effects.push(new Effect(TextKey.BLANK, {"data": new LineBuilder().setParams(TextKey.TELEPORT_TO_TARGET)}));
                        break;
                    case ActivationType.VAMPIRE_BLAST:
                        var Wisdom = this.player.wisdom_;
                        var Vitality = this.player.vitality_ / 2;

                        var Damage = _local1.@totalDamage / 2;
                        var amountWis =  int(((Damage / 100) * Wisdom));
                        var amount =  int(((Damage / 100) * Vitality));


                        var range =  int((_local1.@radius / 150 * this.player.wisdom_)*100)/100;
                        this.effects.push(new Effect("Absorb: {effect}", {
                            "effect": new AppendingLineBuilder().pushParams("{amount} {Wisamount} {Wisamount2} HP within {range} {Wisrange} sqrs", {
                                "amount": Damage,
                                "Wisamount": "<font color=\"#4063E3\">(+" + amountWis + ")</font>",
                                "Wisamount2": "<font color=\"#Cb3638\">(+" + amount + ")</font>",
                                "range": _local1.@radius,
                                "Wisrange": "<font color=\"#4063E3\">(+" + range + ")</font>"
                            }, TooltipHelper.getOpenTag(TooltipHelper.NO_DIFF_COLOR), TooltipHelper.getCloseTag())
                        }));
                        break;

                    case ActivationType.TRAP: //Look at this insanity!
                        var dmg =  _local1.@totalDamage / 75;
                        var dmgdmg = int(dmg * this.player.wisdom_);
                        var targertts = int((_local1.@radius / 140)*100)/100;
                        var TargetsTargets = (targertts * this.player.wisdom_)*100/100;
                        var duration = _local1.@condDuration / 200;
                        var duration2 = int((duration * this.player.wisdom_)*100)/100;
                        var ActualDmg = int((dmgdmg)*100)/100;
                        _local7 = ((_local1.hasOwnProperty("@condEffect")) ? _local1.@condEffect : new LineBuilder().setParams(TextKey.CONDITION_EFFECT_SLOWED));
                        _local8 = ((_local1.hasOwnProperty("@condDuration")) ? _local1.@condDuration : "0");
                        this.effects.push(new Effect("Trap: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{amount} {WisAmount} Damage within {range} {Wisrange} sqrs", {
                                "amount": _local1.@totalDamage,
                                "WisAmount": "<font color=\"#4063E3\">(+" + ActualDmg + ")</font>",
                                "range": _local1.@radius,
                                "Wisrange": "<font color=\"#4063E3\">(+" + TargetsTargets + ")</font>"
                            }, TooltipHelper.getOpenTag(TooltipHelper.NO_DIFF_COLOR), TooltipHelper.getCloseTag())
                        }));
                        this.effects.push(new Effect("Trap Effect: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{effect} for {duration} {Wisduration} seconds \n", {
                            "effect": _local7,
                            "duration": _local8,
                            "Wisduration": "<font color=\"#4063E3\">(+" + duration2 + ")</font>"
                            }, TooltipHelper.getOpenTag(TooltipHelper.NO_DIFF_COLOR), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.STASIS_BLAST:
                        this.effects.push(new Effect(TextKey.STASIS_GROUP, {"stasis": new AppendingLineBuilder().pushParams(TextKey.SEC_COUNT, {"duration": _local1.@duration}, TooltipHelper.getOpenTag(TooltipHelper.NO_DIFF_COLOR), TooltipHelper.getCloseTag())}));
                        break;
                    case ActivationType.DECOY:
                        this.effects.push(new Effect("Decoy: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{duration} {WisDmg} seconds.", {
                                "duration":_local1.@duration,
                                "WisDmg": "<font color=\"#4063E3\">(+" + int(this.player.wisdom_ * _local1.@duration / 125) + ")</font>"
                            }, TooltipHelper.getOpenTag(0xFFFF8F), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.POISON_GRENADE:
                        this.effects.push(new Effect("Poison Grenade: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{damage} {WisDmg} damage over {duration} seconds within {radius} squares", {
                                "damage":_local1.@totalDamage,
                                "duration":_local1.@duration,
                                "radius": _local1.@radius,
                                "WisDmg": "<font color=\"#4063E3\">(+" + int(this.player.wisdom_ * _local1.@amount) + ")</font>"
                            }, TooltipHelper.getOpenTag(0xFFFF8F), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.BURNING_LIGHTNING:
                        this.effects.push(new Effect(TextKey.LIGHTNING, {
                            "data": new AppendingLineBuilder().pushParams(TextKey.DAMAGE_TO_TARGETS, {
                                "damage": _local1.@totalDamage,
                                "targets": _local1.@maxTargets
                            }, TooltipHelper.getOpenTag(TooltipHelper.NO_DIFF_COLOR), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.LIGHTNING:
                        var dmg =  _local1.@totalDamage / 50;
                        var dmgdmg = int(dmg * this.player.wisdom_);
                        var targertts = (_local1.@maxTargets / 75);
                        var TargetsTargets = int(((targertts * this.player.wisdom_) - _local1.@maxTargets)) + 1;
                        this.effects.push(new Effect("Lightning: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{damage} {WisDmg} to {targets} {WisTargets} targets", {
                                "damage":_local1.@totalDamage,
                                "targets":_local1.@maxTargets,
                                "WisDmg": "<font color=\"#4063E3\">(+" + dmgdmg + ")</font>",
                                "WisTargets": "<font color=\"#4063E3\">(+" + TargetsTargets + ")</font>"
                            }, TooltipHelper.getOpenTag(0xFFFF8F), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.COND_EFFECT_AURA:
                        this.effects.push(new Effect("Apply on Party: {data}", {
                            "data": new AppendingLineBuilder().pushParams("Within {range} sqrs {effect} for {duration} seconds", {
                                "effect": _local1.@effect,
                                "duration": _local1.@duration,
                                "range":_local1.@range
                            }, TooltipHelper.getOpenTag(0xFFFF8F), TooltipHelper.getCloseTag())
                        }));
                        break;

                    case ActivationType.COND_EFFECT_SELF:
                        this.effects.push(new Effect("Apply on Self: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{effect} for {duration} seconds", {
                                "effect": _local1.@effect,
                                "duration": _local1.@duration,
                                "radius": _local1.@radius
                            }, TooltipHelper.getOpenTag(0xFFFF8F), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.HEALING_GRENADE:
                        this.effects.push(new Effect("Healing Grenade: {data}", {
                            "data": new AppendingLineBuilder().pushParams("{damage} {WisDmg} health over {duration} seconds within {radius} squares", {
                                "damage":_local1.@totalDamage,
                                "duration":_local1.@duration,
                                "radius": _local1.@radius,
                                "WisDmg": "<font color=\"#4063E3\">(+" + int(this.player.wisdom_ * _local1.@amount) + ")</font>"
                            }, TooltipHelper.getOpenTag(0xFFFF8F), TooltipHelper.getCloseTag())
                        }));
                        break;
                    case ActivationType.REMOVE_NEG_COND:
                        this.effects.push(new Effect(TextKey.REMOVES_NEGATIVE, {}).setColor(TooltipHelper.NO_DIFF_COLOR));
                        break;
                    case ActivationType.REMOVE_NEG_COND_SELF:
                        this.effects.push(new Effect(TextKey.REMOVES_NEGATIVE, {}).setColor(TooltipHelper.NO_DIFF_COLOR));
                        break;
                    case ActivationType.GENERIC_ACTIVATE:
                        _local9 = 16777103;
                        if (this.curItemXML != null) {
                            _local10 = this.getEffectTag(this.curItemXML, _local1.@effect);
                            if (_local10 != null) {
                                _local19 = Number(_local1.@range);
                                _local20 = Number(_local10.@range);
                                _local21 = Number(_local1.@duration);
                                _local22 = Number(_local10.@duration);
                                _local23 = ((_local19 - _local20) + (_local21 - _local22));
                                if (_local23 > 0) {
                                    _local9 = 0xFF00;
                                }
                                else {
                                    if (_local23 < 0) {
                                        _local9 = 0xFF0000;
                                    }
                                }
                            }
                        }
                        _local11 = {
                            "range": _local1.@range,
                            "effect": _local1.@effect,
                            "duration": _local1.@duration
                        };
                        _local12 = "Within {range} sqrs {effect} for {duration} seconds";
                        if (_local1.@target != "enemy") {
                            this.effects.push(new Effect(TextKey.PARTY_EFFECT, {"effect": LineBuilder.returnStringReplace(_local12, _local11)}).setReplacementsColor(_local9));
                        }
                        else {
                            this.effects.push(new Effect(TextKey.ENEMY_EFFECT, {"effect": LineBuilder.returnStringReplace(_local12, _local11)}).setReplacementsColor(_local9));
                        }
                        break;
                    case ActivationType.STAT_BOOST_AURA:
                        _local13 = 16777103;
                        if (this.curItemXML != null) {
                            _local14 = this.getStatTag(this.curItemXML, _local1.@stat);
                            if (_local14 != null) {
                                _local24 = Number(_local1.@range);
                                _local25 = Number(_local14.@range);
                                _local26 = Number(_local1.@duration);
                                _local27 = Number(_local14.@duration);
                                _local28 = Number(_local1.@amount);
                                _local29 = Number(_local14.@amount);
                                _local30 = (((_local24 - _local25) + (_local26 - _local27)) + (_local28 - _local29));
                                if (_local30 > 0) {
                                    _local13 = 0xFF00;
                                }
                                else {
                                    if (_local30 < 0) {
                                        _local13 = 0xFF0000;
                                    }
                                }
                            }
                        }
                        _local3 = int(_local1.@stat);
                        _local15 = LineBuilder.getLocalizedString2(StatData.statToName(_local3));
                        if (_local3 == 112) {
                            _local15 = "Critical Dmg";
                        }
                        if (_local3 == 114) {
                            _local15 = "Critical Hit";
                        }
                        _local16 = {
                            "range": _local1.@range,
                            "stat": _local15,
                            "amount": _local1.@amount,
                            "duration": _local1.@duration
                        };
                        _local17 = "Within {range} sqrs increase {stat} by {amount} for {duration} seconds";


                        this.effects.push(new Effect(TextKey.PARTY_EFFECT, {"effect": LineBuilder.returnStringReplace(_local17, _local16)}).setReplacementsColor(_local13));
                        break;
                    case ActivationType.STAT_BOOST_SELF:
                        _local13 = 16777103;
                        if (this.curItemXML != null) {
                            _local14 = this.getStatTag(this.curItemXML, _local1.@stat);
                            if (_local14 != null) {
                                _local24 = Number(_local1.@range);
                                _local25 = Number(_local14.@range);
                                _local26 = Number(_local1.@duration);
                                _local27 = Number(_local14.@duration);
                                _local28 = Number(_local1.@amount);
                                _local29 = Number(_local14.@amount);
                                _local30 = (((_local24 - _local25) + (_local26 - _local27)) + (_local28 - _local29));
                                if (_local30 > 0) {
                                    _local13 = 0xFF00;
                                }
                                else {
                                    if (_local30 < 0) {
                                        _local13 = 0xFF0000;
                                    }
                                }
                            }
                        }
                        _local3 = int(_local1.@stat);
                        _local15 = LineBuilder.getLocalizedString2(StatData.statToName(_local3));
                        if (_local3 == 112) {
                            _local15 = "Critical Dmg";
                        }
                        if (_local3 == 114) {
                            _local15 = "Critical Hit";
                        }
                        _local16 = {
                            "stat": _local15,
                            "amount": _local1.@amount,
                            "duration": _local1.@duration
                        };
                        _local17 = "Increase {stat} by {amount} for {duration} seconds";
                        this.effects.push(new Effect(TextKey.EFFECT_ON_SELF, {"effect": LineBuilder.returnStringReplace(_local17, _local16)}).setReplacementsColor(_local13));
                        break;
                    case ActivationType.INCREMENT_STAT:
                        _local3 = int(_local1.@stat);
                        _local4 = int(_local1.@amount);
                        _local18 = {};
                        if (((!((_local3 == StatData.HP_STAT))) && (!((_local3 == StatData.MP_STAT))))) {
                            _local2 = TextKey.PERMANENTLY_INCREASES;
                            _local18["statName"] = new LineBuilder().setParams(StatData.statToName(_local3));
                            this.effects.push(new Effect(_local2, _local18).setColor(16777103));
                            break;
                        }
                        _local2 = TextKey.BLANK;
                        _local31 = new AppendingLineBuilder().setDelimiter(" ");
                        var perc:String = (_local1.@isPerc == "true") ? "% " : " ";
                        _local31.pushParams(TextKey.BLANK, {"data": new StaticStringBuilder((prefix(_local4) + perc))});
                        _local31.pushParams(StatData.statToName(_local3));
                        _local18["data"] = _local31;
                        this.effects.push(new Effect(_local2, _local18));
                        break;
                }
            }
        }
    }
    private function handleWisMod():void {
        var _local3:XML;
        var _local4:XML;
        var _local5:String;
        var _local6:String;
        if (this.player == null) {
            return;
        }
        var _local1:Number = (this.player.wisdom_ + this.player.wisdomBoost_);
        if (_local1 < 30) {
            return;
        }
        var _local2:Vector.<XML> = new Vector.<XML>();
        if (this.curItemXML != null) {
            this.curItemXML = this.curItemXML.copy();
            _local2.push(this.curItemXML);
        }
        if (this.objectXML != null) {
            this.objectXML = this.objectXML.copy();
            _local2.push(this.objectXML);
        }
        for each (_local4 in _local2) {
            for each (_local3 in _local4.Activate) {
                _local5 = _local3.toString();
                if (_local3.@effect != "Stasis") {
                    _local6 = _local3.@useWisMod;
                    if (!(((((((_local6 == "")) || ((_local6 == "false")))) || ((_local6 == "0")))) || ((_local3.@effect == "Stasis")))) {
                        switch (_local5) {
                            case ActivationType.HEAL_NOVA:
                                WisHealNova = this.modifyWisModStat(_local3.@amount, 0);
                                WisHealNovarange = this.modifyWisModStat(_local3.@range);
                                break;
                            case ActivationType.COND_EFFECT_AURA:
                                _local3.@duration = this.modifyWisModStat(_local3.@duration);
                                _local3.@range = this.modifyWisModStat(_local3.@range);
                                break;
                            case ActivationType.COND_EFFECT_SELF:
                                _local3.@duration = this.modifyWisModStat(_local3.@duration);
                                break;
                            case ActivationType.STAT_BOOST_AURA:
                                _local3.@amount = this.modifyWisModStat(_local3.@amount, 0);
                                _local3.@duration = this.modifyWisModStat(_local3.@duration);
                                _local3.@range = this.modifyWisModStat(_local3.@range);
                                break;
                            case ActivationType.GENERIC_ACTIVATE:
                                _local3.@duration = this.modifyWisModStat(_local3.@duration);
                                _local3.@range = this.modifyWisModStat(_local3.@range);
                                break;
                        }
                    }
                }
            }
        }
    }

    private function getEffectTag(xml:XML, effectValue:String):XML {
        var matches:XMLList;
        var tag:XML;
        matches = xml.Activate.(text() == ActivationType.GENERIC_ACTIVATE);
        for each (tag in matches) {
            if (tag.@effect == effectValue) {
                return (tag);
            }
        }
        return null;
    }

    private function getStatTag(xml:XML, statValue:String):XML {
        var matches:XMLList;
        var tag:XML;
        matches = xml.Activate.(text() == ActivationType.STAT_BOOST_AURA);
        for each (tag in matches) {
            if (tag.@stat == statValue) {
                return (tag);
            }
        }
        return null;
    }

    private function addActivateOnEquipTagsToEffectsList():void {
        var _local1:XML;
        var _local2:Boolean = true;
        for each (_local1 in this.objectXML.ActivateOnEquip) {
            if (_local2) {
                this.effects.push(new Effect(TextKey.ON_EQUIP, ""));
                _local2 = false;
            }
            if (_local1.toString() == "IncrementStat") {
                this.effects.push(new Effect(TextKey.INCREMENT_STAT, getComparedStatText(_local1, _local1.@isPerc == "true")).setReplacementsColor(this.getComparedStatColor(_local1)));
            }
        }
        for each (_local1 in this.objectXML.Steal) {
            var steallife:Boolean = _local1.@type == "life";
            var stealmana:Boolean = _local1.@type == "mana";
            if (_local2) {
                this.effects.push(new Effect(TextKey.ON_EQUIP, ""));
                _local2 = false;
            }
            if (steallife && stealmana)
                this.effects.push(new Effect("+" + _local1.@amount + " Devour", {}).setColor(9055202));
            else if (steallife)
                this.effects.push(new Effect("+" + _local1.@amount + " Life Steal", {}).setColor(0xd80d38));
            else if (stealmana)
                this.effects.push(new Effect("+" + _local1.@amount + " Mana Leech", {}).setColor(0x6666FF));

        }
        for each (_local1 in this.objectXML.EffectEquip) {
            if (_local2) {
                this.effects.push(new Effect(TextKey.ON_EQUIP, ""));
                _local2 = false;
            }
            var delay:String = (this.objectXML.EffectEquip.@delay % (10 | 15) == 0 && this.objectXML.EffectEquip.@delay > 60) ?
                    this.objectXML.EffectEquip.@delay / 60 + " minutes" :
                    this.objectXML.EffectEquip.@delay + " seconds";
            this.effects.push(new Effect("Grants '" + this.objectXML.EffectEquip.@effect
                    + (this.objectXML.EffectEquip.@delay == 0 ? "'"
                            : "' after " + delay), "")
                    .setColor(9055202));
        }
    }

    private static function getComparedStatText(xml:XML, isPerc:Boolean) : Object {
        var stat:int = int(xml.@stat);
        var amount:int = int(xml.@amount);
        return ({
            "statAmount": prefix(amount) + (isPerc ? "% " : " "),
            "statName": new LineBuilder().setParams(StatData.statToName(stat))
        });
    }
    private static function prefix(input:int) : String {
        var formattedStr:String = (input > -1 ? "+" : "");
        return formattedStr + input;
    }

    private function getComparedStatColor(activateXML:XML):uint {
        var match:XML;
        var otherAmount:int;
        var stat:int = int(activateXML.@stat);
        var amount:int = int(activateXML.@amount);
        var textColor:uint = ((this.playerCanUse) ? TooltipHelper.BETTER_COLOR : TooltipHelper.NO_DIFF_COLOR);
        var otherMatches:XMLList;
        if (this.curItemXML != null) {
            otherMatches = this.curItemXML.ActivateOnEquip.(@stat == stat);
        }
        if (((!((otherMatches == null))) && ((otherMatches.length() == 1)))) {
            match = XML(otherMatches[0]);
            otherAmount = int(match.@amount);
            textColor = TooltipHelper.getTextColor((amount - otherAmount));

            if (activateXML.@isPerc == "true" && match.@isPerc == "true") {
                textColor = TooltipHelper.getTextColor((amount - otherAmount));
            }
            else if (activateXML.@isPerc == "true") {
                textColor = TooltipHelper.NO_DIFF_COLOR;
            }
        }

        if (amount < 0) {
            textColor = 0xFF0000;
        }

        return (textColor);
    }

    private function addAbilityItemRestrictions():void {
        this.restrictions.push(new Restriction(TextKey.KEYCODE_TO_USE, 0xFFFFFF, false));
    }

    private function addConsumableItemRestrictions():void {
        this.restrictions.push(new Restriction(TextKey.CONSUMED_WITH_USE, 0xB3B3B3, false));
        if (((this.isInventoryFull) || ((this.inventoryOwnerType == InventoryOwnerTypes.CURRENT_PLAYER)))) {
            this.restrictions.push(new Restriction(TextKey.DOUBLE_CLICK_OR_SHIFT_CLICK_TO_USE, 0xFFFFFF, false));
        }
        else {
            this.restrictions.push(new Restriction(TextKey.DOUBLE_CLICK_TAKE_SHIFT_CLICK_USE, 0xFFFFFF, false));
        }
    }

    private function addReusableItemRestrictions():void {
        this.restrictions.push(new Restriction(TextKey.CAN_BE_USED_MULTIPLE_TIMES, 0xB3B3B3, false));
        this.restrictions.push(new Restriction(TextKey.DOUBLE_CLICK_OR_SHIFT_CLICK_TO_USE, 0xFFFFFF, false));
    }

    private function makeRestrictionList():void { //very old code, could easily be rewrote for optimization
        var _local2:XML;
        var _local3:Boolean;
        var _local4:int;
        var _local5:int;
        var _local42:Boolean = this.objectXML.hasOwnProperty("Tier");
        var x = (this.objectXML.SlotType);
        var x2 = (this.objectXML.BagType);
        this.restrictions = new Vector.<Restriction>();

        if (((((this.objectXML.hasOwnProperty("VaultItem")) && (!((this.invType == -1))))) && (!((this.invType == ObjectLibrary.idToType_["Vault Chest"]))))) {
            this.restrictions.push(new Restriction(TextKey.STORE_IN_VAULT, 16549442, true));
        }
        if (this.objectXML.hasOwnProperty("Soulbound")) {
            this.restrictions.push(new Restriction(TextKey.ITEM_SOULBOUND, 0xB3B3B3, false));
        }
        if(!Parameters.data_.itemColorText) {

            if (x2 == 11) {
                this.restrictions.push(new Restriction(("Crafting Material"), 0x00ff00, false));
            }
            else if (this.objectXML.hasOwnProperty("BG")) {
                this.restrictions.push(new Restriction(("Earth: Miserable"), 0xDC143C, false));
            }
            else if (this.objectXML.hasOwnProperty("ST")) {
                this.restrictions.push(new Restriction(("Earth: Mythical"), 0xE90000, false));
            }
            else if (this.objectXML.hasOwnProperty("LG"))
            {
                this.restrictions.push(new Restriction(("Earth: Legendary"), 0xfce303, false));
            }
            else if (this.objectXML.hasOwnProperty("MY"))
            {
                this.restrictions.push(new Restriction(("Earth: Legendary"), 0xFFD700, false));
            }
            else if (this.objectXML.hasOwnProperty("EUncommon"))
            {
                this.restrictions.push(new Restriction(("Earth: Uncommon"), 0x00FF00, false));
            }
            else if (this.objectXML.hasOwnProperty("Lcommon"))
            {
                this.restrictions.push(new Restriction(("Moon: Normal"), 0xffffff, false));
            }
            else if (this.objectXML.hasOwnProperty("LUncommon"))
            {
                this.restrictions.push(new Restriction(("Moon: Uncommon"), 0xCACACA, false));
            }
            else if (this.objectXML.hasOwnProperty("Lunar"))
            {
                this.restrictions.push(new Restriction(("Moon: Rare"), 0xCACACA, false));
            }
            else if (this.objectXML.hasOwnProperty("MLG")) {
                this.restrictions.push(new Restriction(("Moon: Legendary"), 0xD5FBFC, false));
            }
            else if (this.objectXML.hasOwnProperty("Epic")) {
                this.restrictions.push(new Restriction(("Earth: Epic"), 0x8A2BE2, false));
            }
           else if (_local42) {
                this.restrictions.push(new Restriction(("Speciality: Normal"), 0xffffff, false));
            }
            else if (x != 10) {
                this.restrictions.push(new Restriction(("Earth: Rare"), 0xB200FF, false));
            }
            else if (_local22) {
                this.restrictions.push(new Restriction(("Speciality: Normal"), 0xffffff, false));
            }
            else {
                this.restrictions.push(new Restriction(("Speciality: Normal"), 0xffffff, false));
            }





            if (this.objectXML.hasOwnProperty("@setType")) {
                this.restrictions.push(new Restriction(("This artifact is a part of " + this.objectXML.attribute("setName")), 0xFF9900, false));
                this.tierText.setColor(0xFF9900);
            }
            if (this.objectXML.hasOwnProperty("ST")) {//0xCACACA

                this.tierText.setColor(0xE90000);
            }
            if (this.objectXML.hasOwnProperty("LG")) {
                this.tierText.setColor(0xfce303);
            }
            if (this.objectXML.hasOwnProperty("BG")) {
                this.tierText.setColor(0xDC143C);
            }
            if (this.objectXML.hasOwnProperty("MLG")) {//0xCACACA
                this.tierText.setColor(0xD5FBFC);
            }
            if (this.objectXML.hasOwnProperty("Lunar")) {//0xCACACA
                this.tierText.setColor(0xCACACA);
            }
            if (this.objectXML.hasOwnProperty("MY")) {
                this.tierText.setColor(0xFFD700);
            }
            if (this.objectXML.hasOwnProperty("Reskin")) {
                this.restrictions.push(new Restriction(("Reskin: This item has a Reskin."), 0xe6cc80, false));
            }
            if (this.objectXML.hasOwnProperty("Upgradeable")) {
                this.restrictions.push(new Restriction(("Upgradeable: This item can be crafted into a better version using the forge."), 0xff8000, false));
            }


        }
        else {

            if (x2 == 11) {
                this.restrictions.push(new Restriction(("Crafting Material"), 0x00ff00, false));
            }
            else if (this.objectXML.hasOwnProperty("BG")) {
                this.restrictions.push(new Restriction(("Earth: Miserable"), 0xDC143C, false));
            }
            else if (this.objectXML.hasOwnProperty("ST")) {
                this.restrictions.push(new Restriction(("Earth: Mythical"), 0xE90000, false));
            }
            else if (this.objectXML.hasOwnProperty("LG"))
            {
                this.restrictions.push(new Restriction(("Earth: Legendary"), 0xfce303, false));
            }
            else if (this.objectXML.hasOwnProperty("MY"))
            {
                this.restrictions.push(new Restriction(("Earth: Legendary"), 0xFFD700, false));
            }
            else if (this.objectXML.hasOwnProperty("EUncommon"))
            {
                this.restrictions.push(new Restriction(("Earth: Uncommon"), 0x00FF00, false));
            }
            else if (this.objectXML.hasOwnProperty("Lcommon"))
            {
                this.restrictions.push(new Restriction(("Moon: Normal"), 0xffffff, false));
            }
            else if (this.objectXML.hasOwnProperty("LUncommon"))
            {
                this.restrictions.push(new Restriction(("Moon: Uncommon"), 0xCACACA, false));
            }
            else if (this.objectXML.hasOwnProperty("Lunar"))
            {
                this.restrictions.push(new Restriction(("Moon: Rare"), 0xCACACA, false));
            }
            else if (this.objectXML.hasOwnProperty("MLG"))
            {
                this.restrictions.push(new Restriction(("Moon: Legendary"), 0xD5FBFC, false));
            }
            else if (this.objectXML.hasOwnProperty("Epic")) {
                this.restrictions.push(new Restriction(("Earth: Epic"), 0x8A2BE2, false));
            }
            else if (_local42) {
                this.restrictions.push(new Restriction(("Speciality: Normal"), 0xffffff, false));
            }
            else if (x != 10) {
                this.restrictions.push(new Restriction(("Earth: Rare"), 0xB200FF, false));
            }
            else if (_local22) {
                this.restrictions.push(new Restriction(("Speciality: Normal"), 0xffffff, false));
            }
            else {
                this.restrictions.push(new Restriction(("Speciality: Normal"), 0xffffff, false));
            }
            if (this.objectXML.hasOwnProperty("Upgradeable")) {
                this.restrictions.push(new Restriction(("Upgradeable: This item can be crafted into a better version using the forge."), 0xff8000, false));
            }


            if (this.objectXML.hasOwnProperty("@setType")) {
                this.restrictions.push(new Restriction(("This artifact is a part of " + this.objectXML.attribute("setName")), 0xFF9900, false));
                this.descText.setColor(0xFF9900);
                this.titleText.setColor(0xFF9900);
                this.tierText.setColor(0xFF9900);
            }
            if (this.objectXML.hasOwnProperty("ST")) {//0xCACACA
                this.descText.setColor(0xE90000);
                this.titleText.setColor(0xE90000);
                this.tierText.setColor(0xE90000);
            }
            if (this.objectXML.hasOwnProperty("LG")) {
                this.descText.setColor(0xfce303);
                this.titleText.setColor(0xfce303);
                this.tierText.setColor(0xfce303);
            }
            if (this.objectXML.hasOwnProperty("BG")) {
                this.descText.setColor(0xDC143C);
                this.titleText.setColor(0xDC143C);
                this.tierText.setColor(0xDC143C);
            }
            if (this.objectXML.hasOwnProperty("MLG")) {//0xCACACA
                this.descText.setColor(0xD5FBFC);
                this.titleText.setColor(0xD5FBFC);
                this.tierText.setColor(0xD5FBFC);
            }
            if (this.objectXML.hasOwnProperty("Lunar")) {//0xCACACA
                this.descText.setColor(0xCACACA);
                this.titleText.setColor(0xCACACA);
                this.tierText.setColor(0xCACACA);
            }
            if (this.objectXML.hasOwnProperty("MY")) {
                this.descText.setColor(0xFFD700);
                this.titleText.setColor(0xFFD700);
                this.tierText.setColor(0xFFD700);
            }
        }

        if (this.playerCanUse) {
            if (this.objectXML.hasOwnProperty("Usable")) {
                this.addAbilityItemRestrictions();
            }
            else {
                if (this.objectXML.hasOwnProperty("Consumable")) {
                    this.addConsumableItemRestrictions();
                }
                else {
                    if (this.objectXML.hasOwnProperty("InvUse")) {
                        this.addReusableItemRestrictions();
                    }
                }
            }
        }

        else {
            if (this.player != null) {
                this.restrictions.push(new Restriction(TextKey.NOT_USABLE_BY, 16549442, true));
            }
        }
        var _local1:Vector.<String> = ObjectLibrary.usableBy(this.objectType);
        if (_local1 != null) {
            this.restrictions.push(new Restriction(TextKey.USABLE_BY, 0xB3B3B3, false));
        }
        for each (_local2 in this.objectXML.EquipRequirement) {
            _local3 = ObjectLibrary.playerMeetsRequirement(_local2, this.player);
            if (_local2.toString() == "Stat") {
                _local4 = int(_local2.@stat);
                _local5 = int(_local2.@value);
                this.restrictions.push(new Restriction(((("Requires " + StatData.statToName(_local4)) + " of ") + _local5), ((_local3) ? 0xB3B3B3 : 16549442), ((_local3) ? false : true)));
            }
        }

        var _local22:Boolean = this.objectXML.hasOwnProperty("Consumable");
        var _local32:Boolean = !this.objectXML.hasOwnProperty("Treasure");

        //corrupt

        if (this.objectXML.hasOwnProperty("CorruptAB")) {
            this.restrictions.push(new Restriction(("Corrupt: "), 0xffff4d, true));
            this.restrictions.push(new Restriction(("100% chance to be Diminished(3s) when hit."), 0xffff4d, false))
        }

        //Rings

        if (this.objectXML.hasOwnProperty("Enlightened")) {
            this.restrictions.push(new Restriction(("Enlightened: "), 0xffff4d, true));
            this.restrictions.push(new Restriction(("Heal 250 HP and lose 150 mana when geting hit under 30% HP (20 second cooldown)"), 0xffff4d, false))
        }

        if (this.objectXML.hasOwnProperty("Divine")) {
            this.restrictions.push(new Restriction(("Divine Assistance: "), 0xffff4d, true));
            this.restrictions.push(new Restriction(("Receive a random buff(3s) every 5 seconds."), 0xffff4d, false))
        }

        //Armors

        if (this.objectXML.hasOwnProperty("HasteAB")) {
            this.restrictions.push(new Restriction(("Haste: "), 0xDCDCDC, true));
            this.restrictions.push(new Restriction(("Gain Haste(3s) when hit by a projectile."), 0xDCDCDC, false));
        }

        if (this.objectXML.hasOwnProperty("Reflect")) {
            this.restrictions.push(new Restriction(("Reflect: "), 0xDCDCDC, true));
            this.restrictions.push(new Restriction(("25% chance to reflect any debuffs received back to the dealer."), 0xDCDCDC, false))
        }

        if (this.objectXML.hasOwnProperty("Dodge")) {
            this.restrictions.push(new Restriction(("Ultra Instinct: "), 0xDCDCDC, true));
            this.restrictions.push(new Restriction(("5% chance to dodge the attack and take 0 damage for the next 5 seconds. (turn music on)."), 0xDCDCDC, false))
        }

        //Weapons

        if (this.objectXML.hasOwnProperty("AwokenAB")) {
            this.restrictions.push(new Restriction(("Awoken: "), 0xFFFFFF, true));
            this.restrictions.push(new Restriction(("25% chance to gain Awoken(3s) when hit."), 0xFFFFFF, false));
        }

        if (this.objectXML.hasOwnProperty("Counter") || this.objectXML.hasOwnProperty("Curse") || this.objectXML.hasOwnProperty("CurseEffect")) {//<CurseEffect/>
            this.restrictions.push(new Restriction(("Curse: "), 0xFF1900, true));
            this.restrictions.push(new Restriction(("2% chance of cursing(2s) the enemy for a small amount of time."), 0xFF1900, false))
        }
        if (this.objectXML.hasOwnProperty("TouchEffect") || this.objectXML.hasOwnProperty("Touch")) {
            this.restrictions.push(new Restriction(("Angel's Touch: "), 0xFfba00, true));
            this.restrictions.push(new Restriction(("2% chance of healing yourself for 7% of your maximum health."), 0xFfba00, false))
        }

        if (this.objectXML.hasOwnProperty("Paralyze") || this.objectXML.hasOwnProperty("ParalyzeEffect")) {
            this.restrictions.push(new Restriction(("Paralyze: "), 0xeab676, true));
            this.restrictions.push(new Restriction(("2% chance of paralyzing(2.5s) the enemy for a small amount of time."), 0xeab676, false))
        }

        if (this.objectXML.hasOwnProperty("Critical")) {
            this.restrictions.push(new Restriction(("Legendary Power: "), 0x76b5c5, true));
            this.restrictions.push(new Restriction(("1% chance your damage will be quintuple."), 0x76b5c5, false))
        }

        //Abilities

        if (this.objectXML.hasOwnProperty("OffensiveTeam")) {
            this.restrictions.push(new Restriction(("Offensive: "), 0x5100ff, true));
            this.restrictions.push(new Restriction(("5% chance to give damaging(4s) and armored(3s) to all allies in 5 sqrs."), 0x5100ff, false))
        }

        if (this.objectXML.hasOwnProperty("Shockwave")) {
            this.restrictions.push(new Restriction(("Shockwave: "), 0x5100ff, true));
            this.restrictions.push(new Restriction(("5% chance to paralyze(5s) all enemies in 10 sqrs."), 0x5100ff, false))
        }

        if (this.objectXML.hasOwnProperty("SupportiveTeam")) {
            this.restrictions.push(new Restriction(("Indestructible: "), 0x5100ff, true));
            this.restrictions.push(new Restriction(("5% chance to heal yourself for 250 HP and give Invulnerable(4s) to players in 5 sqrs."), 0x5100ff, false))
        }


        //Mythical Effects
        var MythicalColor = 0xFFD700;

        if (this.objectXML.hasOwnProperty("ClawMythical")) {
            this.restrictions.push(new Restriction(("Super Saiyan 2: "), 0xFF1900, true));
            this.restrictions.push(new Restriction(("When you equip 'Cloth of Assassination' and you are invisible, you increase your Critical Damage by 75."), 0xFF1900, false))
        }

        if (this.objectXML.hasOwnProperty("EquippedLGAbility8")) {
            this.restrictions.push(new Restriction(("ROTF: "), 0xFFFFFF, true));
            this.restrictions.push(new Restriction(("A special item effect related to rotf. 10% chance to poison the enemy for 1000 damage over 10 seconds."), 0xFFFFFF, false))
        }

        if (this.objectXML.hasOwnProperty("Disarray")) {
            this.restrictions.push(new Restriction(("Disarray: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Cast a spell that explode enemies from the insides using the power of the void."), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("Exorcism")) {
            this.restrictions.push(new Restriction(("Exorcism: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("10% chance to blast all enemies in a 6 tile range for 100 * Wisdom."), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("EquippedLGAbility4")) {
            this.restrictions.push(new Restriction(("Leaf's Assistance: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Receive 100 + (2 * Wisdom) MP if MP falls below 150. (20 second cooldown) [Must be on for at least 10 seconds]."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("EquippedLGAbility3")) {
            this.restrictions.push(new Restriction(("Hydro's Assistance: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Upon being hit, receive 10 + Vit/5 HP and 10 + Wis/5 HP Mana."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("EquippedLGAbility2")) {
            this.restrictions.push(new Restriction(("Lava's Assistance: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Upon being hit below 33% HP, gain Invulnerable(2s) Damaging(2.5s) and Haste(2.5s) (30 second cooldown) [Must be on for at least 10 seconds]."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("CLAW")) {
            this.restrictions.push(new Restriction(("Super Saiyan 2: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip 'Cloth of Assassination' and you are invisible, you increase your Critical Damage by 75."), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("LGPLATEYESAB")) {
            this.restrictions.push(new Restriction(("Plated Guardian: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Defense is increased by 20% while below half HP. (50% HP)"), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("MYRing")) {
            this.restrictions.push(new Restriction(("Contribute: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("For each Critical Hit after 80, gain 10 extra Critical Damage."), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("MYKatana")) {
            this.restrictions.push(new Restriction(("Eternal Rage: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While under 80% HP, heal for 100 HP. [5 second cooldown] [Must be on for at least 5 seconds]"), MythicalColor, false))
        }
        //valor Using your ability also cost health equal to twice your mana cost.
        if (this.objectXML.hasOwnProperty("TCurse")) {
            this.restrictions.push(new Restriction(("Titan's Curse: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Using your ability also cost health equal to twice your mana cost."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("Meteor")) {
            this.restrictions.push(new Restriction(("Meteor: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip 'Scorching Scepter', using your ability has a chance to throw a meteor that deals damage based on both your max health and mana. The impact from this meteor also dazes enemies for 2 seconds."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("Scorch")) {
            this.restrictions.push(new Restriction(("Scorch: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("The extreme heat of this scepter's energy causes damage to pierce directly through defense."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("Resilience")) {
            this.restrictions.push(new Restriction(("Lifeline: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Having less than 1/3rd of your Maximum Health, gain a massive Health Regeneration boost."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("Combustion")) {
            this.restrictions.push(new Restriction(("Combustion: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Using your ability causes an explosion that burns enemies around you. This explosion does 3000 damage over 7 seconds."), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("GResistance")) {
            this.restrictions.push(new Restriction(("Stoneheart: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While being above 50% Maximum Health, you are immune to Armor Broken."), MythicalColor, false))
        }


        if (this.objectXML.hasOwnProperty("ColossusBreastplate")) {
            this.restrictions.push(new Restriction(("Void's Glory: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Apply Strength(4.5s) when hit. [2 second cooldown]"), MythicalColor, false))
        }
        if (this.objectXML.hasOwnProperty("MYHide")) {
            this.restrictions.push(new Restriction(("Silence: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Apply Invisible(5s) when taken below 40% HP. [15 second cooldown]"), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("MYShuriken")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Everlasting Inferno' for a special effect."), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("LGCLOTHYESAB")) {
            this.restrictions.push(new Restriction(("Assassin's Pride: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Critical Hit is increased by 100% while below 500 HP."), MythicalColor, false));
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Claws Of No Remorse' for a special effect."), MythicalColor, false))
        }

        if (this.objectXML.hasOwnProperty("ROBEYESLG")) {
            this.restrictions.push(new Restriction(("Brother's Spirit: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Attack is increased by 20% while HP is above 500."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("ObiArmorAB")) {
            this.restrictions.push(new Restriction(("Absolute Unit: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("For each 5 HP you have above 1000, you gain 1 defense."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("CybShieldAB2")) {
            this.restrictions.push(new Restriction(("Dual Shield: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("If 'Shield of King Cyberious' is equipped, your HP is increased by 60 and you gain Armored."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("CybShieldABB")) {
            this.restrictions.push(new Restriction(("Plated Balance: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("If you have Armored, Solid or Barrier you gain 10 CHT."), MythicalColor, false));
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Weaponized Shield of Command' for a special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("CybRingAB")) {
            this.restrictions.push(new Restriction(("Demonic Horns: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Attack gained from items will also be given to Critical Hit."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("WildShadow")) {
            this.restrictions.push(new Restriction(("Swift Shadows: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain 10 Speed and 25 Critical Hit when MP is above 200."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("QuiverNia")) {
            this.restrictions.push(new Restriction(("Ancient Technique: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain 15 Attack for each 100 mana you currently have."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("CrystalArmor2")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be forged with something to reveal its special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("CrystalArmor")) {
            this.restrictions.push(new Restriction(("Crystal Thorns: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain 8 Dexterity and 8 Attack when hit for more than 25 damage for 5 seconds. (Stacks)"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("EquippedLGAbility5")) {
            this.restrictions.push(new Restriction(("Front Line: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Your healed for double your Vitality when hit below half HP. (50% HP) (20 second cooldown) [Must be on for at least 10 seconds]"), MythicalColor, false));
            this.restrictions.push(new Restriction(("Gladiator: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While HP is above 600, you gain Armored. While HP is below 300, you gain Healing."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("cloakabc")) {
            this.restrictions.push(new Restriction(("Assassin: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain 12 dexterity while invisible."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Deeppads")) {
            this.restrictions.push(new Restriction(("Powered Metal: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be forged with something to reveal its special effect"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("heavenarmor")) {
            this.restrictions.push(new Restriction(("Purity: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain 30 Vitality when hit below half health. (50% HP)"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Lheavenarmor")) {
            this.restrictions.push(new Restriction(("Lunar Energy: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While HP is full, gain 160 HP, 40 Vit & 25 Def."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("LJug")) {
            this.restrictions.push(new Restriction(("Lunar Eyesight: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When used, Remove all debuffs and gain +25 defense for 2 seconds."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Deeparmor")) {
            this.restrictions.push(new Restriction(("Powered Metal: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("The blue metal on this armor prevents you from being paralyzed"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("FrozenItem")) {
            this.restrictions.push(new Restriction(("Frozen: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Enemies damaged by this item will be slowed(1s)."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("MagicAB")) {
            this.restrictions.push(new Restriction(("Enchanted: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Your mana regeneration is increased by (5 Mp/s) while being under 300 mana."), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("WisDmgEffect")) {
            this.restrictions.push(new Restriction(("Damage: Wisdom"), MythicalColor, true));
            this.restrictions.push(new Restriction(("For each wisdom you have, you gain 1% extra weapon damage, must have above 30 wisdom. [Shown as crit]."), MythicalColor, false));
        }
        //WisDmgEffect
        if (this.objectXML.hasOwnProperty("CMagicAB")) {
            this.restrictions.push(new Restriction(("Universal: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain +1 Attack & Dexterity & 2 Wisdom for every 40 mana above 500."), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("FMagicAB")) {
            this.restrictions.push(new Restriction(("Set Ablaze: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain +1 Attack & Dexterity for every 30 mana above 400."), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("IMagicAB")) {
            this.restrictions.push(new Restriction(("Ice'd Out: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Gain +2 Wisdom for every 15 mana above 500."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("OryxItem")) {
            this.restrictions.push(new Restriction(("Bloodlust: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("You have a 25% chance to gain Bloodlust(3s) when hit. (+25 Crit Chance)"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("enrageoryx")) {
            this.restrictions.push(new Restriction(("Enraged Plate: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("20% chance to gain 10 Attack and 10 Dexterity when hit."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("AlienCores2")) {
            this.restrictions.push(new Restriction(("Alien's True Power: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When above 50% HP you gain a 20% increase to all stats. When below 50% HP you gain a 10% increase to all stats. When your health is at 100%, 30% increase to each stat. [Health and Mana are not affected]"), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("AlienCores2")) {
            this.restrictions.push(new Restriction(("Final: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("[Confirmed no longer obtainable and wont be in the future]"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("AlienCores")) {
            this.restrictions.push(new Restriction(("Alien's True Power: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When above 50% HP you gain a 10% increase to all stats. When below 50% HP you gain a 5% increase to all stats. When your health is at 100%, 20% increase to each stat. [Health and Mana are not affected]"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("ProtectionAB")) {
            this.restrictions.push(new Restriction(("Protection: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("there is a 25% chance to gain barrier(3s) when hit by a projectile."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Strength")) {
            this.restrictions.push(new Restriction(("Strength: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("You have a 25% chance to gain Strength(3s) or Damaging(3s) when used."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("PrismOfMagicAB")) {
            this.restrictions.push(new Restriction(("Magic Shroom: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("You gain 20% of your Magic as Health as well as 300% of your Wisdom as Mana."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Juggernaut")) {
            this.restrictions.push(new Restriction(("Legendary Juggernaut: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("You have a 25% chance to gain Strength(3s) or Damaging(3s) when used."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("HolyRobeAB")) {
            this.restrictions.push(new Restriction(("Holy Touch: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Being above 33% hp grants you immunity to most debuffs."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("OmnipotenceAB")) {
            this.restrictions.push(new Restriction(("Omniscient: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("20% to gain double the stats on this item when hit."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("ArmorNilAB")) {
            this.restrictions.push(new Restriction(("Void's Protection: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Taking 50 damage or more you gain 14 Defense, Taking 100 damage or more you gain 28 Defense."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("BloodVialAB")) {
            this.restrictions.push(new Restriction(("Life Force: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When hit for more than 100 damage you gain a third of the damage received as health back."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Resurrects")) {
            this.restrictions.push(new Restriction(("Resurrection: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, when you die you are sent back to nexus at the cost of this item."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("VoidBow")) {
            this.restrictions.push(new Restriction(("Voids Arrows: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Voided Quiver' your arrows will be enchanted, increasing your attack and dexterity by 10."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("VoidQuiver")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Bow of the Void' for a special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("MarbleAssistant")) {
            this.restrictions.push(new Restriction(("Assistance: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When used, a magic being will assist you in your battle, healing all players for 250 HP over 5 seconds. (20sqrs)"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("IceMagicAB")) {
            this.restrictions.push(new Restriction(("Ice Magic: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When using this ability, You have a 25% chance to increase your wisdom by x1.8 while being slowed for 3 seconds."), MythicalColor, false));
        }





        MythicalColor = 0xe6cc80;
        //ST Abilities

        if (this.objectXML.hasOwnProperty("OryxSword2")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you are hit for 50 damage or more you gain 14 Defense, when hit for 100 damage or more you gain 28 Defense."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("OryxArmorABY")) {
            this.restrictions.push(new Restriction(("Gladiator: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While HP is above 600, you gain Armored. While HP is below 300, you gain Healing."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("WrathWand")) {
            this.restrictions.push(new Restriction(("Abomination's Control: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Grotesque Scepter' your knowledge of wizardry will expand, increasing your wisdom by 15 and making you immune to being stunned."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("WrathScepter")) {
            this.restrictions.push(new Restriction(("Abomination's Control: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Grotesque Scepter' your knowledge of wizardry will expand, increasing your wisdom by 15 and making you immune to being stunned."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("OryxShield")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Oryx's Greatsword' for a special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("KazeArmor")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Kazekiri' for a special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("KazeKatana")) {
            this.restrictions.push(new Restriction(("Kaze Blade: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Kamishimo' the wind will assist you in ur battle, increasing your dexterity by 7, defense by 17, and health by 60."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("MagicRobe")) {
            this.restrictions.push(new Restriction(("Magic Shield: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Null-Magic Ring' you gain Enchanted increasing your mana regen by 50%."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("MagicRing")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Magic-Resistance Robe' for a special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("OryxSword1")) {
            this.restrictions.push(new Restriction(("Oryx Secret Weapons: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Oryx's Secondary Greatsword' your combat skills will evolve, increasing your attack and dexterity by 15."), MythicalColor, false));
            this.restrictions.push(new Restriction(("Oryx's Special Guard: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Champion's Bastion' your resistance level will expand, increasing your defense by 15 and vitality by 20."), MythicalColor, false));

        }

        if (this.objectXML.hasOwnProperty("ArmorAB")) {
            this.restrictions.push(new Restriction(("Magical Counter: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, when hit you deal 250 damage to 750 damage in 10 sqrs."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("BoneBow")) {
            this.restrictions.push(new Restriction(("Sharp Bone Arrows: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Quiver of Reanimation' your arrows will be made of sharp bones, increasing your attack by 15. Additionally, 10% chance of reanimation after death."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("radiantheart")) {
            this.restrictions.push(new Restriction(("Crystal Energya: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, when hit there is a 25% chance to gain 100 health and 5 attack for 10 seconds."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("MadGodRobe")) {
            this.restrictions.push(new Restriction(("Mad God's Assault: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, when hit for more than 50 damage 10% chance to gain 10 attack and 5 dexterity for 6 seconds."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("BoneQuiver")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Bow of Reanimation' for a special effect"), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("Hollyhock")) {
            this.restrictions.push(new Restriction(("Green Thumb: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, when hit there is a 25% chance to receive +10 dexterity for 5 seconds."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("BeginnersRingAB")) {
            this.restrictions.push(new Restriction(("Beginners Shield: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, there is a 25% chance to be for petrified for 3 seconds and healed for 6 seconds every time the user is hit below 300 Health."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("infernoRing")) {
            this.restrictions.push(new Restriction(("Inferno's Protection: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("While equipped, when hit for more than 50 damage 25% chance to gain 100 health 20 seconds."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("SilentRobe")) {
            this.restrictions.push(new Restriction(("Blood Seeker: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Thanks to this robe your soul feels light, but thirsty for blood. You gain resistance to bleeding and 5 dexterity during it."), MythicalColor, false));
            this.restrictions.push(new Restriction(("Use with 'Catenae's Eye' to make dexterity buff bigger."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("SilentOrb")) {
            this.restrictions.push(new Restriction(("Special Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("This item can be paired with 'Parasitic Cover' for a special effect."), MythicalColor, false));
        }

        if (this.objectXML.hasOwnProperty("HealAB")) {
            this.restrictions.push(new Restriction(("Enchanted: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Your life regeneration is increased by (15 Hp/s) while being under 45% health."), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("VargoRobe")) {
            this.restrictions.push(new Restriction(("Rune of Knowledge: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("Every 10 seconds you have ability to use your skill without wasting your mana"), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("VargoRing")) {
            this.restrictions.push(new Restriction(("Special Set Item: "), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Vargo Spell' your soul will be enchanted, increasing your mana by 150 and your wisdom by 30."), MythicalColor, false));
        }
        if (this.objectXML.hasOwnProperty("VargoSpell")) {
            this.restrictions.push(new Restriction(("Primordial seal:"), 0xeab676, true));
            this.restrictions.push(new Restriction(("Each point of your wisdom reveals the power sealed in this spell"), 0xeab676, false));
            this.restrictions.push(new Restriction(("Special Set Item:"), MythicalColor, true));
            this.restrictions.push(new Restriction(("When you equip the 'Vargo Ring' your soul will be enchanted, increasing your mana by 150 and your wisdom by 30."), MythicalColor, false));
        }
    }

    private function makeLineTwo():void {
        this.line2 = new LineBreakDesign((MAX_WIDTH - 12), 0);
        addChild(this.line2);
    }

    private function makeRestrictionText():void {
        if (this.restrictions.length != 0) {
            this.restrictionsText = new TextFieldDisplayConcrete().setSize(14).setColor(0xB3B3B3).setTextWidth((MAX_WIDTH - 4)).setIndent(-10).setLeftMargin(10).setWordWrap(true).setHTML(true);
            this.restrictionsText.setStringBuilder(this.buildRestrictionsLineBuilder());
            this.restrictionsText.filters = getoutlinetext();
            waiter.push(this.restrictionsText.textChanged);
            addChild(this.restrictionsText);
        }
    }

    private function buildRestrictionsLineBuilder():StringBuilder {
        var _local2:Restriction;
        var _local3:String;
        var _local4:String;
        var _local5:String;
        var _local1:AppendingLineBuilder = new AppendingLineBuilder();
        for each (_local2 in this.restrictions) {
            _local3 = ((_local2.bold_) ? "<b>" : "");
            _local3 = _local3.concat((('<font color="#' + _local2.color_.toString(16)) + '">'));
            _local4 = "</font>";
            _local4 = _local4.concat(((_local2.bold_) ? "</b>" : ""));
            _local5 = ((this.player) ? ObjectLibrary.typeToDisplayId_[this.player.objectType_] : "");
            _local1.pushParams(_local2.text_, {
                "unUsableClass": _local5,
                "usableClasses": this.getUsableClasses(),
                "keyCode": KeyCodes.CharCodeStrings[Parameters.data_.useSpecial]
            }, _local3, _local4);
        }
        return (_local1);
    }

    private function getUsableClasses():StringBuilder {
        var _local3:String;
        var _local1:Vector.<String> = ObjectLibrary.usableBy(this.objectType);
        var _local2:AppendingLineBuilder = new AppendingLineBuilder();
        _local2.setDelimiter(", ");
        for each (_local3 in _local1) {
            _local2.pushParams(_local3);
        }
        return (_local2);
    }

    private function addDescriptionText():void {
        this.descText = new TextFieldDisplayConcrete().setSize(14).setColor(0xB3B3B3).setTextWidth(MAX_WIDTH).setWordWrap(true);
        if (this.descriptionOverride) {
            this.descText.setStringBuilder(new StaticStringBuilder(this.descriptionOverride));
        }
        else {
            this.descText.setStringBuilder(new LineBuilder().setParams(String(this.objectXML.Description)));
        }
        this.descText.filters = getoutlinetext();
        waiter.push(this.descText.textChanged);
        addChild(this.descText);
    }

    override protected function alignUI():void {
        this.titleText.x = (this.icon.width + 4);
        this.titleText.y = ((this.icon.height / 2) - (this.titleText.height / 2));
        if (this.tierText) {
            this.tierText.y = ((this.icon.height / 3) - (this.tierText.height / 3));
            this.tierText.x = (MAX_WIDTH - 30);
        }
        this.descText.x = 4;
        this.descText.y = (this.icon.height + 2);
        if (contains(this.line1)) {
            this.line1.x = 8;
            this.line1.y = ((this.descText.y + this.descText.height) + 8);
            this.effectsText.x = 4;
            this.effectsText.y = (this.line1.y + 8);
        }
        else {
            this.line1.y = (this.descText.y + this.descText.height);
            this.effectsText.y = this.line1.y;
        }
        this.line2.x = 8;
        this.line2.y = ((this.effectsText.y + this.effectsText.height) + 8);
        var _local1:uint = (this.line2.y + 8);
        if (this.restrictionsText) {
            this.restrictionsText.x = 4;
            this.restrictionsText.y = _local1;
            _local1 = (_local1 + this.restrictionsText.height);
        }
        if (this.powerText) {
            if (contains(this.powerText)) {
                this.powerText.x = 4;
                this.powerText.y = _local1;
            }
        }
    }

    private function buildCategorySpecificText():void {
        if (this.curItemXML != null) {
            this.comparisonResults = this.slotTypeToTextBuilder.getComparisonResults(this.objectXML, this.curItemXML);
        }
        else {
            this.comparisonResults = new SlotComparisonResult();
        }
    }



    private function modifyWisModStat(_arg1:String, _arg2:Number = 1):String {
        var _local5:Number;
        var _local6:int;
        var _local7:Number;
        var _local3:String = "-1";
        var _local4:Number = (this.player.wisdom_ + this.player.wisdomBoost_);
        if (_local4 < 30) {
            _local3 = _arg1;
        }
        else {
            _local5 = Number(_arg1);
            _local6 = (((_local5) < 0) ? -1 : 1);
            _local7 = (((_local5 * _local4) / 150) + (_local5 * _local6));
            _local7 = (Math.floor((_local7 * Math.pow(10, _arg2))) / Math.pow(10, _arg2));
            if ((_local7 - (int(_local7) * _local6)) >= ((1 / Math.pow(10, _arg2)) * _local6)) {
                _local3 = _local7.toFixed(1);
            }
            else {
                _local3 = _local7.toFixed(0);
            }
        }
        return (_local3);
    }


}
}

import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

class Effect {

    public var name_:String;
    public var valueReplacements_:Object;
    public var replacementColor_:uint = 16777103;
    public var color_:uint = 0xB3B3B3;

    public function Effect(_arg1:String, _arg2:Object) {
        this.name_ = _arg1;
        this.valueReplacements_ = _arg2;
    }

    public function setColor(_arg1:uint):Effect {
        this.color_ = _arg1;
        return (this);
    }

    public function setReplacementsColor(_arg1:uint):Effect {
        this.replacementColor_ = _arg1;
        return (this);
    }

    public function getValueReplacementsWithColor():Object {
        var _local4:String;
        var _local5:LineBuilder;
        var _local1:Object = {};
        var _local2:String = "";
        var _local3:String = "";
        if (this.replacementColor_) {
            _local2 = (('</font><font color="#' + this.replacementColor_.toString(16)) + '">');
            _local3 = (('</font><font color="#' + this.color_.toString(16)) + '">');
        }
        for (_local4 in this.valueReplacements_) {
            if ((this.valueReplacements_[_local4] is AppendingLineBuilder)) {
                _local1[_local4] = this.valueReplacements_[_local4];
            }
            else {
                if ((this.valueReplacements_[_local4] is LineBuilder)) {
                    _local5 = (this.valueReplacements_[_local4] as LineBuilder);
                    _local5.setPrefix(_local2).setPostfix(_local3);
                    _local1[_local4] = _local5;
                }
                else {
                    _local1[_local4] = ((_local2 + this.valueReplacements_[_local4]) + _local3);
                }
            }
        }
        return (_local1);
    }

}


class Restriction {

    public var text_:String;
    public var color_:uint;
    public var bold_:Boolean;

    public function Restriction(_arg1:String, _arg2:uint, _arg3:Boolean) {
        this.text_ = _arg1;
        this.color_ = _arg2;
        this.bold_ = _arg3;
    }

}

