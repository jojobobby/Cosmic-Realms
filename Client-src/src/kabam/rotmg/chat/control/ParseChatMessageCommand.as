package kabam.rotmg.chat.control {
import com.company.assembleegameclient.map.Map;
import com.company.assembleegameclient.parameters.Parameters;

import flash.display.StageScaleMode;
import flash.events.Event;

import kabam.rotmg.account.core.Account;
import kabam.rotmg.appengine.api.AppEngineClient;
import kabam.rotmg.build.api.BuildData;
import kabam.rotmg.chat.model.ChatMessage;
import kabam.rotmg.dailyLogin.model.DailyLoginModel;
import kabam.rotmg.game.signals.AddTextLineSignal;
import kabam.rotmg.memMarket.MemMarket;
import kabam.rotmg.memMarket.MemMarketPanel;
import kabam.rotmg.potionStorage.PotionStorage;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.ui.model.HUDModel;

public class ParseChatMessageCommand {

    [Inject]
    public var data:String;
    [Inject]
    public var hudModel:HUDModel;
    [Inject]
    public var addTextLine:AddTextLineSignal;
    [Inject]
    public var client:AppEngineClient;
    [Inject]
    public var account:Account;
    [Inject]
    public var buildData:BuildData;
    [Inject]
    public var dailyLoginModel:DailyLoginModel;

    private function mscaleCommand() : Boolean
    {
        var command:Array;
        command = this.data.match("^/mscale (\\d*\\.*\\d+)");
        if (command != null)
        {
            if (command[1] < 0.5)
            {
                command[1] = 0.5;
            }
            if (command[1] > 5)
            {
                command[1] = 5;
            }
            Parameters.data_.mscale = command[1];
            Parameters.save();
            Parameters.root.dispatchEvent(new Event(Event.RESIZE));
            this.addTextLine.dispatch(ChatMessage.make(Parameters.SERVER_CHAT_NAME, "Map Scale: " + command[1]));
            return true
        }
        return false;
    }

    private function chatScaleCommand() : Boolean
    {
        var command:Array;
        command = this.data.match("^/cscale (\\d*\\.*\\d+)");
        if (command != null)
        {
            if (command[1] < 0)
            {
                command[1] = 0;
            }
            if (command[1] > 2)
            {
                command[1] = 2;
            }
            Parameters.data_.ScaleChat = command[1];
            Parameters.save();
            this.addTextLine.dispatch(ChatMessage.make(Parameters.SERVER_CHAT_NAME, "Chat Scale: " + command[1]));
            return true
        }
        return false;
    }

    public function execute():void {
        if(this.mscaleCommand())
        {
            return;
        }
        if(this.chatScaleCommand())
        {
            return;
        }
        switch (this.data) {
            case "/help":
                this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME, TextKey.HELP_COMMAND));
                return;
            case "/fs":
            case "/fullscreen":
                Parameters.root.stage.scaleMode = StageScaleMode.NO_SCALE;
                Parameters.data_.stageScale = StageScaleMode.NO_SCALE;
                Parameters.save();
                Parameters.root.dispatchEvent(new Event(Event.RESIZE));
                return;
            case "/scaleui":
                Parameters.data_["uiscale"] = !Parameters.data_["uiscale"];
                Parameters.save();
                Parameters.root.dispatchEvent(new Event(Event.RESIZE));
                return;
            case "/mscale":
                this.addTextLine.dispatch(ChatMessage.make("*Help*","Map Scale: " + Parameters.data_["mscale"] + " - Usage: /mscale <decimal number in the range [0.5; 5]>"));
                return;
            case "/cscale":
                this.addTextLine.dispatch(ChatMessage.make("*Help*","Chat Scale: " + (Parameters.data_["ScaleChat"]*100) + "% - Usage: /cscale <decimal number in the range [0; 2]>"));
                return;
            case "/marketplace":
                if (this.hudModel.gameSprite.isNexus_ || this.hudModel.gameSprite.map.name_ == "Vault" || this.hudModel.gameSprite.map.name_ == "Cloth Bazaar")
                {
                    this.hudModel.gameSprite.gsc_.gs_.mui_.setEnablePlayerInput(false);
                    this.hudModel.gameSprite.gsc_.gs_.addChild(new MemMarket(this.hudModel.gameSprite.gsc_.gs_.mui_.gs_));
                }
                return;
            case "/potionstorage":
                if (this.hudModel.gameSprite.isNexus_ || this.hudModel.gameSprite.map.name_ == "Vault" || this.hudModel.gameSprite.map.name_ == "Cloth Bazaar")
                {
                    this.hudModel.gameSprite.gsc_.gs_.mui_.setEnablePlayerInput(false);
                    this.hudModel.gameSprite.gsc_.gs_.addChild(new PotionStorage(this.hudModel.gameSprite.gsc_.gs_.mui_.gs_));
                }
                return;
            default:
                this.hudModel.gameSprite.gsc_.playerText(this.data);
        }
    }


}
}
