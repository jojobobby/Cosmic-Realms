package com.company.assembleegameclient.ui.tooltip.slotcomparisons {
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;
import com.company.assembleegameclient.util.ConditionEffect;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class ShieldComparison extends SlotComparison {

    private var projectileComparison:GeneralProjectileComparison;
    private var condition:XMLList;
    private var otherCondition:XMLList;

    public function ShieldComparison() {
        this.projectileComparison = new GeneralProjectileComparison();
        super();
    }

    override protected function compareSlots(itemXML:XML, curItemXML:XML):void {
        var tagStr:String;
        var duration:Number;
        var conditionEffect:ConditionEffect;
        this.condition = itemXML.Projectile.ConditionEffect.(((((text() == "Armor Broken")) || (((text() == "Slowed")) || ((text() == "Stunned"))) || ((text() == "Paralyzed")))) || ((text() == "Dazed")));
        this.otherCondition = curItemXML.Projectile.ConditionEffect.((((((text() == "Armor Broken")) || ((text() == "Slowed")) || ((text() == "Paralyzed")))) || ((text() == "Dazed"))) || ((text() == "Stunned")));
        this.projectileComparison.compare(itemXML, curItemXML);
        comparisonStringBuilder = this.projectileComparison.comparisonStringBuilder;
        for (tagStr in this.projectileComparison.processedTags) {
            processedTags[tagStr] = true;
        }
        if ((((this.condition.length() == 1)) && ((this.otherCondition.length() == 1)))) {
            duration = Number(this.condition[0].@duration);
            conditionEffect = ConditionEffect.getConditionEffectEnumFromName(this.condition.text());
            comparisonStringBuilder.pushParams(TextKey.SHOT_EFFECT, {"effect": ""});
            comparisonStringBuilder.pushParams(TextKey.EFFECT_FOR_DURATION, {
                "effect": new LineBuilder().setParams(conditionEffect.localizationKey_),
                "duration": duration
            }, TooltipHelper.getOpenTag(NO_DIFF_COLOR), TooltipHelper.getCloseTag());
            processedTags[this.condition[0].toXMLString()] = true;
        }
        this.handleException(itemXML);
    }

    private function handleException(itemXML:XML):void { //useful for special items
        var tag:XML;
        var innerLineBuilder:LineBuilder;
        if (itemXML.@id == "ItemName") {
            tag = itemXML.ConditionEffect.(text() == "Effect")[0];
            innerLineBuilder = new LineBuilder().setParams(TextKey.EFFECT_FOR_DURATION, {
                "effect": TextKey.wrapForTokenResolution(TextKey.ACTIVE_EFFECT_ARMOR_BROKEN),
                "duration": tag.@duration
            }).setPrefix(TooltipHelper.getOpenTag(UNTIERED_COLOR)).setPostfix(TooltipHelper.getCloseTag());
            comparisonStringBuilder.pushParams(TextKey.SHOT_EFFECT, {"effect": innerLineBuilder});
            processedTags[tag.toXMLString()] = true;
        }

    }


}
}
