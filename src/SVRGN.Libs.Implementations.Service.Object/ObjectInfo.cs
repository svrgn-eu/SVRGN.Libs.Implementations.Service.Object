using SVRGN.Libs.Contracts.Base;
using SVRGN.Libs.Contracts.Service.Object;
using System;
using System.Collections.Generic;
using System.Text;

namespace SVRGN.Libs.Implementations.Service.Object
{
    internal class ObjectInfo : IObjectInfo
    {
        #region Properties

        public object Data { get; private set; }
        public Type DataType { get; private set; }

        public DateTime CreatedAt { get; private set; }

        #endregion Properties

        #region Construction

        public ObjectInfo(object Data, Type DataType)
        {
            this.CreatedAt = DateTime.Now;
            this.Data = Data;
            this.DataType = DataType;
        }

        #endregion Construction

        #region Methods

        #endregion Methods
    }
}
