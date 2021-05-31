package com.company.assembleegameclient.ui.tooltip.slotcomparisons {
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class TomeComparison extends SlotComparison {


    override protected function compareSlots(itemXML:XML, curItemXML:XML):void {
        var tag:XML;
        comparisonStringBuilder = new AppendingLineBuilder();
        if (itemXML.@id == "Tome of Purification" || itemXML.@id == "Tome of Exorcism") {
            tag = itemXML.Activate.(text() == "RemoveNegativeConditions")[0];
            comparisonStringBuilder.pushParams(TextKey.REMOVES_NEGATIVE, {}, TooltipHelper.getOpenTag(UNTIERED_COLOR), TooltipHelper.getCloseTag());
            processedTags[tag.toXMLString()] = true;
        }
        else {
            if (itemXML.@id == "Tome of Holy Protection") {
                tag = itemXML.Activate.(text() == "ConditionEffectSelf")[0];
                comparisonStringBuilder.pushParams(TextKey.EFFECT_ON_SELF, {"effect": ""});
                comparisonStringBuilder.pushParams(TextKey.EFFECT_FOR_DURATION, {
                    "effect": TextKey.wrapForTokenResolution(TextKey.ACTIVE_EFFECT_ARMORED),
                    "duration": tag.@duration
                }, TooltipHelper.getOpenTag(UNTIERED_COLOR), TooltipHelper.getCloseTag());
                processedTags[tag.toXMLString()] = true;
            }
        }
    }


}
}
