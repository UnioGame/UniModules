using UnityEngine;

namespace UniModules.UniGame.Tools.ExposedReferences.Runtime
{
    using UnityEngine.Playables;

    public class ExposedPlayableDirector : PlayableDirector, IExposedPropertyTable
    {
        #region IExposedPropertyTable API

        /// <summary>
        ///   <para>Assigns a value for an ExposedReference.</para>
        /// </summary>
        /// <param name="id">Identifier of the ExposedReference.</param>
        /// <param name="value">The value to assigned to the ExposedReference.</param>
        public new void SetReferenceValue(PropertyName id, Object value)
        {
            base.SetReferenceValue(id, value);
        }

        public new Object GetReferenceValue(PropertyName id, out bool idValid)
        {
            return base.GetReferenceValue(id,out idValid);
        }

        /// <summary>
        ///   <para>Remove a value for the given reference.</para>
        /// </summary>
        /// <param name="id">Identifier of the ExposedReference.</param>
        public new void ClearReferenceValue(PropertyName id)
        {
            base.ClearReferenceValue(id);
        }
        
        #endregion
        
    }
}
