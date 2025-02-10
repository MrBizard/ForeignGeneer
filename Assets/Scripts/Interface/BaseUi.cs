using System;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface;

public abstract partial class BaseUi : Control
{
    public abstract void initialize(Object data);
    public abstract void updateUi();
    public abstract void close();

}