package kabam.rotmg.ui.view {
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.ExperienceBoostTimerPopup;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.objects.Player;
import com.company.assembleegameclient.ui.GameObjectListItem;
import com.company.assembleegameclient.ui.GuildText;
import com.company.assembleegameclient.ui.RankText;
import com.company.assembleegameclient.ui.StatusBar;
import com.company.assembleegameclient.ui.panels.itemgrids.EquippedGrid;

import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;

import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;

import flash.display.Sprite;
import flash.events.Event;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;

import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.LineBuilder;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;

import org.osflash.signals.Signal;
import flash.display.Sprite;
import flash.events.Event;

import kabam.rotmg.text.model.TextKey;

public class StatMetersView extends Sprite {
    private var player:Player;
    private var expBar_:StatusBar;
    private var fameBar_:StatusBar;
    private var OverhpBar_:StatusBar;
    private var hpBar_:StatusBar;
    private var mpBar_:StatusBar;
    private var areTempXpListenersAdded:Boolean;
    private var curXPBoost:int;
    private var expTimer:ExperienceBoostTimerPopup;

    public function StatMetersView() {
        this.expBar_ = new StatusBar(176, 16, 5931045, 0x545454, TextKey.EXP_BAR_LEVEL);
        this.fameBar_ = new StatusBar(176, 16, 0xE25F00, 0x545454, TextKey.CURRENCY_FAME);

        this.hpBar_ = new StatusBar(176, 16, 14693428, 0x545454, TextKey.STATUS_BAR_HEALTH_POINTS);

        this.mpBar_ = new StatusBar(176, 16, 6325472, 0x545454, TextKey.STATUS_BAR_MANA_POINTS);
        this.hpBar_.y = 24;
        this.mpBar_.y = 48;
        this.expBar_.visible = true;
        this.fameBar_.visible = false;
        addChild(this.expBar_);
        addChild(this.fameBar_);
        addChild(this.hpBar_);
        addChild(this.mpBar_);
    }

    public function update(_arg1:Player):void {
        this.expBar_.setLabelText(TextKey.EXP_BAR_LEVEL, {"level": _arg1.level_});
        if (_arg1.level_ != 20) {
            if (this.expTimer) {
                this.expTimer.update(_arg1.xpTimer);
            }
            if (!this.expBar_.visible) {
                this.expBar_.visible = true;
                this.fameBar_.visible = false;
            }
            this.expBar_.draw(_arg1.exp_, _arg1.nextLevelExp_, 0);
        }
        else {
            if (!this.fameBar_.visible) {
                this.fameBar_.visible = true;
                this.expBar_.visible = false;
            }
            this.fameBar_.draw(_arg1.currFame_, _arg1.nextClassQuestFame_, 0);
        }
        if (_arg1.xpTimer) {
            if (!this.areTempXpListenersAdded) {
                this.fameBar_.addEventListener("MULTIPLIER_OVER", this.onExpBarOver);
                this.fameBar_.addEventListener("MULTIPLIER_OUT", this.onExpBarOut);
                this.expBar_.addEventListener("MULTIPLIER_OVER", this.onExpBarOver);
                this.expBar_.addEventListener("MULTIPLIER_OUT", this.onExpBarOut);
                this.areTempXpListenersAdded = true;
            }
        }
        if (this.curXPBoost != _arg1.xpBoost_) {
            this.curXPBoost = _arg1.xpBoost_;
            if (this.curXPBoost) {
                this.fameBar_.showMultiplierText();
                this.expBar_.showMultiplierText();
            }
            else {
                this.fameBar_.hideMultiplierText();
                this.expBar_.hideMultiplierText();
            }
        }
        this.hpBar_.draw(_arg1.hp_, _arg1.maxHP_, _arg1.maxHPBoost_, _arg1.maxHPMax_);
        this.mpBar_.draw(_arg1.mp_, _arg1.maxMP_, _arg1.maxMPBoost_, _arg1.maxMPMax_);
    }

    private function onExpBarOver(_arg1:Event):void {
        addChild((this.expTimer = new ExperienceBoostTimerPopup()));
    }

    private function onExpBarOut(_arg1:Event):void {
        if (((this.expTimer) && (this.expTimer.parent))) {
            removeChild(this.expTimer);
            this.expTimer = null;
        }
    }


}
}
