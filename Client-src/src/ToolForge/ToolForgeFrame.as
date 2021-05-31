package ToolForge
{
import ToolForge.forgeList.ForgeListPanel;

    import com.company.assembleegameclient.ui.DeprecatedClickableText;
    import com.company.util.GraphicsUtil;
    import flash.display.Sprite;
    import flash.display.IGraphicsData;
    import flash.display.GraphicsSolidFill;
    import flash.display.GraphicsStroke;
    import flash.display.GraphicsPath;
    import com.company.assembleegameclient.game.GameSprite;
    import ToolForge.forgeControls.ForgeControl;
    import flash.display.LineScaleMode;
    import flash.display.CapsStyle;
    import flash.display.JointStyle;
    import flash.events.MouseEvent;
    import flash.events.Event;
    import kabam.rotmg.account.web.view.LabeledField;
    import kabam.rotmg.text.view.TextFieldDisplayConcrete;
    import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
    import mx.utils.StringUtil;
    import org.osflash.signals.Signal;

    public class ToolForgeFrame extends Sprite
    {
        private var graphicsData:Vector.<IGraphicsData>;
        private var innerFill:GraphicsSolidFill;
        private var outlineFill:GraphicsSolidFill;
        private var outlineStroke:GraphicsStroke;
        private var path1:GraphicsPath;
        public var close:Signal;
        public var forgeItem:Signal;
        private var itemName:LabeledField;
        private var title:TextFieldDisplayConcrete;
        private var gs:GameSprite;
        private var inventory:ForgePlayerInventory;
        private var forgeControl:ForgeControl;
        private var closeButton:DeprecatedClickableText;
        private var buttons:ForgeModifierButtonHolder;

        private var listPanel:ForgeListPanel;
        private var listBtn:Sprite;

        public function ToolForgeFrame(gameSprite:GameSprite)
        {
            this.gs = gameSprite;
            this.forgeItem = new Signal(GameSprite, ForgeItemInformation);
            this.title = new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(21).setBold(true).setStringBuilder(new StaticStringBuilder("Forge Station"));
            addChild(this.title);

            this.buttons = new ForgeModifierButtonHolder(this.gs);
            this.buttons.x = -((this.buttons.width + 10));
            this.buttons.modifierChanged.add(this.updateModifier);
            addChild(this.buttons);
            this.buttons.selectDefault();

            this.closeButton = new DeprecatedClickableText(18, true, "Close");
            this.closeButton.buttonMode = true;
            this.closeButton.x = 15;
            this.closeButton.y = 360;
            addChild(this.closeButton);

            this.listBtn = new ForgeModifierButtonHolder(this.gs, true);
            this.listBtn.x = this.buttons.x;
            this.listBtn.y = this.buttons.y + ForgeModifierButton.HEIGHT + 5
            this.buttons.modifierChanged.add(this.updateModifier);
            this.listBtn.addEventListener(MouseEvent.CLICK, showPanel);
            addChild(this.listBtn);

            this.innerFill = new GraphicsSolidFill(0x363636, 1);
            this.outlineFill = new GraphicsSolidFill(0xFFFFFF, 1);
            this.outlineStroke = new GraphicsStroke(1, false, LineScaleMode.NORMAL, CapsStyle.NONE, JointStyle.ROUND, 3, this.outlineFill);
            this.path1 = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());
            this.graphicsData = new <IGraphicsData>[this.innerFill, this.path1, GraphicsUtil.END_FILL, this.outlineStroke, this.path1, GraphicsUtil.END_STROKE];
            this.close = new Signal();
            this.itemName = new LabeledField("", false);
            this.itemName.inputText_.restrict = "A-Za-z0-9 ";
            this.itemName.inputText_.maxChars = 25;
            this.itemName.y = -28;
            this.itemName.x = (400 - (this.itemName.width + 10));
            //addChild(this.itemName);
            this.inventory = new ForgePlayerInventory(this.gs.map.player_);
            this.inventory.x = 20;
            this.inventory.y = 185;
            addChild(this.inventory);
            this.closeButton.addEventListener(MouseEvent.CLICK, this.onCloseClick);
            addEventListener(Event.ENTER_FRAME, this.onEnterFrame);
            GraphicsUtil.clearPath(this.path1);
            GraphicsUtil.drawCutEdgeRect(-6, -6, 400, 400, 4, [1, 1, 1, 1], this.path1);
            this.x = 180;
            this.y = 125;
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }

        private function showPanel(e:MouseEvent):void {
            this.listPanel = new ForgeListPanel(this.gs);
            this.listPanel.x = 100 - ((800 - this.width) / 2);
            this.listPanel.y = -125;

            addChild(this.listPanel);
        }

        private function onForge(information:ForgeItemInformation):void
        {
            information.newName = StringUtil.trim(this.itemName.text());
            this.forgeItem.dispatch(this.gs, information);
        }

        private function updateModifier(control:ForgeControl):void
        {
            if (((this.forgeControl) && (contains(this.forgeControl))))
            {
                this.forgeControl.forge.remove(this.onForge);
                removeChild(this.forgeControl);
                this.forgeControl = null;
            }
            this.forgeControl = control;
            this.forgeControl.x = 20;
            this.forgeControl.y = 40;
            this.forgeControl.forge.add(this.onForge);
            addChild(this.forgeControl);
        }

        private function draw():void
        {
            graphics.clear();
            graphics.drawGraphicsData(this.graphicsData);
        }

        private function onCloseClick(event:MouseEvent):void
        {
            this.close.dispatch();
        }

        private function onEnterFrame(event:Event):void
        {
            var object:Object;
            var s:String;
            this.draw();
            if (this.forgeControl != null)
            {
                //object = {"fame":0};
                s = StringUtil.trim(this.itemName.text());
                if (s != "")
                {
                    //object.fame = (object.fame + 20);
                }
                this.forgeControl.update(object, s);
            }
        }

        private function onRemovedFromStage(event:Event):void
        {
            removeEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
            removeEventListener(Event.ENTER_FRAME, this.onEnterFrame);
        }
    }
}