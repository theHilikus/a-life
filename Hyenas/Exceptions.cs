using System;
using System.Collections.Generic;
using System.Text;

namespace Hyenas {

    /// <summary>
    /// Exception thrown by objects in the world
    /// </summary>
    class WorldObjectException : ApplicationException {
        public WorldObjectException(string error) : base(error) { }
    }
}
