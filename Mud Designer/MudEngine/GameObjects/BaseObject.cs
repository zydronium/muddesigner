﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MudDesigner.MudEngine.Objects
{
    public class BaseObject
    {
        [Category("Object Setup")]
        [RefreshProperties(RefreshProperties.All)] //Required to refresh Filename property in the editors propertygrid
        public string Name
        {
            get;
            set;
        }

        [Category("Object Setup")]
        public string Description
        {
            get;
            set;
        }

        [Browsable(false)]
        public string Script { get; set; }

        [ReadOnly(true)]
        [Category("Object Setup")]
        public string Filename
        {
            //Returns the name of the object + the objects Type as it's extension.
            //Filenames are generated by the class itself, users can not assign it.
            get
            {
                string fileExtension = this.GetType().Name.ToLower();

                return this.Name + "." + fileExtension;
            }
        }

        /// <summary>
        /// Initializes the base object
        /// </summary>
        public BaseObject()
        {
            Script = "";
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
    }
}
