namespace Beton.Glass
{
    public abstract class WindowBase<TView, TViewModel, TViewData> : IWindow
        where TView : WindowView<TViewModel>
        where TViewModel : WindowViewModel<TViewData>
    {
        public TView View { get; set; }
        public TViewModel ViewModel { get; set; }
        
        public IWindowActivator WindowActivator  { get; set; }
        
        protected virtual void OnConstruct()
        { }
        
        public void Construct(TView view, TViewModel viewModel)
        {
            View = view;
            ViewModel = viewModel;

            OnConstruct();
        }
    }
}