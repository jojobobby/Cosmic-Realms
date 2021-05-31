package com.company.assembleegameclient.ui.tooltip.slotcomparisons {
import com.company.assembleegameclient.ui.tooltip.TooltipHelper;
import kabam.rotmg.text.view.stringBuilder.AppendingLineBuilder;
import com.company.assembleegameclient.objects.Player;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

public class PrismComparison extends SlotComparison {

    private var decoy:XMLList;
    private var otherDecoy:XMLList;

    override protected function compareSlots(itemXML:XML, curItemXML:XML):void {
        this.decoy = itemXML.Activate.(text() == "Decoy");
        this.otherDecoy = curItemXML.Activate.(text() == "Decoy");
        comparisonStringBuilder = new AppendingLineBuilder();
    }


}
}
