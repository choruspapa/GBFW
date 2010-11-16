using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;
using System.Configuration;
using System.Resources;
using Formular.Win.Cab.Services;
using Spring.Context;

namespace Eland.GBFW.Win.Shell.Services
{
    public class CultureService : Formular.Win.Cab.Shell.Services.CultureService, IResourceService
    {
        private const string APP_SETTINGS_KEY_RESOURCE_ASSEMBLY = "ResourceAssembly";
        private const string APP_SETTINGS_KEY_RESOURCE_CLASS = "ResourceClass";

        private Assembly _resourceAssembly = null;
        private string _resourceClassName = "MessageResource";
        private ResourceManager _resourceManager = null;

        // _resourceDictionary[AssemblyFullName] => ResourceManager 
        private static Dictionary<string, ResourceManager> _resourceDictionary = null;
        private IApplicationContext springContext = null;

        public CultureService()
        {
            _resourceDictionary = new Dictionary<string, ResourceManager>();

            try
            {
                _resourceAssembly = Assembly.LoadFrom(ConfigurationManager.AppSettings[APP_SETTINGS_KEY_RESOURCE_ASSEMBLY]);
            }
            catch
            {
                _resourceAssembly = Assembly.GetExecutingAssembly();
            }

            try
            {
                _resourceClassName = ConfigurationManager.AppSettings[APP_SETTINGS_KEY_RESOURCE_CLASS];
            }
            catch
            {
                _resourceClassName = "MessageResource";
            }

            _resourceManager = GetResourceManager(_resourceAssembly, _resourceClassName);
            _resourceDictionary.Add(String.Empty, _resourceManager);

            springContext = Spring.Context.Support.ContextRegistry.GetContext();
        }

        #region IResourceService Members

        public object GetResourceObject(string resourceKey)
        {
            return this.GetResourceObject(Assembly.GetCallingAssembly(), resourceKey);
        }

        public string GetResourceString(string resourceKey)
        {
            return this.GetResourceObject(Assembly.GetCallingAssembly(), resourceKey) as string;
        }

        public object GetResourceObject(Assembly moduleAssembly, string resourceKey)
        {
            if (moduleAssembly == null) return this.GetDefaultResourceObject(resourceKey);
            ResourceManager resourceManager = null;

            _resourceDictionary.TryGetValue(moduleAssembly.FullName, out resourceManager);

            if (resourceManager == null)
            {
                resourceManager = GetResourceManager(moduleAssembly, _resourceClassName);
                if (resourceManager == null) resourceManager = _resourceDictionary[String.Empty];
                try
                {
                    _resourceDictionary.Add(moduleAssembly.FullName, resourceManager);
                }
                catch { }
            }

            if (resourceManager != null)
            {
                object resourceObject = resourceManager.GetObject(resourceKey, this.CurrentCultureInfo);
                if (resourceObject != null) return resourceObject;
                return this.GetDefaultResourceObject(resourceKey);
            }
            return this.GetDefaultResourceObject(resourceKey);
        }

        public string GetResourceString(Assembly moduleAssembly, string resourceKey)
        {
            return this.GetResourceObject(moduleAssembly, resourceKey) as string;
        }
        private object GetDefaultResourceObject(string resourceKey)
        {
            if (_resourceAssembly == null) throw new System.IO.FileLoadException();

            if (_resourceManager == null) return springContext.GetResourceObject(resourceKey, this.CurrentCultureInfo);

            object resource = _resourceManager.GetObject(resourceKey, this.CurrentCultureInfo);
            if (resource == null) return springContext.GetResourceObject(resourceKey, this.CurrentCultureInfo);
            return resource;
        }

        /*
        private object GetResourceObjectFromAssembly(Dictionary<string, ResourceManager> languageDictionary, string resourceKey)
        {
            if (languageDictionary == null || languageDictionary.ContainsKey(this.CurrentCultureName) == false || languageDictionary[this.CurrentCultureName] == null)
            {
                return this.GetDefaultResourceObject(resourceKey);
            }

            return languageDictionary[this.CurrentCultureName].GetObject(resourceKey);
        }
         */

        private static ResourceManager GetResourceManager(Assembly assembly, string resourceClassName)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (String.Compare(type.Name, resourceClassName, true) == 0)
                {
                    return type.GetProperty(typeof(ResourceManager).Name).GetValue(null, null) as ResourceManager;
                }
            }
            return null;
        }

        #endregion
    }
}
