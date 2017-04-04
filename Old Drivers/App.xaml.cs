using Windows.UI.Xaml.Data;
using Windows.ApplicationModel.Activation;
using System.Threading.Tasks;
using Template10.Common;

namespace Old_Drivers
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    [Bindable]
    sealed partial class App : BootStrapper
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 从 https://github.com/Windows-XAML/Template10/blob/master/Templates%20(Project)/Blank/App.xaml.cs 复制, 
        /// 文档在 https://github.com/Windows-XAML/Template10/wiki/Bootstrapper 查看
        /// </summary>
        /// <param name="startKind">定制 StartKind 参数来确定原始启动方法是启动还是激活</param>
        /// <param name="args">正在激活该应用程序的原因</param>
        /// <returns>布尔值</returns>
        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}
