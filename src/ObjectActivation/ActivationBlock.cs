﻿using System;
using System.Collections.Generic;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// The activation block contains the objects that shall
    /// get disposed, the activation block is disposed. 
    /// The disposal of an included object shall only occur, if
    /// the object has been created by this instance.
    /// </summary>
    public class ActivationBlock : IDisposable, IActivates
    {
        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        public string Name
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
        /// Stores the list of all active instances within this collection
        /// </summary>
        private ActiveInstanceCollection activeInstances =
            new ActiveInstanceCollection();

        /// <summary>
        /// Gets the active instances
        /// </summary>
        internal ActiveInstanceCollection ActiveInstances
        {
            get { return this.activeInstances; }
        }

        /// <summary>
        /// Gets the container that contains the information how, when and
        /// in which way to create the object
        /// </summary>
        internal ActivationContainer InnerContainer
        {
            get { return this.innerContainer; }
        }

        /// <summary>
        /// Gets an inner block that may already contain the required object.
        /// </summary>
        internal ActivationBlock InnerBlock
        {
            get { return this.InnerBlock; }
        }

        /// <summary>
        /// Initializes a new instance of the ActivationBlock class.
        /// </summary>
        /// <param name="name">Name of the activation block</param>
        /// <param name="innerContainer">The inner container</param>
        public ActivationBlock(string name, ActivationContainer innerContainer)
        {
            this.Name = name;
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
            this.Name = name;
            this.innerContainer = innerContainer;
            this.innerBlock = innerBlock;
        }

        /// <summary>
        /// Disposes all included object
        /// </summary>
        public void Dispose()
        {
            lock (this.activeInstances)
            {
                foreach (var activeInstance in this.activeInstances)
                {
                    var disposable = activeInstance.Value as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }

                this.activeInstances.Clear();
            }
        }

        /// <summary>
        /// Adds an instance to the block
        /// </summary>
        /// <param name="activeInstance">Instance to be added</param>
        internal void Add(ActiveInstance activeInstance)
        {
            lock (this.activeInstances)
            {
                this.activeInstances.Add(activeInstance);
            }
        }

        /// <summary>
        /// Activates an object by a list of enablers
        /// </summary>
        /// <param name="enablers">Enabler to be used</param>
        /// <returns>Created object</returns>
        public object Get(IEnumerable<IEnabler> enablers)
        {
            foreach (var item in this.innerContainer.ActivationInfos)
            {
                if (item.CriteriaCatalogue.DoesMatch(enablers))
                {
                    return item.FactoryActivationBlock(this, enablers);
                }
            }

            return null;
        }
    }
}