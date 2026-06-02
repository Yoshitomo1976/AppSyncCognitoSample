using AppSyncCognitoSample.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Application.Interfaces
{
    public interface IUserInputRepository
    {
        Task SaveAsync(UserInputRecord record, CancellationToken cancellationToken);
    }
}
