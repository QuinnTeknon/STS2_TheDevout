using BaseLib.Abstracts;
using BaseLib.Utils;
using Blank.BlankCode.Content.Character;

namespace Blank.BlankCode.Content.Potions;

[Pool(typeof(BlankPotionPool))]
public abstract class BlankPotion : CustomPotionModel;