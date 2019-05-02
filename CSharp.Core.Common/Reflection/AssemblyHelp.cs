using CSharp.Core.Common.Resource;
using System;
using System.Linq;
using System.Globalization;
using System.Reflection;
using System.IO;
using System.Reflection.Emit;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 反射靜態協助,透過指定dll名稱方式,以固定的執行路徑下生成物件或以組件完整格式取出所有型別等
    /// </summary>
    public static class AssemblyHelp
    {
        /// <summary>
        /// 載入組件，其名稱指定為完整格式。
        /// </summary>
        /// <param name="assemblyString">組件名稱的完整格式</param>
        /// <returns>包含在這個組件中定義的所有型別。</returns>
        public static Type[] GetTypesByLoad(string assemblyString)
        {
            return Assembly.Load(assemblyString).GetTypes();
        }
        /// <summary>
        /// 載入指定路徑上組件檔案的內容。
        /// </summary>
        /// <param name="path">要載入之檔案的路徑</param>
        /// <returns>包含在這個組件中定義的所有型別。</returns>
        public static Type[] GetTypesByLoadFile(string path)
        {
            return Assembly.LoadFile(path).GetTypes();
        }
        /// <summary>
        /// 載入組件，指定其檔案名稱或路徑。
        /// </summary>
        /// <param name="assemblyFile">檔案的名稱或路徑，包含組件的資訊清單</param>
        /// <returns>包含在這個組件中定義的所有型別。</returns>
        public static Type[] GetTypesByLoadFrom(string assemblyFile)
        {
            return Assembly.LoadFrom(assemblyFile).GetTypes();
        }

        /// <summary>
        /// 載入組件，其名稱指定為完整格式,傳回符合基底類別之類別陣列
        /// </summary>
        /// <param name="assemblyString">組件名稱的完整格式</param>
        /// <param name="baseType">基底類別</param>
        /// <returns>包含在這個組件中定義的所有型別。</returns>
        public static Type[] GetTypesByLoad(string assemblyString, Type baseType)
        {
            Type[] types = GetTypesByLoad(assemblyString);
            return types.Where(x => x.BaseType != null && (x.BaseType == baseType || x.BaseType.BaseType == baseType)).ToArray();
        }

        /// <summary>
        /// 載入指定名稱之Assembly,以固定的執行路徑下
        /// </summary>
        /// <param name="assemblyName">namespace 名稱(Assembly Name)</param>
        /// <param name="className">type 名稱(Class Name)</param>
        /// <param name="dllName">dll的實際檔案名稱,不只定時是 用 >namespace 名稱(Assembly Name)</param>
        /// <returns>反射物件</returns>     
        public static object LoadAssembly(string assemblyName, string className, string dllName = "")
        {
            string path;
            object obj;
            string typeFullName = string.Format("{0}.{1}", assemblyName, className);
            string appPath = FileHelper.GetAppPath();
            appPath = System.Web.HttpContext.Current == null ? appPath.Remove(appPath.LastIndexOf('\\')) : appPath + "bin";

            if (String.IsNullOrEmpty(dllName))
            {
                path = string.Format("{0}\\{1}.dll", appPath, assemblyName);
            }
            else
            {
                path = string.Format("{0}\\{1}.dll", appPath, dllName);
            }
            try
            {
                obj = Assembly.LoadFrom(path).CreateInstance(typeFullName, true); //動態載入DLL  
            }
            catch (Exception)
            {
                throw;
            }
            if (obj == null)
            {
                throw new InvalidOperationException(string.Format(ErrorResource.ReflectionError, path));
            }
            return obj;
            //或
            //Type type = asm.GetType(strTypeName);         //二種寫法均可
            //return Activator.CreateInstance(type, null);

            //若要執行DLL中的Form.Method(param1, param2)
            //MethodInfo mi = type.GetMethod("MethodName");
            //object obj = asm.CreateInstance(type.FullName, true);
            //return (string)mi.Invoke(obj, new string[] { "param1", "param2" });
        }
        /// <summary>
        /// 載入指定名稱之Assembly,以固定的執行路徑下
        /// </summary>
        /// <example>
        /// <code language="cs" title="載入指定名稱之Assembly,以固定的執行路徑下">
        /// ILoadBalance loadBalance = AssemblyHelp.LoadAssembly&lt;ILoadBalance&gt;("CSharp.Services.Proxy.LoadBalancing", "ConsistentHashingAlgorithm", "CSharp.Services.Proxy");
        /// </code>
        /// </example>
        /// <typeparam name="TObj">轉換的TYPE</typeparam>
        /// <param name="assemblyName">namespace 名稱(Assembly Name)</param>
        /// <param name="className">type 名稱(Class Name)</param>
        /// <param name="dllName">dll的實際檔案名稱,不只定時是 用 >namespace 名稱(Assembly Name)</param>
        /// <returns>反射物件已轉型</returns>     
        public static TObj LoadAssembly<TObj>(string assemblyName, string className, string dllName = "")
        {
            return (TObj)LoadAssembly(assemblyName, className, dllName);
        }
        /// <summary>
        /// 使用有參數建構函數指定由泛型參數型別執行個體
        /// </summary>
        /// <typeparam name="TObj">泛型參數型別</typeparam>
        /// <param name="assemblyName">namespace 名稱(Assembly Name)</param>
        /// <param name="className">type 名稱(Class Name)</param>
        /// <param name="dllName">Client.dll 去掉 .dll 傳入 Client</param>         
        /// <param name="args">建構子所需參數</param>
        /// <returns>泛型參數型別執行個體</returns>
        public static TObj LoadAssembly<TObj>(string assemblyName, string className, string dllName, params object[] args)
        {
            string path = System.Web.HttpContext.Current == null ? FileHelper.GetAppPath() : FileHelper.GetAppPath() + "bin\\";
            return (TObj)Activator.CreateInstance(Assembly.LoadFrom(string.Format("{0}{1}.dll", path, dllName)).GetType(string.Format("{0}.{1}", assemblyName, className)), args);
        }
        /// <summary>
        /// 取得Assembly內的資源
        /// </summary>
        /// <param name="assemblyName">Assembly名稱</param>
        /// <param name="resName">資源名稱</param>
        /// <returns></returns>
        public static Stream GetResource(string assemblyName, string resName)
        {
            string dll = Path.Combine(FileHelper.GetAppPath(), string.Format("{0}.dll", assemblyName));
            if (File.Exists(dll))
                return GetResource(Assembly.LoadFrom(dll), resName);
            return null;
        }
        /// <summary>
        ///  取得Assembly內的資源
        /// </summary>
        /// <param name="assembly">組件執行個體</param>
        /// <param name="resName">資源名稱</param>
        /// <returns></returns>
        public static Stream GetResource(Assembly assembly, string resName)
        {
            if (assembly != null && resName.Trim() != "")
                return assembly.GetManifestResourceStream(resName);
            return null;
        }
        /// <summary>
        /// 動態繫結公用Method
        /// </summary>
        /// <param name="MethodNoumenon">instance</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">參數</param>
        /// <param name="MethodTypes">參數類別(參數中有任一個可能為null時必須提供)</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object DynamicPublicMethod(object MethodNoumenon, string MethodName, object[] MethodVariable, Type[] MethodTypes = null)
        {
            BindingFlags MethodFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
            return DynamicExecuteMethod(MethodNoumenon, MethodName, MethodVariable, MethodFlag, MethodTypes);
        }
        /// <summary>
        /// 動態繫結非公用Method
        /// </summary>
        /// <param name="MethodNoumenon">instance</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">參數</param>
        /// <param name="MethodTypes">參數類別(參數中有任一個可能為null時必須提供)</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object DynamicNonPublicMethod(object MethodNoumenon, string MethodName, object[] MethodVariable, Type[] MethodTypes = null)
        {
            BindingFlags MethodFlag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;
            return DynamicExecuteMethod(MethodNoumenon, MethodName, MethodVariable, MethodFlag, MethodTypes);
        }
        /// <summary>
        /// 動態繫結Method
        /// </summary>
        /// <param name="MethodNoumenon">instance</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">參數</param>
        /// <param name="MethodFlag">位元遮罩，由一個或多個 BindingFlags 組成，而這些旗標會指定執行搜尋的方式</param>
        /// <param name="MethodTypes">參數類別(參數中有任一個可能為null時必須提供)</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object DynamicExecuteMethod(object MethodNoumenon, string MethodName, object[] MethodVariable, BindingFlags MethodFlag, Type[] MethodTypes = null)
        {
            //Type[] MethodTypes = null;
            if (MethodTypes == null && MethodVariable != null)
            {
                int n = MethodVariable.Length;
                MethodTypes = new Type[n];
                for (int i = 0; i < n; i++)
                    MethodTypes[i] = MethodVariable[i].GetType();
            }
            Type type = MethodNoumenon.GetType();
            MethodInfo method;
            if (MethodTypes == null)
                method = type.GetMethod(MethodName, MethodFlag);
            else
                method = type.GetMethod(MethodName, MethodFlag, null, MethodTypes, null);
            if (method != null)
                return method.Invoke(MethodNoumenon, MethodFlag, Type.DefaultBinder, MethodVariable, null);
            else
                return null;
        }
        /// <summary>
        /// 根據指定名稱取得變數值
        /// </summary>
        /// <param name="strFindName">變數名稱</param>
        /// <param name="objTarget">對象</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object GetVariable(string strFindName, object objTarget)
        {
            object objTemp;
            try
            {
                objTemp = objTarget.GetType().InvokeMember(strFindName.Trim(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.GetField, null, objTarget, null);
            }
            catch
            {
                objTemp = null;
            }
            return objTemp;
        }
        /// <summary>
        /// 根據指定名稱設定變數值
        /// </summary>
        /// <param name="strFindName">變數名稱</param>
        /// <param name="objTarget">對象</param>
        /// <param name="args">設定值</param>
        /// <returns>表示叫用的成員之傳回值的物件</returns>
        public static object SetVariable(string strFindName, object objTarget, object[] args)
        {
            object objTemp;
            try
            {
                objTemp = objTarget.GetType().InvokeMember(strFindName.Trim(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.SetField, null, objTarget, args);
            }
            catch
            {
                objTemp = null;
            }
            return objTemp;
        }

        /// <summary>
        /// 靜態繫結公用Method
        /// </summary>
        /// <param name="AssemblyName">Assembly Name</param>
        /// <param name="ClassName">Class Name</param>
        /// <param name="MethodName">Method Name</param>
        /// <param name="MethodVariable">object陣列</param>
        /// <returns>傳回值</returns>
        public static object StaticPublicMethod(string AssemblyName, string ClassName, string MethodName, object[] MethodVariable)
        {
            string appPath = FileHelper.GetAppPath();
            appPath = appPath.Remove(appPath.LastIndexOf('\\'));
            AssemblyName = string.Format("{0}\\{1}.dll", appPath, AssemblyName);
            try
            {
                Type[] types = Assembly.LoadFrom(AssemblyName).GetTypes();
                foreach (Type t in types)
                {
                    if (t.Name == ClassName || t.FullName == ClassName)
                    {
                        BindingFlags MethodFlag = BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase;
                        return t.GetMethod(MethodName, MethodFlag).Invoke(null, MethodVariable);
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Exception[] tmp = ex.LoaderExceptions;
                string errmsg = ErrorResource.ReflectionTypeLoadError + Environment.NewLine;
                foreach (Exception e in tmp)
                    errmsg += e.Message + Environment.NewLine;
                throw new InvalidOperationException(errmsg);
            }
            return null;
        }
        /// <summary>
        /// 使用有建構子傳入參數或無參數的方式來生成泛型物件
        /// </summary>
        /// <typeparam name="T">泛型物件</typeparam>         
        /// <param name="args">建構子傳入參數</param>
        /// <returns>泛型物件</returns>
        public static T CreateObject<T>(params object[] args)
        {
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
            {
                if (args.Length == 0) return default(T);//基本型別預設值
                return (T)Convert.ChangeType(args[0], typeof(T));//以第一個參數當值
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T), args);//建構子傳入參數
            }
        }
        /// <summary>
        /// Generates a Dynamic method that can act as a more performant version of 'new T();'
        /// Note: this is a slow operation, hence the bootstrap or JIT generation.
        /// 當資料物件需要大量生成時可使用  70000 次  
        /// </summary>
        /// <typeparam name="TEntity">Data Class to create</typeparam>
        /// <returns>delegate for invocation</returns>
        public static Func<object> DynamicNew<TEntity>() where TEntity : class, new()
        {
            //Type type = typeof(TEntity);
            //ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            //DynamicMethod dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            //ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            //ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            //ilGenerator.Emit(OpCodes.Ret);
            //return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));


            Type type = typeof(TEntity);
            DynamicMethod dynamicMethod = new DynamicMethod("CreateInstance", type, null, type.Module);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Newobj, type.GetConstructor(BindingFlags.Instance
                                            | BindingFlags.Public
                                            | BindingFlags.NonPublic
                                            | BindingFlags.ExactBinding, null, Type.EmptyTypes, null));
            ilGenerator.Emit(OpCodes.Ret);
            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
        }
    }
}
