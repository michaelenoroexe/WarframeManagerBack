using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IPasswordEqualityComparer
    {
        /// <summary>
        /// Compare user inputed password with hashed in db.
        /// </summary>
        /// <returns>Are password equal or not.</returns>
        public bool Equals(string x, string y);
    }
}
