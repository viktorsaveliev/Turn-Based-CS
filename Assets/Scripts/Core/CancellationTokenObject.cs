using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Echobay
{
    public class CancellationTokenObject : MonoBehaviour
    {
        public CancellationToken Token => this.GetCancellationTokenOnDestroy();
    }
}
