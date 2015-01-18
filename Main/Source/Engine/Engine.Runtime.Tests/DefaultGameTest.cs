// <copyright file="DefaultGameTest.cs">Copyright ©  2014</copyright>

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mud.Engine.Runtime.Game;

namespace Mud.Engine.Runtime.Game
{
    [TestClass]
    [PexClass(typeof(DefaultGame))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class DefaultGameTest
    {
    }
}
