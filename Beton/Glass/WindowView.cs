using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Beton.Glass
{
    public abstract class WindowView<TViewModel> : View<TViewModel>, IWindowView
        where TViewModel : IWindowViewModel
    {
        [SerializeField] protected List<Button> _closeButtons;
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        public CanvasGroup CanvasGroup => _canvasGroup;

        internal override void OnInitInternal(TViewModel viewModel)
        {
            Bind(_closeButtons, viewModel.CloseWindow);
        }
    }
}