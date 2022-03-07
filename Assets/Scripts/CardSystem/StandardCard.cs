using UnityEngine;
using Unstable;
using Unstable.Entities;

namespace CardSystem
{
    [CreateAssetMenu(menuName = "Create Standard Card", fileName = "Card", order = 0)]
    public class StandardCard : Card
    {
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _illustration;
        [SerializeField] private GameObject _actionPrefab;

        public override string Name => _name;
        public override string Description => _description;
        public override Sprite Illustration => _illustration;

        public override CardUseResult Use(PlayerMasterController playerController)
        {
            var result = new CardUseResult();
            var actionObj = Instantiate(_actionPrefab);
            var action = actionObj.GetComponent<IAction>();
            
            // performs initialization of the action with the visitor pattern
            action.Accept(playerController);
            
            Debug.Assert(action != null, "The game object does not contain any action");
            result.ConsumesCard = true;
            result.Action = action;
            return result;
        }
    }
}