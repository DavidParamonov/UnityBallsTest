using System;

public class MovingObjectDTO {
    public float Speed;
    public String Name;
    public float MaxSpeed;

    public float GetSpeedRealValue() {
        return MaxSpeed != 0 ? Speed / MaxSpeed : 0;
    }
}