﻿using System;
using SampSharp.EntityComponentSystem.Entities;

namespace TestMode.Ecs.Services
{
    public class VehicleRepository : IVehicleRepository
    {
        public void Foo()
        {
            Console.WriteLine("Foo vehicles");
        }
        public void FooForPlayer(Entity player)
        {
            Console.WriteLine($"Foo vehicles for {player}");
        }
    }
}