using Expresso.ViewModel;

namespace Expresso.view
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();

            BindingContext = new RregistroProductoViewModel();

        }

        
    }

}
