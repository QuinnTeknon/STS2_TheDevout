using BaseLib.Abstracts;
using Blank.BlankCode.System.Extensions;
using Godot;

namespace Blank.BlankCode.Content.Character;

public class BlankRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Blank.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}