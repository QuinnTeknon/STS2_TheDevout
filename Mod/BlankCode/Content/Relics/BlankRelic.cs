using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Blank.BlankCode.Content.Character;
using Blank.BlankCode.System.Extensions;
using Blank.BlankCode.System.Resources;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Blank.BlankCode.Content.Relics;

/// <summary>
/// This is the base class for your mod's relics, which is set up to load the relic's images from your mod's resources.
/// When creating a relic, right click the Relics folder and create a new file with the Custom Relic template.
/// This will generate a class that extends this one.
/// You can also just create the class manually; just make sure to inherit from this class.
///
/// The [Pool] annotation marks this relic as being tied to your specific character. Inheriting from this class means
/// that your relics don't need to invidually say which pool they should be in.
/// </summary>
[Pool(typeof(BlankRelicPool))]
public abstract class BlankRelic : CustomRelicModel {
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
    
    protected bool CheckResource(string name, SpireResource res, Player chara) => res.getName() == name && chara == Owner;
    public virtual Task AfterResourceModified(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceSpent(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) => Task.CompletedTask;
    public virtual Task AfterCardEnchanted(PlayerChoiceContext context, CardModel card) => Task.CompletedTask;
}