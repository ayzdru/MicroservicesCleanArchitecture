using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.BlazorWebAssembly.States
{
    public class BasketState
    {
        private int _orderCount;
        public int OrderCount
        {
            get
            {
                return _orderCount;
            }
            set
            {
                _orderCount = value;
                NotifyStateChanged();
            }
        }
        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
