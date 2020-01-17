﻿// SampSharp
// Copyright 2020 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using SampSharp.Entities.SAMP.Components;
using SampSharp.Entities.SAMP.Definitions;
using SampSharp.Entities.SAMP.NativeComponents;

namespace SampSharp.Entities.SAMP
{
    /// <summary>
    /// Represents a service for adding entities to and control the SA:MP world.
    /// </summary>
    public class WorldService : IWorldService
    {
        /// <summary>
        /// The type of a world entity.
        /// </summary>
        public static readonly Guid WorldType = new Guid("DD999ED7-9935-4F66-9CDC-E77484AF6BB8");

        /// <summary>
        /// The entity identifier used by the world entity.
        /// </summary>
        public static readonly EntityId WorldId = new EntityId(WorldType, 0);

        // TODO: Handling of invalid IDs (SA-MP entity limits)

        private readonly IEntityManager _entityManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldService" /> class.
        /// </summary>
        public WorldService(IEntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        private Entity World => _entityManager.Get(WorldId);
        private NativeWorld Native => World.GetComponent<NativeWorld>();

        /// <inheritdoc />
        public float Gravity
        {
            get => Native.GetGravity();
            set
            {
                if (value < -50 || value > 50)
                    throw new ArgumentOutOfRangeException(nameof(value), value,
                        "Value must be between -50.0 and 50.0.");

                Native.SetGravity(value);
            }
        }

        /// <inheritdoc />
        public Actor CreateActor(int modelId, Vector3 position, float rotation)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>()
                .CreateActor(modelId, position.X, position.Y, position.Z, rotation);

            if (id == NativeActor.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(world, SampEntities.GetActorId(id));

            entity.AddComponent<NativeActor>();
            return entity.AddComponent<Actor>();
        }

        /// <inheritdoc />
        public Vehicle CreateVehicle(VehicleModelType type, Vector3 position, float rotation, int color1, int color2,
            int respawnDelay = -1, bool addSiren = false)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>().CreateVehicle((int) type, position.X, position.Y, position.Z,
                rotation, color1, color2, respawnDelay, addSiren);
            
            if (id == NativeVehicle.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(world, SampEntities.GetVehicleId(id));

            entity.AddComponent<NativeVehicle>();
            return entity.AddComponent<Vehicle>();
        }

        /// <inheritdoc />
        public Vehicle CreateStaticVehicle(VehicleModelType type, Vector3 position, float rotation, int color1,
            int color2, int respawnDelay = -1, bool addSiren = false)
        {
            var world = World;

            var id = respawnDelay == -1 && !addSiren
                ? world.GetComponent<NativeWorld>().AddStaticVehicle((int) type, position.X, position.Y, position.Z,
                    rotation, color1, color2)
                : world.GetComponent<NativeWorld>().AddStaticVehicleEx((int) type, position.X, position.Y, position.Z,
                    rotation, color1, color2, respawnDelay, addSiren);
            
            if (id == NativeVehicle.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(world, SampEntities.GetVehicleId(id));

            entity.AddComponent<NativeVehicle>();
            return entity.AddComponent<Vehicle>();
        }

        /// <inheritdoc />
        public GangZone CreateGangZone(float minX, float minY, float maxX, float maxY)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>().GangZoneCreate(minX, minY, maxX, maxY);

            var entity = _entityManager.Create(world, SampEntities.GetGangZoneId(id));
            
            if (id == NativeGangZone.InvalidId)
                throw new EntityCreationException();

            entity.AddComponent<NativeGangZone>();
            return entity.AddComponent<GangZone>(minX, minY, maxX, maxY);
        }

        /// <inheritdoc />
        public Pickup CreatePickup(int model, int type, Vector3 position, int virtualWorld = -1)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>()
                .CreatePickup(model, type, position.X, position.Y, position.Z, virtualWorld);
            
            if (id == NativePickup.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(world, SampEntities.GetPickupId(id));

            entity.AddComponent<NativePickup>();
            return entity.AddComponent<Pickup>(virtualWorld, model, type, position);
        }

        /// <inheritdoc />
        public bool AddStaticPickup(int model, int type, Vector3 position, int virtualWorld = -1)
        {
            var world = World;

            return world.GetComponent<NativeWorld>()
                .AddStaticPickup(model, type, position.X, position.Y, position.Z, virtualWorld);
        }

        /// <inheritdoc />
        public GlobalObject CreateObject(int modelId, Vector3 position, Vector3 rotation, float drawDistance)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>().CreateObject(modelId, position.X, position.Y, position.Z,
                rotation.X, rotation.Y, rotation.Z, drawDistance);
            
            if (id == NativeObject.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(world, SampEntities.GetObjectId(id));

            entity.AddComponent<NativeObject>();
            return entity.AddComponent<GlobalObject>(drawDistance);
        }

        /// <inheritdoc />
        public PlayerObject CreatePlayerObject(Entity player, int modelId, Vector3 position, Vector3 rotation,
            float drawDistance)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            if (!player.IsOfType(SampEntities.PlayerType))
                throw new InvalidEntityArgumentException(nameof(player), SampEntities.PlayerType);

            var world = World;

            var id = world.GetComponent<NativeWorld>().CreatePlayerObject(player.Id, modelId, position.X, position.Y,
                position.Z, rotation.X, rotation.Y, rotation.Z, drawDistance);
            
            if (id == NativePlayerObject.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(player, SampEntities.GetPlayerObjectId(player.Id, id));

            entity.AddComponent<NativePlayerObject>();
            return entity.AddComponent<PlayerObject>(drawDistance);
        }

        /// <inheritdoc />
        public TextLabel CreateTextLabel(string text, Color color, Vector3 position, float drawDistance,
            int virtualWorld = 0, bool testLos = true)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>().Create3DTextLabel(text, color, position.X, position.Y,
                position.Z, drawDistance, virtualWorld, testLos);
            
            if (id == NativeTextLabel.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(world, SampEntities.GetTextLabelId(id));

            entity.AddComponent<NativeTextLabel>();
            return entity.AddComponent<TextLabel>(text, color, position, drawDistance, virtualWorld, testLos);
        }

        /// <inheritdoc />
        public PlayerTextLabel CreatePlayerTextLabel(Entity player, string text, Color color, Vector3 position,
            float drawDistance, bool testLos = true, Entity attachedTo = null)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            
            if (!player.IsOfType(SampEntities.PlayerType))
                throw new InvalidEntityArgumentException(nameof(player), SampEntities.PlayerType);

            var attachPlayer = NativePlayer.InvalidId;
            var attachVehicle = NativeVehicle.InvalidId;

            if (attachedTo != null)
            {
                if (!player.IsOfAnyType(SampEntities.PlayerType, SampEntities.VehicleType))
                    throw new InvalidEntityArgumentException(nameof(player), SampEntities.PlayerType,
                        SampEntities.VehicleType);

                if (attachedTo.IsOfType(SampEntities.PlayerType))
                    attachPlayer = attachedTo.Id;
                else
                    attachVehicle = attachedTo.Id;
            }

            var world = World;

            var id = world.GetComponent<NativeWorld>().CreatePlayer3DTextLabel(player.Id, text, color, position.X,
                position.Y, position.Z, drawDistance, attachPlayer, attachVehicle, testLos);
            
            if (id == NativePlayerTextLabel.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(player, SampEntities.GetPlayerTextLabelId(player.Id, id));

            entity.AddComponent<NativePlayerTextLabel>();
            return entity.AddComponent<PlayerTextLabel>(text, color, position, drawDistance, testLos, attachedTo);
        }

        /// <inheritdoc />
        public TextDraw CreateTextDraw(Vector2 position, string text)
        {
            var world = World;

            var id = world.GetComponent<NativeWorld>().TextDrawCreate(position.X, position.Y, string.IsNullOrEmpty(text) ? "_" : text);
            
            if (id == NativeTextDraw.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(null, SampEntities.GetTextDrawId(id));

            entity.AddComponent<NativeTextDraw>();
            return entity.AddComponent<TextDraw>(position, text);
        }
        
        /// <inheritdoc />
        public PlayerTextDraw CreatePlayerTextDraw(Entity player, Vector2 position, string text)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));
            
