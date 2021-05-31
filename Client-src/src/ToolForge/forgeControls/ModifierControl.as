/**package ToolForge.forgeControls
{
    import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.objects.Player;

public class ModifierControl extends ForgeControl
    {
        public static const CRYSTAL:int = 3189;

        public function ModifierControl(_arg_1:GameSprite)
        {
            super(_arg_1);
            drawSlot(INPUT_TARGET, 0, EQUIPMENT_ANY);
            drawSlot(INPUT_ITEM, CRYSTAL);
            drawSlot(INPUT_ITEM, CRYSTAL);
            drawSlot(OUTPUT);
        }
        override public function update(_arg_1:Object, _arg_2:String):void
        {
        }

    }
} **/