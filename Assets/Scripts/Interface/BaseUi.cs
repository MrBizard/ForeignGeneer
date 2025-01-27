using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface;

public abstract partial class BaseUi : Control
{
    public abstract void initialize(Node data);
    public abstract void updateUi();

}