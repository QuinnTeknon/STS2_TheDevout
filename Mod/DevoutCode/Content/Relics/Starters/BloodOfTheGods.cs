using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Content.Relics.Starters;

public class BloodOfTheGods : DevoutRelic {
    public override RelicRarity Rarity => RelicRarity.Starter;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RegenPower>(3m)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [..base.ExtraHoverTips, HoverTipFactory.FromPower<RegenPower>()];
    
    public override async Task BeforeCombatStart() {
        await PowerCmd.Apply<RegenPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["RegenPower"].BaseValue, Owner.Creature, null);
    }

    public override RelicModel GetUpgradeReplacement() => ModelDb.Relic<MilkOfCreation>();
}