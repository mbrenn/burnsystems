﻿using System;

namespace BurnSystems.ObjectActivation
{
	/// <summary>
	/// The activation container stores the information how objects
	/// shall be created, how they shall be disposed and if an object
	/// shall be created for each request.
	/// An activation container can be embedded within another activation container. 
	/// The containers requires a name.
	/// </summary>
	public class ActivationContainer
	{
		/// <summary>
		/// Gets or sets the name of the activationcontainer
		/// </summary>
		public string Name
		{
			get;
			set;
		}
		
		/// <summary>
		/// Stores the instance of the inner container
		/// </summary>
		private ActivationContainer innerContainer;
		
		/// <summary>
		/// Initializes a new instance of the ActivationContainer class.
		/// </summary>
		/// <param name="name">Name of the Container</param>
		public ActivationContainer(string name)
		{
			this.Name = name;
		}
		
		/// <summary>
		/// Initializes a new instance of the ActivationContainer class. 
		/// </summary>
		/// <param name="name">Name of the container class</param>
		/// <param name="innerContainer">The innercontainer that shall be 
		/// embedded, if this container does not have a resolution for this 
		/// instance. </param>
		public ActivationContainer(string name, ActivationContainer innerContainer)
		{
			this.Name = name;
			this.innerContainer = innerContainer;
		}
		
		/// <summary>
		/// Converts the object to string.
		/// </summary>
		/// <returns>String converted object</returns>
		public override string ToString()
		{
			return this.Name;
		}
		
		public void Add(CriteriaCatalogue catalogue, Func<object> factory)
		{
		
		}
	}
}
