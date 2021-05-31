// Decompiled by AS3 Sorcerer 3.16
// http://www.as3sorcerer.com/

//ToolForge.forgeHelp.ForgeHelpWindowUIElement

package ToolForge.forgeHelp
{

import org.osflash.signals.Signal;

public class ForgeHelpWindowUIElement extends ForgeHelpWindow
    {

        public var onChange:Signal;

        public function ForgeHelpWindowUIElement()
        {
            super(null);
            this.onChange = new Signal();
        }
        override protected function alignUi():void
        {
            super.alignUi();
            this.onChange.dispatch();
        }

    }
}//package ToolForge.forgeHelp

