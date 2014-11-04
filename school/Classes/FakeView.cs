// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FakeView.cs" company="SemanticArchitecture">
//   http://www.SemanticArchitecture.net pkalkie@gmail.com
// </copyright>
// <summary>
//   Defines the FakeView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using school.Models;
namespace school.Classes
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    public class FakeView : IView
    {
        #region IView Members

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}