using Devout.DevoutCode.Content.Cards;
using Devout.DevoutCode.Content.Powers;
using Devout.DevoutCode.Content.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace Devout.DevoutCode.System.Commands;

public class EnchantCmd {
    public static async Task Enchant(EnchantmentModel? enchantment, CardModel card, decimal amount = 1) {
        CardCmd.Enchant(enchantment.IsMutable ? enchantment : enchantment.ToMutable(), card, amount);
        NCardEnchantVfx nCardEnchantVfx = NCardEnchantVfx.Create(card);
        if (nCardEnchantVfx != null) {
            NRun.Instance?.GlobalUi.CardPreviewContainer.AddChildSafely(nCardEnchantVfx);
        }
        await AfterEnchanted(new ThrowingPlayerChoiceContext(), card);
    }
    
    private static Task AfterEnchanted(PlayerChoiceContext context, CardModel target) {
        ICombatState state = target.CombatState;
        foreach (AbstractModel model in state.IterateHookListeners()) {
            if (model is DevoutCard or DevoutPower or DevoutRelic) {
                if (model is DevoutCard card) {
                    card.AfterCardEnchanted(context, target);
                } else if (model is DevoutPower power) {
                    power.AfterCardEnchanted(context, target);
                } else if (model is DevoutRelic relic) {
                    relic.AfterCardEnchanted(context, target);
                }
                model.InvokeExecutionFinished();
            }
        }
        return Task.CompletedTask;
    }
    
    public static async Task CreateEnchantedCards<T>(EnchantmentModel? enchantment, PileType pile, CardPilePosition position, decimal number = 1, AbstractModel? source = null, Player? player = null) where T : CardModel {
        IReadOnlyList<CardPileAddResult> result = await GeneralCmd.CreateCards<T>(pile, position, number, source, player);
        List<CardModel> cards = result.Select(c => c.cardAdded).ToList();
        if (enchantment != null) {
            foreach (CardModel item in cards) {
                Enchant(enchantment.ToMutable(), item);
            }
        } else {
            CardCmd.PreviewCardPileAdd(result);
        }
    }
    
    public static async Task EnchantCardsInPile(PlayerChoiceContext context, EnchantmentModel enchantment, PileType pile, decimal number, AbstractModel source, bool random = false) {
        Player? player = GeneralCmd.GetPlayerSource(source);
        IEnumerable<CardModel> cards = [];
        switch (random) {
            case true:
                cards = GeneralCmd.GetPileFromType(player, pile).Cards.TakeRandom((int) number, player.RunState.Rng.CombatCardGeneration);
                break;
            case false:
                CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, (int) number);
                switch (pile) {
                    case PileType.Hand:
                        cards = await CardSelectCmd.FromHand(context, player, prefs, null, source);
                        break;
                    case PileType.Draw: case PileType.Discard:
                        cards = await CardSelectCmd.FromSimpleGrid(context, (
                            from c in pile.GetPile(player).Cards
                            orderby c.Rarity, c.Id
                            select c).ToList(), player, new CardSelectorPrefs(CardSelectorPrefs.EnchantSelectionPrompt, (int) number));
                        break;
                }
                break;
        }
        foreach (CardModel item in cards) {
            Enchant(enchantment.ToMutable(), item);
        }
    }
}