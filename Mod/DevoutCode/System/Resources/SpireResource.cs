using BaseLib.Abstracts;
using BaseLib.Utils;
using Devout.DevoutCode.Content.Cards;
using Devout.DevoutCode.Content.Powers;
using Devout.DevoutCode.Content.Relics;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Devout.DevoutCode.System.Resources;

public class SpireResource : CustomResource {
    private string name;
    private Player player;
    
    public string getName() => name;

    public SpireResource() : base("resource") {}
    public SpireResource(string title, (Vector2, int, int) transform) : base(title) {
        name = title;
    }

    public async Task Set(Decimal amount, object? info = null) {
        if (CombatManager.Instance.IsEnding || player?.PlayerCombatState == null)
            return;
        Amount = (int) amount;
        await updateCardVisuals(player);
        await AfterModified(player.Creature.CombatState, (int) amount, player, info);
    }
    
    public async Task Gain(Decimal amount, object? info = null) {
        if (CombatManager.Instance.IsEnding || player?.PlayerCombatState == null)
            return;
        Amount += (int) amount;
        await updateCardVisuals(player);
        await AfterGained(player.Creature.CombatState, (int) amount, player, info);
    }
    
    public async Task Lose(Decimal amount, object? info = null) {
        if (CombatManager.Instance.IsEnding || player?.PlayerCombatState == null)
            return;
        Amount -= (int) amount;
        await updateCardVisuals(player);
        await AfterLost(player.Creature.CombatState, (int) amount, player, info);
    }

    private Task AfterModified(ICombatState state, int amount, Player modifier, object? info = null) {
        foreach (AbstractModel model in state.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card) {
                    card.AfterResourceModified(this, amount, modifier, info);
                } else if (model is DevoutPower power) {
                    power.AfterResourceModified(this, amount, modifier, info);
                } else if (model is DevoutRelic relic) {
                    relic.AfterResourceModified(this, amount, modifier, info);
                }
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }
    
    private async Task AfterGained(ICombatState state, int amount, Player gainer, object? info = null) {
        foreach (AbstractModel model in state.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card) {
                    await card.AfterResourceGained(this, amount, gainer, info);
                } else if (model is DevoutPower power) {
                    await power.AfterResourceGained(this, amount, gainer, info);
                } else if (model is DevoutRelic relic) {
                    await relic.AfterResourceGained(this, amount, gainer, info);
                }
                model.InvokeExecutionFinished();
            }
        }
    }
    
    private Task AfterLost(ICombatState state, int amount, Player spender, object? info = null) {
        foreach (AbstractModel model in state.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card) {
                    card.AfterResourceSpent(this, amount, spender, info);
                } else if (model is DevoutPower power) {
                    power.AfterResourceSpent(this, amount, spender, info);
                } else if (model is DevoutRelic relic) {
                    relic.AfterResourceSpent(this, amount, spender, info);
                }
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }

    public async Task updateCardVisuals(Player player) {
        MainFile.Logger.Info($"Updating cards [{String.Join(", ", player.PlayerCombatState.Hand.Cards)}] in hand.");
        foreach (CardModel card in player.PlayerCombatState.Hand.Cards) {
            await Task.Run(() => {
                var instance = NCard.FindOnTable(card);
                if (instance is { Model: not null }) {
                    instance.UpdateVisuals(PileType.Hand, CardPreviewMode.Normal);
                    MainFile.Logger.Info($" - Updated {card} in hand.");
                }
            });
        }
    }

    public void SetPlayer(Creature creature) {
        player = creature.Player ?? null;
    }
}