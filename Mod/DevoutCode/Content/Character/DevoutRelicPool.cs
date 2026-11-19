using BaseLib.Abstracts;
using Devout.DevoutCode.System.Extensions;
using Godot;

namespace Devout.DevoutCode.Content.Character;

public class DevoutRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Devout.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}