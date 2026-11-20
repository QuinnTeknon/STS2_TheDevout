using BaseLib.Utils;
using Devout.DevoutCode.Content.Character;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Content.Cards;

[Pool(typeof(DevoutCardPool))]
public abstract class DevoutCard : BlankCard {
    public DevoutCard(
        int cost,
        CardType type,
        CardRarity rarity,
        TargetType target) :
        base(cost, type, rarity, target)
    {
        
    }
}