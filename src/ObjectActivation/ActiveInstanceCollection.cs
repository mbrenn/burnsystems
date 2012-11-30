using System;
using System.Collections.Generic;
using System.Linq;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// Stores a collection of active instance
    /// </summary>
    internal class ActiveInstanceCollection : IEnumerable<ActiveInstance>
    {
        /// <summary>
        /// Stores the list of active instances
        /// </summary>
        public List<ActiveInstance> activeInstances =
            new List<ActiveInstance>();

        public void Clear()
        {
            this.activeInstances.Clear();
        }

        public void Add(ActiveInstance instance)
        {
            this.activeInstances.Add(instance);
        }

        public object Find(CriteriaCatalogue catalogue)
        {
            var result =
                this.activeInstances
                    .Where(x => x.Criterias.Guid == catalogue.Guid)
                    .FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            return result.Value;
        }

        public object Find(ActivationInfo info)
        {
            return this.Find(info.CriteriaCatalogue);
        }

        public object Find(BindingHelper helper)
        {
            return this.Find(helper.ActivationInfo);
        }

        public IEnumerator<ActiveInstance> GetEnumerator()
        {
            return this.activeInstances.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.activeInstances.GetEnumerator();
        }

        /// <summary>
        /// Converts the element to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} with {1} instances", typeof(ActiveInstanceCollection).Name, this.activeInstances.Count);
        }
    }
}