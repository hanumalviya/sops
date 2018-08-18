using Model.Students;
using System;
using System.Linq;

namespace SOPS.Services.Contracts
{
    public interface IContractUpdater
    {
        void Update(Contract contract);
    }
}
