using System;
using System.Linq;
using Model.Students;

namespace SOPS.Services.Contracts
{
    public interface IContractProvider
    {
        Contract GetContract(int id);
    }
}
