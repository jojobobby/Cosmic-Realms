package kabam.rotmg.classes.view {
import com.company.assembleegameclient.constants.ScreenTypes;
import com.company.assembleegameclient.screens.AccountScreen;
import com.company.assembleegameclient.screens.TitleMenuOption;
import com.company.rotmg.graphics.ScreenGraphic;

import flash.display.Shape;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.text.TextFieldAutoSize;

import io.decagames.rotmg.ui.buttons.SliceScalingButton;
import io.decagames.rotmg.ui.defaults.DefaultLabelFormat;

import io.decagames.rotmg.ui.sliceScaling.SliceScalingBitmap;
import io.decagames.rotmg.ui.texture.TextureParser;

import kabam.rotmg.core.StaticInjectorContext;
import kabam.rotmg.core.model.PlayerModel;
import kabam.rotmg.game.view.CreditDisplay;
import kabam.rotmg.text.view.TextFieldDisplayConcrete;
import kabam.rotmg.text.view.stringBuilder.StaticStringBuilder;
import kabam.rotmg.ui.view.SignalWaiter;
import kabam.rotmg.ui.view.TitleView_topBar;
import kabam.rotmg.ui.view.charSelectBG;
import kabam.rotmg.ui.view.components.ScreenBase;

import org.osflash.signals.Signal;
import org.osflash.signals.natives.NativeMappedSignal;

public class CharacterSkinView extends Sprite {

    private const base:ScreenBase = makeScreenBase();
    private const account:AccountScreen = makeAccountScreen();
    private const lines:Shape = makeLines();
    private const creditsDisplay:CreditDisplay = makeCreditDisplay();
    private const graphic:ScreenGraphic = makeScreenGraphic();
    private const list:CharacterSkinListView = makeListView();
    private const detail:ClassDetailView = makeClassDetailView();
    public const play:Signal = new NativeMappedSignal(closeButton, MouseEvent.CLICK);
    public const back:Signal = new NativeMappedSignal(backButton, MouseEvent.CLICK);
    private static var bg:Class = charSelectBG;
    private var buttonsBackground:SliceScalingBitmap;
    private var closeButton:SliceScalingButton;
    private var backButton:SliceScalingButton;
    private var nameText:TextFieldDisplayConcrete;

    private function makeScreenBase():ScreenBase {
        var _local1:ScreenBase = new ScreenBase();
        addChild(new bg);
        addChild(_local1);
        addChild(new TitleView_topBar());
        this.nameText = new TextFieldDisplayConcrete().setSize(22).setColor(0xB3B3B3);
        this.nameText.setBold(true).setAutoSize(TextFieldAutoSize.CENTER);
        this.nameText.setStringBuilder(new StaticStringBuilder("Skins"));
        this.nameText.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
        this.nameText.y = 24;
        this.nameText.x = ((800 - this.nameText.width) / 2);
        addChild(this.nameText);
        return (_local1);
    }

    private function makeAccountScreen():AccountScreen {
        var _local1:AccountScreen = new AccountScreen();
        addChild(_local1);
        return (_local1);
    }

    private function makeCreditDisplay():CreditDisplay {
        var _local1:CreditDisplay;
        _local1 = new CreditDisplay(null, true, true);
        var _local2:PlayerModel = StaticInjectorContext.getInjector().getInstance(PlayerModel);
        if (_local2 != null) {
            _local1.draw(_local2.getCredits(), _local2.getFame(), _local2.getTokens());
        }
        _local1.x = 800;
        _local1.y = 20;
        addChild(_local1);
        return (_local1);
    }

    private function makeLines():Shape {
        var _local1:Shape = new Shape();
        _local1.graphics.clear();
        _local1.graphics.lineStyle(2, 0x545454);
        _local1.graphics.moveTo(0, 105);
        _local1.graphics.lineTo(800, 105);
        _local1.graphics.moveTo(346, 105);
        _local1.graphics.lineTo(346, 526);
        addChild(_local1);
        return (_local1);
    }

    private function makeScreenGraphic():ScreenGraphic {
        var graphic:ScreenGraphic = new ScreenGraphic();
        this.buttonsBackground = TextureParser.instance.getSliceScalingBitmap("UI","popup_header_title",800);
        this.buttonsBackground.y = 502.5;
        addChild(this.buttonsBackground);
        this.closeButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        this.closeButton.x = 350;
        this.closeButton.y = 520;
        this.closeButton.width = 100;
        this.closeButton.setLabel("Play",DefaultLabelFormat.questButtonCompleteLabel);
        this.backButton = new SliceScalingButton(TextureParser.instance.getSliceScalingBitmap("UI","generic_green_button"));
        this.backButton.x = 175;
        this.backButton.y = 520.5;
        this.backButton.width = 100;
        this.backButton.setLabel("Back",DefaultLabelFormat.questButtonCompleteLabel);
        this.backButton.greyScale(true);
        addChild(this.backButton);
        addChild(this.closeButton);
        return graphic;
    }

    private function makeListView():CharacterSkinListView {
        var _local1:CharacterSkinListView;
        _local1 = new CharacterSkinListView();
        _local1.x = 351;
        _local1.y = 110;
        addChild(_local1);
        return (_local1);
    }

    private function makeClassDetailView():ClassDetailView {
        var _local1:ClassDetailView;
        _local1 = new ClassDetailView();
        _local1.x = 5;
        _local1.y = 110;
        addChild(_local1);
        return (_local1);
    }


}
}
