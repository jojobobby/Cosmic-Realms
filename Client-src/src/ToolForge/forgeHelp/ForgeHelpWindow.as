// Decompiled by AS3 Sorcerer 3.16
// http://www.as3sorcerer.com/

//ToolForge.forgeHelp.ForgeHelpWindow

package ToolForge.forgeHelp
{
import com.company.util.GraphicsUtil;

import flash.display.Sprite;
    import flash.display.DisplayObject;
    import flash.events.Event;
    import flash.display.GraphicsSolidFill;
    import flash.display.GraphicsStroke;
    import flash.display.LineScaleMode;
    import flash.display.CapsStyle;
    import flash.display.JointStyle;
    import flash.display.GraphicsPath;
    import flash.display.IGraphicsData;

import kabam.rotmg.pets.view.components.DialogCloseButton;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

import kabam.rotmg.ui.view.SignalWaiter;

import org.osflash.signals.Signal;

public class ForgeHelpWindow extends Sprite
    {

        public static const WIDTH:int = 360;

        private var textsChanged:SignalWaiter;
        private var uiElements:Vector.<DisplayObject>;
        public var onClose:Signal;

        public function ForgeHelpWindow(_arg_1:String)
        {
            this.textsChanged = new SignalWaiter();
            this.uiElements = new Vector.<DisplayObject>();
            this.onClose = new Signal(ForgeHelpWindow);
            this.textsChanged.complete.add(this.alignUi);
            if (_arg_1 != null){
                this.addText(_arg_1, false).setBold(true).setSize(21).name = "HeadingText";
            }
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
        }
        private static function makeText(_arg_1:String, _arg_2:Boolean):TextFieldDisplayConcrete
        {
            return (new TextFieldDisplayConcrete().setColor(0xFFFFFF).setSize(16).setMultiLine(true).setWordWrap(true).setTextWidth(WIDTH).setHTML(_arg_2).setStringBuilder(new LineBuilder().setParams(_arg_1)));
        }

        final protected function addText(_arg_1:String, _arg_2:Boolean=false):TextFieldDisplayConcrete
        {
            var _local_3:TextFieldDisplayConcrete = makeText(_arg_1, _arg_2);
            this.uiElements.push(_local_3);
            this.textsChanged.push(_local_3.textChanged);
            addChild(_local_3);
            return (_local_3);
        }
        final protected function addUiElement(_arg_1:ForgeHelpWindowUIElement):ForgeHelpWindowUIElement
        {
            this.uiElements.push(_arg_1);
            this.textsChanged.push(_arg_1.onChange);
            addChild(_arg_1);
            return (_arg_1);
        }
        final protected function addCloseButton():DialogCloseButton
        {
            var _local_1:DialogCloseButton;
            _local_1 = new DialogCloseButton(1);
            _local_1.disabled = false;
            _local_1.clicked.add(this.onCloseClick);
            addChild(_local_1);
            _local_1.x = (WIDTH - _local_1.width);
            return (_local_1);
        }
        private function onCloseClick():void
        {
            this.onClose.dispatch(this);
        }
        protected function alignUi():void
        {
            var _local_1:DisplayObject;
            var _local_2:Number = 0;
            for each (_local_1 in this.uiElements) {
                _local_1.y = _local_2;
                _local_2 = (_local_2 + (_local_1.height + (((_local_1.name == "HeadingText")) ? 15 : 10)));
            }
            this.draw();
        }
        private function onAddedToStage(_arg_1:Event):void
        {
            removeEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            this.draw();
        }
        private function draw():void
        {
            if ((parent is ForgeHelpWindow)){
                return;
            }
            var _local_1:GraphicsSolidFill = new GraphicsSolidFill(0x363636, 1);
            var _local_2:GraphicsSolidFill = new GraphicsSolidFill(0xFFFFFF, 1);
            var _local_3:GraphicsStroke = new GraphicsStroke(1, false, LineScaleMode.NORMAL, CapsStyle.NONE, JointStyle.ROUND, 3, _local_2);
            var _local_4:GraphicsPath = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());
            var _local_5:Vector.<IGraphicsData> = new <IGraphicsData>[_local_1, _local_4, GraphicsUtil.END_FILL, _local_3, _local_4, GraphicsUtil.END_STROKE];
            GraphicsUtil.clearPath(_local_4);
            GraphicsUtil.drawCutEdgeRect(-6, -6, width, (height + 5), 4, [1, 1, 1, 1], _local_4);
            graphics.clear();
            graphics.drawGraphicsData(_local_5);
            this.x = (400 - (this.width / 2));
            this.y = (300 - (this.height / 2));
        }

    }
}//package ToolForge.forgeHelp

