
package ToolForge.forgeControls
{
    import com.company.assembleegameclient.game.GameSprite;
    import ToolForge.ForgeInventorySlot;

import com.company.assembleegameclient.objects.Player;

public class ManaControl extends ForgeControl
    {
        private static const CONCH:int = 3189;

        public function ManaControl(gameSprite:GameSprite)
        {
            super(gameSprite);
            setHelp(ManaHelp);
            drawSlot(INPUT_TARGET, 0, EQUIPMENT_ANY);
            drawSlot(INPUT_ITEM, CONCH);
            drawSlot(OUTPUT);
        }

        override public function update(_arg_1:Object, _arg_2:String):void
        {
            //var _local_3:Number = _arg_1.fame;
            var _local_4:ForgeInventorySlot = getSlot(INPUT_TARGET);
            var _local_5:ForgeInventorySlot = getSlot(OUTPUT);
            var _local_6:ForgeInventorySlot = getSlot(INPUT_ITEM);
            //var _local_7:ItemData = _local_4.getItemData();
            //if (_local_7 == null){
                //_local_5.reset();
                //return;
            //};
            //_arg_1.fame = (_arg_1.fame + _local_6.mergeItemData(_local_7, "ManaCost"));
            if (_arg_2 != ""){
                //_local_7.Name = _arg_2;
            }
            //if (((!((_arg_1.fame == _local_3))) || (!((_arg_2 == ""))))){
                //_local_5.updateItemData(_local_4, _local_7);
                _local_5.setTitle("Enchantment Price:", {});
                //_local_5.setDescription((_arg_1.fame + " Fame"), {});
            //} else {
                _local_5.reset();
            //};
        }

    }
}

import ToolForge.forgeHelp.ForgeHelpWindow;
import ToolForge.forgeHelp.ForgeHelpImageTextElement;
import com.company.assembleegameclient.objects.ObjectLibrary;

class ManaHelp extends ForgeHelpWindow 
{

    public function ManaHelp()
    {
        super("Mana Cost Reduction");
        addText("Mana reduction is a really powerful enchantment, its recommend to use that enchantment on supporter classes like priest or paladin.");
        addText("The enchantment can be maxed out till you reach 90% reduction on your item.");
        addUiElement(new ForgeHelpImageTextElement(ObjectLibrary.getRedrawnTextureFromType(3187, 80, false), ("The Golden Cockle will apply a 1% mana reduction to your target item.\n" + "Its the most common mana modifier that drops from Thessal and Coral Gifts.")));
        addUiElement(new ForgeHelpImageTextElement(ObjectLibrary.getRedrawnTextureFromType(3188, 80, false), ("The Golden Conch will apply a 2% mana reduction to your target item.\n" + "Its pretty uncommon modifier that drops from Thessal.")));
        addUiElement(new ForgeHelpImageTextElement(ObjectLibrary.getRedrawnTextureFromType(3189, 80, false), ("The Golden Horn Conch will apply a 3% mana reduction to your target item.\n" + "Its a very rare modifier and does only drop from Coral Gifts.")));
        addCloseButton();
    }
}
