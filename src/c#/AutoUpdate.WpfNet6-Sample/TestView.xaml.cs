using System.Windows;

namespace AutoUpdate.WpfNet6_Sample
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : Window
    {
        public TestView()
        {
            InitializeComponent();
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            //var context = new BaseContext();
            //context.TargetPath = "123";
            //context.SourcePath = "123";
            //IPipelineBuilder builder = new PipelineBuilder<BaseContext>(context).
            //    UseMiddleware<MD5Middleware>().
            //    UseMiddleware<CompressMiddleware>().
            //    UseMiddleware<ConfigMiddleware>().
            //    UseMiddleware<PatchMiddleware>();
            //builder.Launch();
        }
    }
}