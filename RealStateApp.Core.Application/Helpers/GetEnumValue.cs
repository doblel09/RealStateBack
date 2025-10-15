using RealStateApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Helpers
{
    public static class GetEnumValue
    {
        public static OfferStatus? Compare(string input)
        {
            if (Enum.TryParse<OfferStatus>(input, true, out var result))
            {
                return result;
            }
            return null;
        }
    }
}
