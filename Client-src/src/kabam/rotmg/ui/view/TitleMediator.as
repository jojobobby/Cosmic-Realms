package kabam.rotmg.ui.view
{
    import com.company.assembleegameclient.mapeditor.MapEditor;
    import com.company.assembleegameclient.screens.ServersScreen;
    import com.company.assembleegameclient.ui.language.LanguageOptionOverlay;
    import flash.events.Event;
    import flash.external.ExternalInterface;
    import flash.net.URLRequest;
    import flash.net.URLRequestMethod;
    import flash.net.URLVariables;
    import flash.net.navigateToURL;
    import flash.system.Capabilities;
    import kabam.rotmg.account.core.Account;
    import kabam.rotmg.account.core.signals.OpenAccountInfoSignal;
    import kabam.rotmg.account.web.view.WebLoginDialog;
    import kabam.rotmg.application.DynamicSettings;
    import kabam.rotmg.application.api.ApplicationSetup;
    import kabam.rotmg.core.model.PlayerModel;
    import kabam.rotmg.core.signals.SetScreenSignal;
    import kabam.rotmg.core.signals.SetScreenWithValidDataSignal;
    import kabam.rotmg.core.view.Layers;
    import kabam.rotmg.dialogs.control.OpenDialogSignal;
    import kabam.rotmg.editor.view.TextureView;
    import kabam.rotmg.legends.view.LegendsView;
    import kabam.rotmg.ui.model.EnvironmentData;
    import kabam.rotmg.ui.signals.EnterGameSignal;
    import robotlegs.bender.bundles.mvcs.Mediator;
    import robotlegs.bender.framework.api.ILogger;

    public class TitleMediator extends Mediator
    {
        private static var supportCalledBefore:Boolean = false;
        [Inject]
        public var view:TitleView;
        [Inject]
        public var account:Account;
        [Inject]
        public var playerModel:PlayerModel;
        [Inject]
        public var setScreen:SetScreenSignal;
        [Inject]
        public var setScreenWithValidData:SetScreenWithValidDataSignal;
        [Inject]
        public var enterGame:EnterGameSignal;
        [Inject]
        public var openAccountInfo:OpenAccountInfoSignal;
        [Inject]
        public var openDialog:OpenDialogSignal;
        [Inject]
        public var setup:ApplicationSetup;
        [Inject]
        public var layers:Layers;
        [Inject]
        public var logger:ILogger;

        override public function initialize():void
        {
            this.view.initialize(this.makeEnvironmentData());
            this.view.playClicked.add(this.handleIntentionToPlay);
            this.view.serversClicked.add(this.showServersScreen);
            this.view.accountClicked.add(this.handleIntentionToReviewAccount);
            this.view.legendsClicked.add(this.showLegendsScreen);
            this.view.editorClicked.add(this.showMapEditor);
        }

        private function makeEnvironmentData():EnvironmentData
        {
            var _local1:EnvironmentData = new EnvironmentData();
            _local1.isDesktop = (Capabilities.playerType == "Desktop");
            _local1.canMapEdit = ((this.playerModel.isAdmin()) || (this.playerModel.mapEditor()));
            _local1.buildLabel = this.setup.getBuildLabel();
            return (_local1);
        }

        override public function destroy():void
        {
            this.view.playClicked.remove(this.handleIntentionToPlay);
            this.view.serversClicked.remove(this.showServersScreen);
            this.view.accountClicked.remove(this.handleIntentionToReviewAccount);
            this.view.legendsClicked.remove(this.showLegendsScreen);
            this.view.editorClicked.remove(this.showMapEditor);
        }

        private function handleIntentionToPlay():void
        {
            if (!account.isRegistered())
            {
                this.openDialog.dispatch(new WebLoginDialog());
            }
            else
            {
                this.enterGame.dispatch();
            }
        }

        private function showServersScreen():void
        {
            this.setScreen.dispatch(new ServersScreen());
        }

        private function handleIntentionToReviewAccount():void
        {
            this.openAccountInfo.dispatch(false);
        }

        private function showLegendsScreen():void
        {
            this.setScreen.dispatch(new LegendsView());
        }

        private function showMapEditor():void
        {
            this.setScreen.dispatch(new MapEditor());
        }

        private function showTextureEditor():void
        {
            this.setScreen.dispatch(new TextureView());
        }

        private function attemptToCloseClient():void
        {
            dispatch(new Event("APP_CLOSE_EVENT"));
        }
    }
}