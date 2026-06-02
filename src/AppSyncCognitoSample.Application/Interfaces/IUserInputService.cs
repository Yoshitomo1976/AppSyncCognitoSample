using AppSyncCognitoSample.Application.Models;
using AppSyncCognitoSample.Shared.AppSync;
using AppSyncCognitoSample.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSyncCognitoSample.Application.Interfaces
{
    public interface IUserInputService
    {
        Task<SaveUserInputResult> SaveAsync(AppSyncRequest request, CancellationToken cancellationToken);
    }
}
