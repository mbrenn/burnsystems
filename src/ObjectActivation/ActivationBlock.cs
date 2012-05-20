﻿using System;
using System.Collections.Generic;

namespace BurnSystems.ObjectActivation
{
	/// <summary>
	/// The activation block contains the objects that shall
	/// get disposed, the activation block is disposed. 
	/// The disposal of an included object shall only occur, if
	/// the object has been created by this instance. .
	/// </summary>
	public class ActivationBlock : IDisposable
	{
		/// <summary>
		/// Gets or sets the name of the block
		/// </summary>
		public string name
		{
			get;
			set;
		}
		
		/// <summary>
		/// Stores the container that contains the information how, when and
		/// in which way to create the object
		/// </summary>
		private ActivationContainer innerContainer;
		
		/// <summary>
		/// Contains an inner block that may already contain the required object.
		/// </summary>
		private ActivationBlock innerBlock;
		
		/// <summary>
		/// Stores the list of activation infos
		/// </summary>
		private List<ActivationInfo> activationInfos = new List<ActivationInfo>();
		
		/// <summary>
		/// Gets the list of activation information. 
		/// All objects within this list will be disposed, if 
		/// Activation Block will be disposed
		/// </summary>
		internal List<ActivationInfo> ActivationInfos
		{
			get{ return this.activationInfos; }
		}
		
		/// <summary>
		/// Initializes a new instance of the ActivationBlock class.
		/// </summary>
		/// <param name="name">Name of the activation block</param>
		/// <param name="innerContainer">The inner container</param>
		public ActivationBlock(string name, ActivationContainer innerContainer)
		{
			this.name = name;
			this.innerContainer = innerContainer;
		}
		
		/// <summary>
		/// Initializes a new instance of the ActivationBlock class.
		/// </summary>
		/// <param name="name">Name of the object to be created</param>
		/// <param name="innerContainer">The container to be used</param>
		/// <param name="innerBlock">The inner block containing the necessary
		/// information</param>
		public ActivationBlock(
			string name, 
			ActivationContainer innerContainer, 
			ActivationBlock innerBlock)
		{
			this.name = name;
			this.innerContainer = innerContainer;
			this.innerBlock = innerBlock;
		}
		
		/// <summary>
		/// Disposes all included object
		/// </summary>
		public void Dispose()
		{			
			foreach(var value in this.activationInfos)
			{
				var disposable = value as IDisposable;
				if(disposable != null)
				{
					disposable.Dispose();
				}
			}
			
			this.activationInfos.Clear();
		}
		
		/// <summary>
		/// Activates an object by a list of enablers
		/// </summary>
		/// <param name="enablers">Enabler to be used</param>
		/// <returns>Created object</returns>
		public object Get(IEnumerable<IEnabler> enablers)
		{
			foreach (var item in this.ActivationInfos)
			{
				if(item.CriteriaCatalogue.DoesMatch(enablers))
				{
					return item.FactoryActivationBlock(this);
				}
			}
			
			return null;
		}
	}
}
