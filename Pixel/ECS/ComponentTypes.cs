using System;

namespace Pixel.ECS
{
    [Flags]
    public enum ComponentType : ulong
    {
        None                    = 0b0,
        CameraComponent         = 0b1,
        DbgBoundingBox          = 0b10,
        DestinationComponent    = 0b100,
        DialogComponent         = 0b1000,
        DrawableComponent       = 0b10000,
        InputComponent          = 0b100000,
        MouseComponent          = 0b1000000,
        NetworkComponent        = 0b10000000,
        ParticleComponent       = 0b100000000,
        ParticleEmitterComponent= 0b1000000000,
        PositionComponent       = 0b10000000000,
        SpeedComponent          = 0b100000000000,
        TextComponent           = 0b1000000000000,
        TransformComponent      = 0b10000000000000,
        VelocityComponent       = 0b100000000000000,
        Unused1                 = 0b1000000000000000,
        Unused2                 = 0b10000000000000000,
        Unused3                 = 0b100000000000000000,
        Unused4                 = 0b1000000000000000000,
        Unused5                 = 0b10000000000000000000,
        Unused6                 = 0b100000000000000000000,
        Unused7                 = 0b1000000000000000000000,
        Unused8                 = 0b10000000000000000000000,
        Unused9                 = 0b100000000000000000000000,
        Unused10                = 0b1000000000000000000000000,
        Unused11                = 0b10000000000000000000000000,
        Unused12                = 0b100000000000000000000000000,
        Unused13                = 0b1000000000000000000000000000,
        Unused14                = 0b10000000000000000000000000000,
        Unused15                = 0b100000000000000000000000000000,
    } 
}