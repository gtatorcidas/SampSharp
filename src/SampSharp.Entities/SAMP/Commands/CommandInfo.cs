﻿// SampSharp
// Copyright 2022 Tim Potze
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

namespace SampSharp.Entities.SAMP.Commands;

/// <summary>Provides information about a command.</summary>
public class CommandInfo
{
    /// <summary>Initializes a new instance of the <see cref="CommandInfo" /> class.</summary>
    /// <param name="name">The name of the command.</param>
    /// <param name="parameters">The parameters of the command.</param>
    public CommandInfo(string name, CommandParameterInfo[] parameters)
    {
        Name = name;
        Parameters = parameters;
    }

    /// <summary>Gets the name of this command.</summary>
    public string Name { get; }

    /// <summary>Gets the parameters of this command.</summary>
    public CommandParameterInfo[] Parameters { get; }
}