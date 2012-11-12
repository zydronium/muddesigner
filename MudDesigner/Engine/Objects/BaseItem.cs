﻿/* BaseItem
 * Product: Mud Designer Engine
 * Copyright (c) 2012 AllocateThis! Studios. All rights reserved.
 * http://MudDesigner.Codeplex.com
 *  
 * File Description: The base class for all game items.
 */
//Microsoft .NET using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//AllocateThis! Mud Designer using statements
using MudDesigner.Engine.Mobs;
using MudDesigner.Engine.Core;

namespace MudDesigner.Engine.Objects
{
    /// <summary>
    /// The base class for all game items.
    /// </summary>
    public abstract class BaseItem : GameObject,  IItem
    {
        /// <summary>
        /// Gets or Sets how much this item weighs
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// Gets or Sets how much health this item has
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Gets or Sets if this item is indestructible
        /// </summary>
        public bool Indestructible { get; set; }

        /// <summary>
        /// Gets or Sets if this item is usable only by admins or not.
        /// </summary>
        public bool IsAdminOnly { get; set; }

        /// <summary>
        /// Takes all of this Game Objects properties and copies them over to the argument object.
        /// </summary>
        /// <param name="copyTo">The object that will have it's properties replaced with the calling Object</param>
        public override void CopyState(ref dynamic copyTo)
        {
            if (copyTo is IItem)
            {
                Scripting.ScriptObject newObject = new Scripting.ScriptObject(copyTo);

                newObject.SetProperty("Weight", Weight, null);
                newObject.SetProperty("Health", Health, null);
                newObject.SetProperty("Indestructible", Indestructible, null);
                newObject.SetProperty("IsAdminOnly", IsAdminOnly, null);
            }

            base.CopyState(ref copyTo);
        }

        /// <summary>
        /// Inspects the item and tells the player what is seen.
        /// </summary>
        /// <param name="player">The player that wants to inspect this item</param>
        public virtual void Inspect(IPlayer player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Repairs the item
        /// </summary>
        /// <param name="healder">The character that is performing the heal</param>
        /// <param name="amount">The healing amount</param>
        public void Repair(IMob healder, int amount)
        {
        }

        /// <summary>
        /// Deals damage to this item
        /// </summary>
        /// <param name="dealer">The character that is dealing the damage</param>
        /// <param name="amount">The amount of damage</param>
        public void Damage(IMob dealer, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
