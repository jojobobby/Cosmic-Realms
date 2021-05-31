package ToolForge
{
    import flash.utils.Dictionary;
    import ToolForge.forgeControls.ForgeControl;

    public class ForgeSlotCollection 
    {
        private var slots:Dictionary;
        private var parent:ForgeControl;

        public function ForgeSlotCollection(control:ForgeControl)
        {
            this.slots = new Dictionary();
            this.parent = control;
        }

        private static function countY(i:int):Array
        {
            var height:int = ForgeControl.HEIGHT;
            var number:Number = ((45 * i) + (5 * i));
            var carrau:Array = [];
            height = ((height - number) / 2);
            var _local_5:int;
            while (_local_5 < i)
            {
                carrau.push(((height + (45 * _local_5)) + (5 * _local_5)));
                _local_5++;
            }
            return carrau;
        }

        public function addSlot(s:String, slot:ForgeInventorySlot)
        {
            if (this.slots[s] == null)
            {
                this.slots[s] = [slot];
                return;
            }
            this.slots[s].push(slot);
        }

        public function align():void
        {
            this.alignHorizontal();
            this.alignVertical();
        }

        public function alignVertical():void
        {
            var slot:ForgeInventorySlot;
            var i:int;
            var _local_3:Array;
            var i1:int;
            if (this.slots[ForgeControl.INPUT_TARGET] != null)
            {
                i = this.slots[ForgeControl.INPUT_TARGET].length;
                _local_3 = countY(i);
                for each (slot in this.slots[ForgeControl.INPUT_TARGET])
                {
                    //slot.y = _local_3[i1];
                    i1++;
                }
                i1 = 0;
            }
            if (this.slots[ForgeControl.INPUT_ITEM] != null)
            {
                i = this.slots[ForgeControl.INPUT_ITEM].length;
                _local_3 = countY(i);
                var count:int = 0;
                for each (slot in this.slots[ForgeControl.INPUT_ITEM])
                {
                    if (count < 1)
                    {
                        slot.y = _local_3[3];
                        count++;
                    }
                    else if (count == 1)
                    {
                        slot.y = _local_3[4];
                        count--;
                    }
                    i1++;
                }
                i1 = 0;
            }
            if (this.slots[ForgeControl.OUTPUT] != null)
            {
                i = this.slots[ForgeControl.OUTPUT].length;
                _local_3 = countY(i);
                for each (slot in this.slots[ForgeControl.OUTPUT])
                {
                    slot.y = _local_3[i1];
                    i1++;
                }
            }
        }

        private function alignHorizontal():void
        {
            var slot:ForgeInventorySlot;
            var i:int = -45;
            if (this.slots[ForgeControl.INPUT_TARGET] != null)
            {
                for each (slot in this.slots[ForgeControl.INPUT_TARGET])
                {
                    //slot.x = i;
                }
                //i = (i + 55);
            }
            if (this.slots[ForgeControl.INPUT_ITEM] != null)
            {
                var count:int = 0;
                for each (slot in this.slots[ForgeControl.INPUT_ITEM])
                {
                    if (count < 2)
                    {
                        slot.x = i;
                        count++
                    }
                    else if (count < 4)
                    {
                        slot.x = i + 55;
                        count++
                    }
                    else if (count < 6)
                    {
                        slot.x = i + 110;
                        count++
                    }
                    else if (count < 8)
                    {
                        slot.x = i + 165
                        count++
                    }
                }
                i += 165;
            }
            this.parent.drawArrow((i + 100), ((ForgeControl.HEIGHT - 2) / 2), (ForgeControl.WIDTH - 55), ((ForgeControl.HEIGHT - 2) / 2));
            if (this.slots[ForgeControl.OUTPUT] != null)
            {
                for each (slot in this.slots[ForgeControl.OUTPUT])
                {
                    slot.x = (ForgeControl.WIDTH - 55);
                }
            }
        }

        public function getSlots(s:String):Array
        {
            return this.slots[s];
        }

        public function freeAll():void
        {
            var _local_1:Array;
            var slot:ForgeInventorySlot;
            for each (_local_1 in this.slots)
            {
                for each (slot in _local_1)
                {
                    slot.resetFully();
                }
            }
        }
    }
}