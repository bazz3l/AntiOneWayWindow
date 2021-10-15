using System.Collections.Generic;

namespace Oxide.Plugins
{
    [Info("Anti One Way Window", "Bazz3l", "1.0.1")]
    [Description("Stops items being placed on window embrasures or bars this was creating one way windows.")]
    public class AntiOneWayWindow : CovalencePlugin
    {
        #region Fields

        private readonly HashSet<string> _blockedPrefabs = new HashSet<string>
        {
            "shutter.metal.embrasure.a",
            "shutter.metal.embrasure.b",
            "wall.window.bars.metal",
            "wall.window.bars.wood"
        };

        #endregion
        
        #region Lang

        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                { "PlacementBlocked", "<color=#e2e2e2><color=#ffc55c>Placement blocked</color>: trying to place siren light on <color=#ffc55c>{0}</color>.</color>" },
            }, this);
        }
        
        string Lang(string key, string id = null, params object[] args) => string.Format(lang.GetMessage(key, this, id), args);

        #endregion
        
        #region Oxide
        
        object CanBuild(Planner planner, Construction prefab, Construction.Target target)
        {
            BasePlayer player = planner.GetOwnerPlayer();
            
            if (prefab.deployable == null || target.entity == null)
                return null;

            if (!prefab.deployable.fullName.Contains("sirenlight.deployed.prefab"))
                return null;
            
            if (!_blockedPrefabs.Contains(target.entity.ShortPrefabName)) 
                return null;

            if (player == null)
                player.ChatMessage(Lang("PlacementBlocked", player.UserIDString, target.entity.ShortPrefabName));

            return false;
        }

        #endregion
    }
}