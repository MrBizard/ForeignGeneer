using System;
using Godot;

namespace ForeignGeneer.Assets.Scripts.Interface;

public interface BaseUi
{
    public void initialize(Object data);
    public void updateUi(int updateType = 0);
    public void close();

}