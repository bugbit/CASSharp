﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CASSharp.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CASSharp.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a {0} I can&apos;t convert it to an integer..
        /// </summary>
        internal static string ConvertToIntegerException {
            get {
                return ResourceManager.GetString("ConvertToIntegerException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Function &apos;{0}&apos;: {1}.
        /// </summary>
        internal static string EvalFunctionException {
            get {
                return ResourceManager.GetString("EvalFunctionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a I expected {0} number of parameters.
        /// </summary>
        internal static string NoEqualFnArgsException {
            get {
                return ResourceManager.GetString("NoEqualFnArgsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a I cannot execute the instruction &apos;{0}&apos; because it is not at the beginning of the expression.
        /// </summary>
        internal static string NoExecInsTrNoStartExprException {
            get {
                return ResourceManager.GetString("NoExecInsTrNoStartExprException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a I didn&apos;t expect a {0}..
        /// </summary>
        internal static string NoExpectTokenException {
            get {
                return ResourceManager.GetString("NoExpectTokenException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a {0} Not an integer.
        /// </summary>
        internal static string NoExprIntegerException {
            get {
                return ResourceManager.GetString("NoExprIntegerException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a I expected a maximum of {0} number of parameters.
        /// </summary>
        internal static string NoMaxFnArgsException {
            get {
                return ResourceManager.GetString("NoMaxFnArgsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a I expected as a minimum {0} number of parameters.
        /// </summary>
        internal static string NoMinFnArgsException {
            get {
                return ResourceManager.GetString("NoMinFnArgsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a I don&apos;t recognize &apos;{0}&apos;.
        /// </summary>
        internal static string NoRecognizeStError {
            get {
                return ResourceManager.GetString("NoRecognizeStError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a In position {0} : {1}.
        /// </summary>
        internal static string StError {
            get {
                return ResourceManager.GetString("StError", resourceCulture);
            }
        }
    }
}
