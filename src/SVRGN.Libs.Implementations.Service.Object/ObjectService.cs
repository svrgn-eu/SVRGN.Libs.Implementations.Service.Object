using SVRGN.Libs.Contracts.Service.Logging;
using SVRGN.Libs.Contracts.Service.Object;
using SVRGN.Libs.Extensions;
using SVRGN.Libs.Implementations.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVRGN.Libs.Implementations.Service.Object
{
    public class ObjectService : IObjectService
    {
        #region Properties

        public bool IsInitialized { get; private set; } = false;

        private List<IObjectInfo> objects;

        private ILogService logService;

        #endregion Properties

        #region Construction

        public ObjectService(ILogService LogService)
        {
            this.logService = LogService;
            this.Initialize();
        }

        #endregion Construction

        #region Methods

        #region Initialize
        public void Initialize()
        {
            if (!this.IsInitialized)
            {
                this.objects = new List<IObjectInfo>();
                this.IsInitialized = true;
            }
        }
        #endregion Initialize

        #region Create
        public T Create<T>(params object[] Parameters) where T : class
        {
            T result = default;
            this.logService.Info("ObjectService", "Create", $"Attempting to create an object of type '{typeof(T)}'");
            result = this.CreateWithoutStoring<T>(Parameters);

            if (result != null)
            {
                this.objects.Add(this.CreateWithoutStoring<ObjectInfo>(result, typeof(T)));

                //if the result implements IInitialize then call it
                /*
                if (result.DoesImplementInterface<IInitialize>())
                {
                    this.logService.Info("ObjectService", "Create", $"Attempting to initialize a new object of type '{typeof(T)}'");
                    ((IInitialize)result).Initialize();
                }
                */
            }

            this.logService.Info("ObjectService", "Create", $"Ended creation of a new object of type '{typeof(T)}'");

            return result;
        }
        #endregion Create

        #region CreateWithTypeName
        public T CreateWithTypeName<T>(string TypeName, params object[] Parameters) where T : class
        {
            T result = default;
            this.logService.Info("ObjectService", "CreateWithTypeName", $"Attempting to create an object of type '{typeof(T)}'");
            result = this.CreateWithoutStoringWithText<T>(TypeName, Parameters);

            if (result != null)
            {
                this.objects.Add(this.CreateWithoutStoring<ObjectInfo>(result, typeof(T)));

                //if the result implements IInitialize then call it
                /*
                if (result.DoesImplementInterface<IInitialize>())
                {
                    this.logService.Info("ObjectService", "CreateWithTypeName", $"Attempting to initialize a new object of type '{typeof(T)}'");
                    ((IInitialize)result).Initialize();
                }
                */
            }

            this.logService.Info("ObjectService", "CreateWithTypeName", $"Ended creation of a new object of type '{typeof(T)}'");

            return result;
        }
        #endregion CreateWithTypeName

        #region CreateWithoutStoring
        public T CreateWithoutStoring<T>(params object[] Parameters) where T : class
        {
            T result = default;
            this.logService.Info("ObjectService", "CreateWithoutStoring", $"Attempting to create an object of type '{typeof(T)}'");
            try
            {
                result = DiContainer.Resolve<T>(Parameters);
                this.logService.Info("ObjectService", "CreateWithoutStoring", $"Successfully created the object");
            }
            catch (Exception ex)
            {
                this.logService.Error("ObjectService", "CreateWithoutStoring", $"Failed when creating the object with message '{ex.Message}'", ex);
            }
            return result;
        }
        #endregion CreateWithoutStoring

        #region CreateWithoutStoringWithText
        public T CreateWithoutStoringWithText<T>(string TypeName, params object[] Parameters) where T : class
        {
            T result = default;
            this.logService.Info("ObjectService", "CreateWithoutStoringWithText", $"Attempting to create an object of type '{typeof(T)}'");
            try
            {
                Type type = TypeName.ToType(true);
                result = (T)DiContainer.Resolve(type, Parameters);
                this.logService.Info("ObjectService", "CreateWithoutStoringWithText", $"Successfully created the object");
            }
            catch (Exception ex)
            {
                this.logService.Error("ObjectService", "CreateWithoutStoringWithText", $"Failed when creating the object", ex);
            }
            return result;
        }
        #endregion CreateWithoutStoringWithText

        #region Count
        public int Count()
        {
            int result = -1;

            if (this.IsInitialized)
            {
                result = this.objects.Count;
            }

            return result;
        }
        #endregion Count

        #endregion Methods

        #region Events

        #endregion Events
    }
}
