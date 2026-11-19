using Devout.DevoutCode.Content.Cards;
using Devout.DevoutCode.Content.Powers;
using Devout.DevoutCode.Content.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devout.DevoutCode.System.Commands;

public class GeneralCmd {
    public static async Task<IReadOnlyList<CardPileAddResult>> CreateCards<T>(PileType pile, CardPilePosition position, decimal number = 1, AbstractModel? source = null, Player? player = null) where T : CardModel {
        player ??= GetPlayerSource(source);
        List<CardModel> cards = CardFactory.GetDistinctForCombat(player, Enumerable.Repeat(0, (int) number).Select(c => ModelDb.Card<T>()).ToList(), (int) number, player?.RunState.Rng.CombatCardGeneration).ToList();
        return await CardPileCmd.AddGeneratedCardsToCombat(cards, pile, player, position);
    }

    public static CardPile? GetPileFromType(Player? player, PileType pile) {
        if (player == null) {
            return pile switch {
                PileType.Deck => player.Deck,
                PileType.Draw => player.PlayerCombatState.DrawPile,
                PileType.Discard => player.PlayerCombatState.DiscardPile,
                PileType.Hand => player.PlayerCombatState.Hand,
                PileType.Exhaust => player.PlayerCombatState.ExhaustPile,
            };
        }
        return null;
    }

    public static Player? GetPlayerSource(AbstractModel source) {
        if (source is CardModel card) {
            return card.Owner;
        }
        if (source is PowerModel power) {
            return power.Owner.Player;
        }
        return null;
    }
    
    public static void ReduceCost(CardModel card, int amount) {
        card.EnergyCost.AddThisTurn(-amount);
    }
    
    public static async Task LoseHP(PlayerChoiceContext context, Creature creature, decimal number, CardModel source, CardPlay? play) {
        await CreatureCmd.Damage(context, creature, number, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, source, play);
    }
    
    public static List<CardModel> GetEnchantedCards<T>(Player player, PileType pile) where T : EnchantmentModel {
        CardPile p = GetPileFromType(player, pile);
        return p.Cards.Where(c => c.Enchantment is T).ToList();
    }

    public static int GetPowerModifier<T>(Creature creature, int divisor) where T : PowerModel {
        return creature.GetPowerAmount<T>() / divisor;
    }
    
    public static int GetDualPowerModifier<T1, T2>(Creature creature, int divisor) where T1 : PowerModel where T2 : PowerModel {
        return (creature.GetPowerAmount<T1>() + creature.GetPowerAmount<T2>()) / divisor;
    }

    public static async Task<CardPileAddResult?> TransformTo<T>(CardModel from) where T : CardModel {
        EnchantmentModel? enchantment = from.Enchantment;
        AfflictionModel? affliction = from.Affliction;
        CardPileAddResult? result = await CardCmd.TransformTo<T>(from, CardPreviewStyle.None);
        CardModel? to = result?.cardAdded;
        if (to != null) {
            if (enchantment != null)
                await EnchantCmd.Enchant(enchantment, to, enchantment.Amount);
            if (affliction != null)
                await CardCmd.Afflict(affliction, to, affliction.Amount);
            return result;
        }
        return null;
    }

    public static CardModel CardFromType(Type? type, out CardModel? card) {
        card = ModelDb.GetByIdOrNull<CardModel>(ModelDb.GetId(type));
        return card;
    }
}