// Decompiled by AS3 Sorcerer 3.16
// http://www.as3sorcerer.com/

//ToolForge.forgeHelp.ForgeHelpImageTextElement

package ToolForge.forgeHelp
{
    import flash.display.Bitmap;
    import flash.display.BitmapData;

    public class ForgeHelpImageTextElement extends ForgeHelpWindowUIElement 
    {

        private var image:Bitmap;

        public function ForgeHelpImageTextElement(_arg_1:BitmapData, _arg_2:String)
        {
            this.image = new Bitmap(_arg_1);
            addChild(this.image);
            addText(_arg_2).setPosition((this.image.width + 5), 0).setTextWidth((WIDTH - (this.image.width - 5)));
        }
    }
}//package ToolForge.forgeHelp

