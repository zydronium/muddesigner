﻿//-----------------------------------------------------------------------
// <copyright file="StorageFilenameAttribute.cs" company="AllocateThis!">
//     Copyright (c) AllocateThis! Studio's. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace MudEngine.Engine.Core
{
    /// <summary>
    /// Any property decorated with a FilenameAttribute will have the property value used as the objects
    /// file name when stored using an IPersistedStorage object that stores objects to disk.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class StorageFilenameAttribute : Attribute
    {
        // Empty by design.
    }
}
