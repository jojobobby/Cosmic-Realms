package ToolForge.forgeHelp
{
    import flash.display.Sprite;
    import flash.display.Bitmap;
    import com.company.assembleegameclient.objects.ObjectLibrary;
    import com.company.util.AssetLibrary;
    import flash.events.MouseEvent;
    import flash.display.Shape;
    import org.osflash.signals.Signal;

    public class ForgeHelpButton extends Sprite
    {
        public static const WIDTH:int = 40;
        public static const HEIGHT:int = 40;

        private var icon:Bitmap;
        private var infoClass:Class;
        private var over:Boolean;
        public var onInfoClick:Signal;

        public function ForgeHelpButton(cls:Class)
        {
            this.infoClass = cls;
            this.onInfoClick = new Signal(ForgeHelpButton);
            this.icon = new Bitmap(ObjectLibrary.getRedrawnTextureFromType(int(AssetLibrary.getImageFromSet("lofiInterfaceBig", 12)), 40, false));
            addChild(this.icon);
            this.draw();
            this.icon.x = -8;
            this.icon.y = -8;
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
            this.draw();
            this.onInfoClick.dispatch(this);
        }

        public function createHelp():ForgeHelpWindow
        {
            return (((this.infoClass) ? new this.infoClass() : null));
        }

        private function draw():void
        {
            var i:uint = ((this.over) ? 0x828282 : 0x969696);
            var i1:uint = ((this.over) ? 0xB5B5B5 : 0xFFFFFF);
            var i2:uint = ((this.over) ? 0x404040 : 0x454545);
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
            this.over = true;
            this.draw();
        }

        private function onClick(event:MouseEvent):void
        {
            this.select();
        }
    }
}