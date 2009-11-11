﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MUDEngine.Objects.Environment
{
    public class Room : BaseObject
    {
        [Category("Room Information")]
        [DefaultValue("You don't smell anything unsual.")]
        public string Smell
        {
            get;
            set;
        }

        [Category("Room Information")]
        [DefaultValue("You hear nothing of interest.")]
        public string Listen
        {
            get;
            set;
        }

        [Category("Room Information")]
        [DefaultValue("You feel nothing.")]
        public string Feel
        {
            get;
            set;
        }

        /*public Stats StatDrain
        {
            get;
            set;
        }*/

        [Category("Room Information")]
        [DefaultValue(0)]
        public int StatDrainAmount
        {
            get;
            set;
        }

        [Category("Room Information")]
        [DefaultValue(false)]
        public bool IsSafeRoom
        {
            get;
            set;
        }

        public Room()
        {
            this.Feel = "You feel nothing.";
            this.Listen = "You hear nothing of interest.";
            this.Smell = "You don't smell anything unsual.";
            this.StatDrainAmount = 0;
        }
    }
}
