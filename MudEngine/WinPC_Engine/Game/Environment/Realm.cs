﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using MudEngine.Core.Interfaces;
using MudEngine.Game.Characters;
using MudEngine.Game;
using MudEngine.GameScripts;

namespace MudEngine.Game.Environment
{
    public class Realm : Environment
    {
        public new String Filename
        {
            get
            {
                String path = Path.Combine(this.Game.SavePaths.GetPath(DAL.DataTypes.Environments), this.Name, this.Name + "." + this.GetType().Name);
                return path;
            }
        }

        public Realm(StandardGame game, String name, String description) : base(game, name, description)
        {
            this._ZoneCollection = new List<Zone>();
        }

        public Zone CreateZone(String name, String description)
        {
            Zone zone = new Zone(this.Game, name, description, this);
            this._ZoneCollection.Add(zone);
            zone.Realm = this;
            return zone;
        }

        public Zone GetZone(String name)
        {
            var v = from zone in this._ZoneCollection
                    where zone.Name == name
                    select zone;

            return v.First();
        }

        public override bool Save(Boolean ignoreFileWrite)
        {
            if (!base.Save(true))
                return false;

            foreach (Zone zone in this._ZoneCollection)
            {
                zone.Save();
            }

            this.SaveData.Save(this.Filename);

            return true;
        }

        public override void Load(string filename)
        {
            String path = Path.GetDirectoryName(filename);

            if (!Directory.Exists(path))
                return;

            base.Load(filename);

            String[] zones = Directory.GetFiles(Path.Combine(path, "Zones"), "*.zone", SearchOption.AllDirectories);

            foreach (String zone in zones)
            {
                Zone z = new Zone(this.Game, String.Empty, String.Empty, this);
                z.Load(zone);

                if (z != null)
                    this._ZoneCollection.Add(z);
            }
        }

        private List<Zone> _ZoneCollection;
    }
}
