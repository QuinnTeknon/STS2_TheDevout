using BaseLib.Abstracts;
using BaseLib.Utils;
using Devout.DevoutCode.Content.Character;

namespace Devout.DevoutCode.Content.Potions;

[Pool(typeof(DevoutPotionPool))]
public abstract class DevoutPotion : CustomPotionModel;