using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Blank.BlankCode.Content.Character;
using Blank.BlankCode.System.Extensions;
using Blank.BlankCode.System.Resources;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Blank.BlankCode.Content.Cards;

[Pool(typeof(BlankCardPool))]
public abstract class BlankCard : CustomCardModel {
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    
    private Random random = new Random();
    protected override bool HasEnergyCostX => cost == X;
    public const int X = -2;
    private int options = 3;
    public int cost;

    public BlankCard(
        int cost,
        CardType type,
        CardRarity rarity,
        TargetType target) :
        base(cost, type, rarity, target)
    {
        this.cost = cost;
    }
    
    public virtual Task Play(PlayerChoiceContext context, CardPlay play) => Task.CompletedTask;
    
    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play) {
        if (TargetType.In(TargetType.AnyEnemy, TargetType.AnyAlly, TargetType.Osty))
            ArgumentNullException.ThrowIfNull(play.Target);
        await Task.Run(async() => {
            MainFile.Logger.Info($"TRYING TO PLAY '{GetType().ToString().Split(".").Last()}' FROM '{Pile}'...");
            await Play(context, play);
        });
        base.OnPlay(context, play);
    }
    
    protected bool CheckResource(string name, SpireResource res, Player chara) => res.getName() == name && chara == Owner;
    public virtual Task AfterResourceModified(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceSpent(SpireResource res, int amount, Player spender, object? info = null) => Task.CompletedTask;
    public virtual Task AfterResourceGained(SpireResource res, int amount, Player gainer, object? info = null) => Task.CompletedTask;
    public virtual Task AfterCardEnchanted(PlayerChoiceContext context, CardModel card) => Task.CompletedTask;
}