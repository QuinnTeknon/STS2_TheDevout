using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace Devout.DevoutCode.Content.Relics.Starters;

public class MilkOfCreation : BloodOfTheGods {
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<RegenPower>(5m)];
}