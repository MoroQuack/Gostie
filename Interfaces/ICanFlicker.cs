using Godot;
using System;

public interface ICanFlicker
{
	void TurnOn();
	void TurnOff();
	bool IsOn();
}
