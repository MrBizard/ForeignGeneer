using System;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface;

public interface BaseUi
{
    public abstract void initialize(Object data);
    public abstract void updateUi(int updateType = 0);
    public abstract void close();

}