            if (!player.IsOfType(SampEntities.PlayerType))
                throw new InvalidEntityArgumentException(nameof(player), SampEntities.PlayerType);

            var world = World;

            var id = world.GetComponent<NativeWorld>().CreatePlayerTextDraw(player.Id, position.X, position.Y, string.IsNullOrEmpty(text) ? "_" : text);
            
            if (id == NativePlayerTextDraw.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(player, SampEntities.GetPlayerTextDrawId(player.Id, id));

            entity.AddComponent<NativePlayerTextDraw>();
            return entity.AddComponent<PlayerTextDraw>(position, text);
        }
        
        /// <inheritdoc />
        public Menu CreateMenu(string title, Vector2 position, float col0Width, float? col1Width = null)
        {
            var world = World;

            var columns = col1Width != null ? 2 : 1;

            var id = world.GetComponent<NativeWorld>().CreateMenu(title, columns, position.X, position.Y, col0Width, col1Width ?? 0.0f);
            
            if (id == NativeMenu.InvalidId)
                throw new EntityCreationException();

            var entity = _entityManager.Create(null, SampEntities.GetMenuId(id));

            entity.AddComponent<NativeMenu>();
            return entity.AddComponent<Menu>(title, columns, position, col0Width, col1Width ?? 0.0f);
        }

