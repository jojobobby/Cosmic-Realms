package kabam.rotmg.account
{
    import com.company.assembleegameclient.account.ui.MoneyFrameMediator;
    import flash.display.DisplayObjectContainer;
    import flash.display.LoaderInfo;
    import kabam.rotmg.account.core.BuyCharacterSlotCommand;
    import kabam.rotmg.account.core.commands.PurchaseGoldCommand;
    import kabam.rotmg.account.core.commands.VerifyAgeCommand;
    import kabam.rotmg.account.core.control.IsAccountRegisteredToBuyGoldGuard;
    import kabam.rotmg.account.core.model.OfferModel;
    import kabam.rotmg.account.core.services.GetCharListTask;
    import kabam.rotmg.account.core.services.VerifyAgeTask;
    import kabam.rotmg.account.core.signals.PurchaseGoldSignal;
    import kabam.rotmg.account.core.signals.UpdateAccountInfoSignal;
    import kabam.rotmg.account.core.signals.VerifyAgeSignal;
    import kabam.rotmg.account.core.view.MoneyFrame;
    import kabam.rotmg.account.core.view.PurchaseConfirmationDialog;
    import kabam.rotmg.account.core.view.PurchaseConfirmationMediator;
    import kabam.rotmg.account.transfer.TransferAccountConfig;
    import kabam.rotmg.account.web.WebAccountConfig;
    import kabam.rotmg.core.signals.MoneyFrameEnableCancelSignal;
    import kabam.rotmg.core.signals.TaskErrorSignal;
    import kabam.rotmg.ui.signals.BuyCharacterSlotSignal;
    import org.swiftsuspenders.Injector;
    import robotlegs.bender.extensions.mediatorMap.api.IMediatorMap;
    import robotlegs.bender.extensions.signalCommandMap.api.ISignalCommandMap;
    import robotlegs.bender.framework.api.IConfig;
    import robotlegs.bender.framework.api.IContext;
    import robotlegs.bender.framework.api.ILogger;

    public class AccountConfig implements IConfig
    {
        [Inject]
        public var root:DisplayObjectContainer;
        [Inject]
        public var injector:Injector;
        [Inject]
        public var commandMap:ISignalCommandMap;
        [Inject]
        public var mediatorMap:IMediatorMap;
        [Inject]
        public var context:IContext;
        [Inject]
        public var info:LoaderInfo;
        [Inject]
        public var logger:ILogger;

        public function configure():void
        {
            this.configureCommonFunctionality();
            this.configureAccountSpecificFunctionality();
        }

        private function configureCommonFunctionality():void
        {
            this.injector.map(TaskErrorSignal).asSingleton();
            this.injector.map(UpdateAccountInfoSignal).asSingleton();
            this.injector.map(VerifyAgeTask);
            this.injector.map(GetCharListTask);
            this.injector.map(MoneyFrameEnableCancelSignal).asSingleton();
            this.injector.map(OfferModel).asSingleton();
            this.mediatorMap.map(MoneyFrame).toMediator(MoneyFrameMediator);
            this.mediatorMap.map(PurchaseConfirmationDialog).toMediator(PurchaseConfirmationMediator);
            this.commandMap.map(BuyCharacterSlotSignal).toCommand(BuyCharacterSlotCommand).withGuards(IsAccountRegisteredToBuyGoldGuard);
            this.commandMap.map(PurchaseGoldSignal).toCommand(PurchaseGoldCommand);
            this.commandMap.map(VerifyAgeSignal).toCommand(VerifyAgeCommand);
        }

        private function configureAccountSpecificFunctionality():void
        {
            this.context.configure(WebAccountConfig);
            this.context.configure(TransferAccountConfig);
        }
    }
}