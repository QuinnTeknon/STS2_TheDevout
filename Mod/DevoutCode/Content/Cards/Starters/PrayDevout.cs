using Devout.DevoutCode.System.Commands;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Devout.DevoutCode.Content.Cards.Starters;

public class PrayDevout() : DevoutCard(1, CardType.Skill, CardRarity.Basic, TargetType.Self) {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new HealVar(5), new HpLossVar(3)];
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    public override async Task Play(PlayerChoiceContext context, CardPlay play) {
        await GeneralCmd.LoseHP(context, Owner.Creature, DynamicVars.HpLoss.BaseValue, this, play);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    }

    protected override void OnUpgrade() => BaseReplayCount += 1;
}