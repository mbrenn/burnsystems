using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.UnitTests.ObjectActivation.Objects
{
    public class Town
    {
        public Rules Rules
        {
            get;
            set;
        }

        public IDatabase Database
        {
            get;
            set;
        }

        public long TownId
        {
            get;
            set;
        }

        public Town(long townId)
        {
            this.TownId = townId;
        }

        public int GetPoints()
        {
            return this.Rules.TownPoints;
        }
    }
}
