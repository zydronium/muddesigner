﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using MudDesigner.MudEngine.Interfaces;

namespace MudDesigner.MudEngine.GameObjects
{
    public class BaseObject : IGameObject
    {
        [Category("Object Setup")]
        [RefreshProperties(RefreshProperties.All)] //Required to refresh Filename property in the editors propertygrid
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                this._Name = value;
                this.Filename = value + "." + this.GetType().Name.ToLower();
            }
        }

        [Category("Object Setup")]
        public string Description
        {
            get;
            set;
        }

        [Browsable(false)]
        public string Script { get; set; }

        [Category("Object Setup")]
        public string Filename
        {
            //Returns the name of the object + the objects Type as it's extension.
            //Filenames are generated by the class itself, users can not assign it.
            get
            {
                return this._Filename;
            }
            set 
            {
                string extension = "." + this.GetType().Name.ToLower();
                if (!value.EndsWith(extension))
                    value += extension;

                this._Filename = value; 
            }
        }

        [Category("Senses")]
        [DefaultValue("You don't smell anything unsual.")]
        public string Smell
        {
            get;
            set;
        }

        [Category("Senses")]
        [DefaultValue("You hear nothing of interest.")]
        public string Listen
        {
            get;
            set;
        }

        [Category("Senses")]
        [DefaultValue("You feel nothing.")]
        public string Feel
        {
            get;
            set;
        }

        [Category("Environment Information")]
        [DefaultValue(false)]
        public bool IsSafe
        {
            get;
            set;
        }

        private string _Filename = "";
        private string _Name = "";
        /// <summary>
        /// Initializes the base object
        /// </summary>
        public BaseObject()
        {
            Script = "";
            _Name = "New " + this.GetType().Name;
            _Filename = _Name + "." + this.GetType().Name.ToLower();

            this.Feel = "You feel nothing.";
            this.Listen = "You hear nothing of interest.";
            this.Smell = "You don't smell anything unsual.";
            this.Name = DefaultName();
            SetupScript();
        }

        private bool ShouldSerializeName()
        {
            return this.Name != DefaultName();
        }

        private void ResetName()
        {
            this.Name = DefaultName();
        }

        private string DefaultName()
        {
            return "New " + this.GetType().Name;
        }

        private void SetupScript()
        {
            //Check if the realm script is empty. If so then generate a standard script for it.
            if (Script == "")
            {
                //Instance a new method helper class
                ManagedScripting.CodeBuilding.MethodSetup method = new ManagedScripting.CodeBuilding.MethodSetup();
                string script = "";
                //Setup our method. All objects inheriting from BaseObject will have the standard
                //methods created for them.
                string[] names = new string[] { "OnCreate", "OnDestroy", "OnEnter", "OnExit" };
                foreach (string name in names)
                {
                    method = new ManagedScripting.CodeBuilding.MethodSetup();
                    method.Name = name;
                    method.ReturnType = "void";
                    method.IsOverride = true;
                    method.Modifier = ManagedScripting.CodeBuilding.ClassGenerator.Modifiers.Public;
                    method.Code = new string[] { "base." + method.Name + "();" };
                    script = script.Insert(Script.Length, method.Create() + "\n");
                }
                Script = script;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnCreate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnEquip()
        {
        }

        public virtual void OnUnequip()
        {
        }

        public virtual void OnMount()
        {
        }

        public virtual void OnDismount()
        {
        }
    }
}
