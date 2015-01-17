﻿// SampSharp
// Copyright (C) 2015 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using SampSharp.GameMode.World;

namespace SampSharp.GameMode.Events
{
    /// <summary>
    ///     Provides data for the <see cref="BaseMode.UnoccupiedVehicleUpdated" /> event.
    /// </summary>
    public class UnoccupiedVehicleEventArgs : PlayerVehicleEventArgs //@todo: change to inherit VehicleEventArgs only.
    {
        public UnoccupiedVehicleEventArgs(int playerid, int vehicleid, int passengerSeat, Vector newPosition,
            Vector newVelocity)
            : base(playerid, vehicleid)
        {
            PassengerSeat = passengerSeat;
            NewPosition = newPosition;
            NewVelocity = newVelocity;
        }

        public int PassengerSeat { get; private set; }

        public Vector NewPosition { get; private set; }

        public Vector NewVelocity { get; private set; }

        /// <summary>
        /// Gets or sets whether to stop the vehicle syncing its position to other players.
        /// </summary>
        public bool PreventPropagation { get; set; }
        //@todo Implement IPropagationPreventable interface where possible
    }
}