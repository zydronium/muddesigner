﻿//-----------------------------------------------------------------------
// <copyright file="InputMessage.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudEngine.Engine.Core
{
    /// <summary>
    /// Appends an input symbol before the message, to indicate that user input is required.
    /// </summary>
    public class InputMessage : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InputMessage(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// Formats the message.
        /// </summary>
        public string FormatMessage()
        {
            return string.Format("{0}>: ", this.Message);
        }
    }
}
