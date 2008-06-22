//-----------------------------------------------------------------------
// <copyright file="Executor.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Runtime.Remoting;

namespace BurnSystems.Test
{
    /// <summary>
    /// Diese Klasse sucht sich die Tests in einer Assembly zusammen
    /// und führt diese bei Bedarf aus. 
    /// </summary>
    public class Executor
    {
        /// <summary>
        /// Liste von Methoden
        /// </summary>
        List<MethodBase> _Methods =
            new List<MethodBase>();

        /// <summary>
        /// Liste von Methoden
        /// </summary>
        public List<MethodBase> Methods
        {
            get { return _Methods; }
            set { _Methods = value; }
        }

        /// <summary>
        /// Lädt alle Tests ein, die in der Assembly gefunden worden sind.
        /// </summary>
        /// <param name="oAssembly">Assembly mit den Tests</param>
        public void LoadAssembly(Assembly oAssembly)
        {
            foreach (var oType in oAssembly.GetTypes())
            {
                var aoAttributes = oType.GetCustomAttributes(
                    typeof(TestClassAttribute), false);

                if (aoAttributes.Length == 0)
                {
                    // Kein Attribut, kein Test
                    continue;
                }
                if (!oType.IsPublic)
                {
                    throw new InvalidOperationException(
                        LocalizationBS.Executor_NoPublicClass);
                }

                foreach (var oMethod in oType.GetMethods())
                {
                    var aoMethodAttributes = oMethod.GetCustomAttributes(
                        typeof(TestMethodAttribute), false);
                    if (aoMethodAttributes.Length == 0)
                    {
                        continue;
                    }                 

                    _Methods.Add(oMethod);
                }
            }
        }

        /// <summary>
        /// Lädt eine Assembly aus einem Pfad. Wenn diese Assembly
        /// bereits eingeladen wurde, so wird sie wiederverwendet. 
        /// </summary>
        /// <param name="strPath">Pfad zur Assembly</param>
        public void LoadAssembly(String strPath)
        {
            strPath = Path.GetFullPath(strPath);

            // Suche zuerst die schon geladenen Assemblies durch
            foreach (var oAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var strAssemblyPath = Path.GetFullPath(
                    oAssembly.Location);
                if (strAssemblyPath == strPath)
                {
                    LoadAssembly(oAssembly);
                    return;
                }
            }

            // Nun lade die Assembly
            var oLoadedAssembly = Assembly.LoadFile(strPath);
            LoadAssembly(oLoadedAssembly);
        }

        /// <summary>
        /// Ruft die Methode auf und gibt die Testergebnisse zurück
        /// </summary>
        /// <param name="oMethod">Methode</param>
        public static Result TestMethod(MethodBase oMethod)
        {
            if (!oMethod.IsPublic)
            {
                // Nur öffentliche Methoden
                throw new InvalidOperationException(
                    LocalizationBS.Executor_NoPublicMethod);
            }
            if (oMethod.GetParameters().Length != 0)
            {
                // Nur Methoden ohne einem Parameter können aufgerufen
                // werden
                throw new InvalidOperationException(
                    LocalizationBS.Executor_InvalidParameterCount);
            }

            // Erzeugt die AppDomain
            var oAppDomain = AppDomain.CreateDomain("TESTDomain",
                null, Environment.CurrentDirectory, Environment.CurrentDirectory, false);

            var oObject = 
                oAppDomain.CreateInstanceFrom(oMethod.ReflectedType.Assembly.Location, 
                oMethod.ReflectedType.FullName);
            
            // Startet nun den Test
            var oResult = new Result();
            Stopwatch oWatch = new Stopwatch();
            try
            {
                oWatch.Start();

                oAppDomain.DoCallBack(new Helper(oMethod, oObject).Invoke);

                oResult.Failed = false;
            }
            catch (Exception exc)
            {
                oResult.Failed = true;
                oResult.Exception = exc;
            }
            finally
            {
                oWatch.Stop();
                oResult.Duration = oWatch.Elapsed;
            }

            // Entlädt die AppDomain
            AppDomain.Unload(oAppDomain);
            return oResult;
        }

        [Serializable()]
        class Helper
        {
            /// <summary>
            /// Methode, die aufgerufen werden soll
            /// </summary>
            MethodBase _MethodBase;

            /// <summary>
            /// Das Handle, das genutzt werden soll
            /// </summary>
            ObjectHandle _Handle;

            public Helper(MethodBase oBase, ObjectHandle oHandle)
            {
                _MethodBase = oBase;
                _Handle = oHandle;
            }

            public void Invoke()
            {                
                _MethodBase.Invoke(_Handle.Unwrap(), null);
            }
        }

    }
}
