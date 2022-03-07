using Unstable.Actions.GreatSwordSlash;
using Unstable.PlayerActions.Charge;

namespace Unstable.Actions
{
    public interface IActionVisitor
    {
        void Visit(IAction action)
        {
        }

        void Visit(GreatSwordSlashAction greatSwordSlashAction)
        {
            Visit((IAction)greatSwordSlashAction);
        }

        void Visit(ChargeAction chargeAction)
        {
            Visit((IAction)chargeAction);
        }
    }
}