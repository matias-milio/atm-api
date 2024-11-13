using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Core.Contracts
{
    public interface IJwtService
    {
        string GenerateToken(int cardHolderId);
    }
}
