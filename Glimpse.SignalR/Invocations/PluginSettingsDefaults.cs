// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PluginSettingsDefaults.cs" company="">
//   
// </copyright>
// <summary>
//   The plugin settings defaults.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Glimpse.SignalR.Invocations
{
    using System;
    using System.Collections.Generic;

    using Glimpse.SignalR.Invocations.Plumbing;

    /// <summary>
    /// The plugin settings defaults.
    /// </summary>
    public static class PluginSettingsDefaults
    {
        #region Static Fields

        /// <summary>
        /// The invocations.
        /// </summary>
        private static readonly ICollection<InvocationModel> Invocations = new List<InvocationModel>();

        /// <summary>
        /// The locker.
        /// </summary>
        private static object locker = new object();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get invocations func.
        /// </summary>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        public static Func<IEnumerable<InvocationModel>> GetInvocationsFunc()
        {
            return () => Invocations;
        }

        /// <summary>
        /// The store invocation.
        /// </summary>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        public static Action<InvocationModel> StoreInvocation()
        {
            return invocation =>
                {
                    lock (locker)
                    {
                        Invocations.Add(invocation);
                    }
                };
        }

        #endregion
    }
}