using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Velocity.ViewModels;

namespace Velocity.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class DebugPage : Page
{
    public DebugViewModel ViewModel
    {
        get;
    }

    public DebugPage()
    {
        ViewModel = App.GetService<DebugViewModel>();
        InitializeComponent();
    }

}
