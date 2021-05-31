/**package ToolForge.forgeControls
{
    import com.company.assembleegameclient.game.GameSprite;
    import ToolForge.ForgeInventorySlot;

import com.company.assembleegameclient.objects.Player;

public class DamageControl extends ForgeControl
    {
        private static const CARD:int = 9018;

        public function DamageControl(gameSprite:GameSprite)
        {
            super(gameSprite);
            setHelp(DamageHelp);
            drawSlot(INPUT_TARGET, 0, EQUIPMENT_ANY);
            drawSlot(INPUT_ITEM, CARD);
            drawSlot(OUTPUT);
        }

        override public function update(object:Object, string:String) : void
        {
            //var _local_3:Number = object.fame;
            var _local_4:ForgeInventorySlot = getSlot(INPUT_TARGET);
            var _local_5:ForgeInventorySlot = getSlot(OUTPUT);
            var _local_6:ForgeInventorySlot = getSlot(INPUT_ITEM);
            //object.fame = (object.fame + _local_6.mergeItemData(_local_7, "DamageMultiplier"));
            if (string != "")
            {
                //_local_7.Name = string;
            }
            //if (((!((object.fame == _local_3))) || (!((string == "")))))
            //{
                //_local_5.updateItemData(_local_4, _local_7);
                _local_5.setTitle("Enchantment Price:", {});
                _local_5.setDescription((" Fame"), {});
            //} else
           // {
                _local_5.reset();
           // }
        }

    }
}

import ToolForge.forgeHelp.ForgeHelpWindow;
import ToolForge.forgeHelp.ForgeHelpImageTextElement;
import com.company.assembleegameclient.objects.ObjectLibrary;

class DamageHelp extends ForgeHelpWindow 
{

    public function DamageHelp()
    {
        super("Damage Multiplier");
        addText("Damage Multiplier is a really powerful enchantment, its compulsory for each class");
        addText("The enchantment can be maxed out till you reach 400% plus damage on your item.");
        addUiElement(new ForgeHelpImageTextElement(ObjectLibrary.getRedrawnTextureFromType(9018, 80, false), ("The Devil Tarot Card will apply a 1% Damage Multiplier to your target item.\n" + "Its the most common damage multiplier that drops from Abyss Idol.")));
        addUiElement(new ForgeHelpImageTextElement(ObjectLibrary.getRedrawnTextureFromType(3861, 80, false), ("The Fool Tarot Card will apply a 2% Damage Multiplier to your target item.\n" + "Its pretty uncommon multiplier that drops from The Puppet Master.")));
        addUiElement(new ForgeHelpImageTextElement(ObjectLibrary.getRedrawnTextureFromType(9020, 80, false), ("Death Tarot Card will apply a 3% Damage Multiplier to your target item.\n" + "Its Very rare Multiplier and does only drop from Lord Ruthven.")));
        addCloseButton();
    }
}**/