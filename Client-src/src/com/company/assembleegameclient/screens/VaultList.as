package com.company.assembleegameclient.screens
{
    import flash.display.Sprite;
    import kabam.rotmg.core.model.PlayerModel;

    public class VaultList extends Sprite
    {
        private const AMOUNT_IN_ROW:int = 2;
        private const VAULT_OFFSET:int = 5;
        private const PREVIEW_WIDTH:int = 203;
        private const PREVIEW_HEIGHT:int = 107;
        private var playerModel:PlayerModel;
        private var vaults:Vector.<VaultPreview>;

        public function VaultList(playerModel:PlayerModel)
        {
            this.playerModel = playerModel;
            this.vaults = new Vector.<VaultPreview>();
            this.createVaults();
            this.addVaults();
        }

        private function createVaults():void
        {
            var vaultSlots:Vector.<VaultSlot>;
            for each (var vault:Vector.<int> in playerModel.getVaults())
            {
                vaultSlots = new Vector.<VaultSlot>();
                for each (var _local_2:int in vault)
                {
                    vaultSlots.push(new VaultSlot(_local_2));
                }
                vaults.push(new VaultPreview(vaultSlots));
            }
        }

        private function addVaults():void
        {
            var i:int;
            var i2:int;
            var i3:int;
            var preview:VaultPreview;
            var totalVaults:int = vaults.length;
            i = 0;
            while (i < totalVaults)
            {
                i2 = (i % AMOUNT_IN_ROW);
                i3 = int((i / AMOUNT_IN_ROW));
                preview = vaults[i];
                preview.x = (i2 * PREVIEW_WIDTH) + (i2 * VAULT_OFFSET);
                preview.y = ((i3 * PREVIEW_HEIGHT) + (i3 * VAULT_OFFSET)) + 15;
                addChild(preview);
                i++;
            }
        }
    }
}