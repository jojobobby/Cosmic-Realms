package ToolForge
{
    import flash.display.Sprite;
    import com.company.assembleegameclient.game.GameSprite;
    import ToolForge.forgeControls.ForgeControl;
    import com.company.assembleegameclient.objects.ObjectLibrary;
    import ToolForge.forgeControls.PresentControl;
    import flash.display.BitmapData;
    import org.osflash.signals.Signal;

    public class ForgeModifierButtonHolder extends Sprite
    {
        public var modifierChanged:Signal;
        private var buttons:Vector.<ForgeModifierButton>;
        private var selectedButton:ForgeModifierButton;
        private var gs:GameSprite;

        public function ForgeModifierButtonHolder(gameSprite:GameSprite, isListButton:Boolean = false)
        {
            this.gs = gameSprite;
            this.modifierChanged = new Signal(ForgeControl);
            this.buttons = new Vector.<ForgeModifierButton>();
            //this.addModifier("Add Modifiers", ObjectLibrary.getRedrawnTextureFromType(25000, 40, true), ModifierControl);
            //this.addModifier("Damage Multiplier", ObjectLibrary.getRedrawnTextureFromType(0x0B0B, 60, true), DamageControl);
            //this.addModifier("Mana Cost", ObjectLibrary.getRedrawnTextureFromType(2794, 60, true), ManaControl);
            if(!isListButton){
                this.addModifier("Forge", ObjectLibrary.getRedrawnTextureFromType(0x4a23, 72, true), PresentControl);
            }else{
                this.addModifier("Forge List", ObjectLibrary.getRedrawnTextureFromType(0x4a22,64,true), PresentControl);
            }
        }

        public function selectDefault():void
        {
            this.buttons[0].select();
        }

        private function addModifier(s:String, bitmapData:BitmapData, cls:Class):void
        {
            var button:ForgeModifierButton;
            button = new ForgeModifierButton(this.gs, s, bitmapData, cls);
            button.onSelect.add(this.onButtonSelect);
            button.x = (((this.buttons.length % 3) * ForgeModifierButton.WIDTH) + ((this.buttons.length % 3) * 5));
            button.y = ((int((this.buttons.length / 3)) * ForgeModifierButton.HEIGHT) + (int((this.buttons.length / 3)) * 5));
            addChild(button);
            this.buttons.push(button);
        }

        private function onButtonSelect(button:ForgeModifierButton):void
        {
            if (this.selectedButton == button)
            {
                return;
            }
            if (this.selectedButton)
            {
                this.selectedButton.deselect();
            }
            this.selectedButton = button;
            this.modifierChanged.dispatch(button.createControl());
        }
    }
}