using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 
 * Author(s): Purm & cannap
 * 
 */

namespace Frohgame.Core {
    public class NotEnoughMetalException : Exception {
        public NotEnoughMetalException(string message)
            : base(message) {

        }
    }
    public class NotEnoughCrystalException : Exception {
        public NotEnoughCrystalException(string message)
            : base(message) {

        }
    }
    public class NotEnoughDeuteriumException : Exception {
        public NotEnoughDeuteriumException(string message)
            : base(message) {

        }
    }

    public class LoginFailedException : Exception {
        public LoginFailedException(string message)
            : base(message) {

        }
    }
	
	public class NoCacheDataException : Exception {
        public NoCacheDataException(string message)
            : base(message) {

        }
    }
	
	public class InvalidSessionException : Exception {
        public InvalidSessionException(string message)
            : base(message) {

        }
    }
}