        /// <inheritdoc />
        public void SetObjectsDefaultCameraCollision(bool disable)
        {
            Native.SetObjectsDefaultCameraCol(disable);
        }

        /// <inheritdoc />
        public void SendClientMessage(Color color, string message)
        {
            Native.SendClientMessageToAll(color, message);
        }

        /// <inheritdoc />
        public void SendClientMessage(Color color, string messageFormat, params object[] args)
        {
            SendClientMessage(color, string.Format(messageFormat, args));
        }

        /// <inheritdoc />
        public void SendClientMessage(string message)
        {
            SendClientMessage(Color.White, message);
        }

        /// <inheritdoc />
        public void SendClientMessage(string messageFormat, params object[] args)
        {
            SendClientMessage(Color.White, string.Format(messageFormat, args));
        }

        /// <inheritdoc />
        public void SendPlayerMessageToPlayer(Entity sender, string message)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));

            if (!sender.IsOfType(SampEntities.PlayerType))
                throw new InvalidEntityArgumentException(nameof(sender), SampEntities.PlayerType);

            Native.SendPlayerMessageToAll(sender.Id, message);
        }

        /// <inheritdoc />
        public void SendDeathMessage(Entity killer, Entity player, Weapon weapon)
        {
            if (killer != null && !killer.IsOfType(SampEntities.PlayerType))
                throw new InvalidEntityArgumentException(nameof(killer), SampEntities.PlayerType);

            if (player == null)
                throw new ArgumentNullException(nameof(player));
            
            if (!player.IsOfType(SampEntities.PlayerType))
                throw new InvalidEntityArgumentException(nameof(player), SampEntities.PlayerType);

            Native.SendDeathMessage(killer?.Id ?? NativePlayer.InvalidId, player.Id, (int) weapon);
        }

        /// <inheritdoc />
        public void GameText(string text, int time, int style)
        {
            Native.GameTextForAll(text, time, style);
        }

        /// <inheritdoc />
        public void CreateExplosion(Vector3 position, ExplosionType type, float radius)
        {
            Native.CreateExplosion(position.X, position.Y, position.Z, (int) type, radius);
        }

        /// <inheritdoc />
        public void SetWeather(int weather)
        {
            Native.SetWeather(weather);
        }
    }
}