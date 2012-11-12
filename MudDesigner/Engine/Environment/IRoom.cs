﻿using System.Collections.Generic;

using MudDesigner.Engine.Core;
using System;
using System.Collections;
using MudDesigner;
using MudDesigner.Engine.Mobs;
using MudDesigner.Engine.Objects;

namespace MudDesigner.Engine.Environment
{
    public struct Senses
    {
        public string See { get; set; }
        public string Hear { get; set; }
        public string Smell { get; set; }
        public string Feel { get; set; }
        public string Taste { get; set; }
    }

    public interface IRoom : IEnvironment
    {
        IZone Zone { get; set; }

        Senses Sense { get; set; }
        
        List<IPlayer> Occupants { get; set; }
        Dictionary<AvailableTravelDirections, IDoor> Doorways { get; set; }

        List<IItem> Items { get; set; }

        void AddDoorway(AvailableTravelDirections direction, IRoom arrival, bool autoAddReverseDireciton, bool forceOverwrite);
        void RemoveDoorway(AvailableTravelDirections direction, bool autoRemoveReverseDirection);
        IDoor GetDoorway(AvailableTravelDirections direction);
        IDoor[] GetDoorways();

        void AddItem(IItem item);
        void AddDecoration(string decoration);

        IItem[] GetItems();
        IItem GetItem(string itemName);
        string[] GetDecorations();

        void RemoveItem(IItem item);
        void RemoveDecoration(string decoration);

        void ClearItems();
        void ClearDecorations();

        //void QueryQuest(IQuest quest);
    }
}