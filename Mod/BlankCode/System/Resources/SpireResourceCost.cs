using BaseLib.Abstracts;
using Blank.BlankCode.Content.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Blank.BlankCode.System.Resources;

public class SpireResourceCost(CardModel card, int value, string name, bool X = false) : CustomResourceCost<SpireResource>(card, value, value == BlankCard.X || X) {
    public CardModel model = card;
    public string name = name;
    public int Get() {
        //MainFile.Logger.Info($"{card.GetType().ToString().Split(".").Last()}: Canonical({card.IsCanonical}) CostsX({CostsX})");
        return CostsX ? BlankCard.X : GetWithModifiers(CostModifiers.Local);
    }
}