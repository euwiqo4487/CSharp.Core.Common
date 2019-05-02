using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 可序列化物件複製(無內建複製機制)
    /// </summary>
    public static class SerializableObject
    {
        /// <summary>
        /// 深層複製
        /// </summary>
        /// <typeparam name="T">物件類別</typeparam>
        /// <param name="obj">物件個體</param>
        /// <returns></returns>
        public static T Clone<T>(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}
