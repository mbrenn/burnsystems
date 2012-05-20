
using System;

namespace BurnSystems.ObjectActivation
{
	/// <summary>
	/// Description of ActivationInfo.
	/// </summary>
	internal class ActivationInfo
	{
		/// <summary>
		/// Gets the information about the activated object
		/// </summary>
		public object ActivatedObject
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Gets the information whether the activatedobject has
		/// been created by this activation.
		/// </summary>
		public bool HasBeenCreated
		{
			get;
			private set;
		}
		
		/// <summary>
		/// Initializes a new instance of the ActivationInfo class
		/// </summary>
		/// <param name="activatedObject">The object that has been created</param>
		/// <param name="hasBeenCreated">true, if the object has been created
		/// by this call.</param>
		public ActivationInfo(object activatedObject, bool hasBeenCreated)
		{
			if (activatedObject == null)
				throw new ArgumentNullException("activatedObject");
			
			this.ActivatedObject = activatedObject;
			this.HasBeenCreated = hasBeenCreated;
		}
	}
}
