using Unstable.Actions.GreatSwordSlash;
using Unstable.PlayerActions.Charge;

namespace Unstable.Actions
{
    public interface IActionVisitor<out TRet, TTag>
    {
        TRet Visit(IAction action)
        {
            return default;
        }

        TRet Visit(GreatSwordSlashAction greatSwordSlashAction)
        {
            return Visit((IAction)greatSwordSlashAction);
        }

        TRet Visit(ChargeAction chargeAction)
        {
            return Visit((IAction)chargeAction);
        }
    }
}