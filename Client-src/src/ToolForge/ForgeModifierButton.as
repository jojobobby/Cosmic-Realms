package ToolForge
{
    import com.company.assembleegameclient.ui.tooltip.TextToolTip;
    import com.company.assembleegameclient.ui.tooltip.ToolTip;
    import flash.display.Sprite;
    import flash.display.Bitmap;
    import com.company.assembleegameclient.game.GameSprite;
    import flash.events.MouseEvent;
    import flash.display.Shape;
    import flash.display.BitmapData;
    import ToolForge.forgeControls.ForgeControl;
    import org.osflash.signals.Signal;

    public class ForgeModifierButton extends Sprite
    {
        public static const WIDTH:int = 40;
        public static const HEIGHT:int = 40;

        public var tooltipSignal:Signal;
        public var onSelect:Signal;
        private var icon:Bitmap;
        private var tooltipText:String;
        private var targetClass:Class;
        private var selected:Boolean;
        private var over:Boolean;
        private var gs:GameSprite;

        public function ForgeModifierButton(gameSprite:GameSprite, s:String, bitmapData:BitmapData, cls:Class)
        {
            this.gs = gameSprite;
            this.tooltipText = s;
            this.tooltipSignal = new Signal(ToolTip);
            this.onSelect = new Signal(ForgeModifierButton);
            this.targetClass = cls;
            this.icon = new Bitmap(bitmapData);
            addChild(this.icon);
            this.draw();
            this.icon.x = ((WIDTH / 2) - (this.icon.width / 2));
            this.icon.y = ((HEIGHT / 2) - (this.icon.height / 2));
            addEventListener(MouseEvent.ROLL_OVER, this.onRollOver);
            addEventListener(MouseEvent.ROLL_OUT, this.onRollOut);
            addEventListener(MouseEvent.CLICK, this.onClick);
            var shape:Shape = new Shape();
            shape.graphics.beginFill(0);
            shape.graphics.drawRect(-1, -1, (WIDTH + 2), (HEIGHT + 2));
            shape.graphics.endFill();
            addChild(shape);
            mask = shape;
        }

        public function select():void
        {
            this.selected = true;
            this.draw();
            this.onSelect.dispatch(this);
        }

        public function deselect():void
        {
            this.selected = false;
            this.draw();
        }

        public function createControl():ForgeControl
        {
            return (((this.targetClass) ? new this.targetClass(this.gs) : null));
        }

        private function draw():void
        {
            var i:uint = ((((this.selected) || (this.over))) ? 0x828282 : 0x969696);
            var i1:uint = ((this.selected) ? 0x454545 : ((this.over) ? 0xB5B5B5 : 0xFFFFFF));
            var i2:uint = ((this.selected) ? 0xFFFFFF : ((this.over) ? 0x404040 : 0x454545));
            graphics.clear();
            graphics.beginFill(i, 1);
            graphics.drawRect(0, 0, WIDTH, HEIGHT);
            graphics.endFill();
            graphics.lineStyle(2, i1, 1);
            graphics.moveTo(0, HEIGHT);
            graphics.lineTo(0, 0);
            graphics.lineTo(WIDTH, 0);
            graphics.lineStyle(2, i2, 1);
            graphics.lineTo(WIDTH, HEIGHT);
            graphics.lineTo(0, HEIGHT);
            graphics.lineStyle();
        }

        private function onRollOut(event:MouseEvent):void
        {
            this.over = false;
            this.draw();
        }

        private function onRollOver(event:MouseEvent):void
        {
            var _local_2:ToolTip = new TextToolTip(0x363636, 0x9B9B9B, null, this.tooltipText, 200, {});
            _local_2.attachToTarget(this);
            this.tooltipSignal.dispatch(_local_2);
            this.over = true;
            this.draw();
        }

        private function onClick(event:MouseEvent):void
        {
            this.select();
        }
    }
}