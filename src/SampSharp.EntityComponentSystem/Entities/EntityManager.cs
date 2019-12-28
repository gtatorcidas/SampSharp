﻿// SampSharp
// Copyright 2019 Tim Potze
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
using System.Collections.Generic;

namespace SampSharp.EntityComponentSystem.Entities
{
    /// <summary>
    /// Represents the entity manager.
    /// </summary>
    /// <seealso cref="SampSharp.EntityComponentSystem.Entities.IEntityManager" />
    public class EntityManager : IEntityManager
    {
        private readonly Dictionary<EntityId, Entity> _entities = new Dictionary<EntityId, Entity>();

        public Entity Create(Entity parent, EntityId id)
        {
            var entity = new Entity(parent, id);

            if (_entities.ContainsKey(id))
                throw new EntityCreationException($"Duplicate identity {id} in entities container.");

            _entities.Add(id, entity);

            return entity;
        }

        public Entity Get(EntityId id)
        {
            _entities.TryGetValue(id, out var entity);
            return entity;
        }

        public void Destroy(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.Destroy();
            _entities.Remove(entity.Id);
        }
    }
}