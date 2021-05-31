package kabam.rotmg.marketUI.signals {
import org.osflash.signals.Signal;

public class NewPageSignal extends Signal {

    public static var _staticInstance:NewPageSignal;

    public static const forward:int = 1;
    public static const backward:int = 0;

    public function NewPageSignal() {
        super(int);
        _staticInstance = this;
    }
}
}
