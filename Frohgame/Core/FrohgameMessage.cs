using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frohgame.Core {
    /// <summary>
    /// Represents a message from a player/the system to another player
    /// </summary>
    public class FrohgameMessage {
        /// <summary>
        /// content of the message
        /// </summary>
        public string Content {
            get;
            set;
        }

        /// <summary>
        /// title of the message
        /// </summary>
        public string Title {
            get;
            set;
        }
    }
}
