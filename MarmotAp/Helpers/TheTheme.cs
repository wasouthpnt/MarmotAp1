namespace MarmotAp.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case 0:
                    App.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
                //light
                case 1:
                    App.Current.UserAppTheme = AppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = AppTheme.Dark;
                    break;
            }

            var nav = App.Current.MainPage as NavigationPage;

            var e = DependencyService.Get<IEnvironment>();

            if (App.Current.RequestedTheme == AppTheme.Dark)
            {
                e?.SetStatusBarColor(System.Drawing.Color.Black, false);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromRgb(0, 0, 0);
                    nav.BarTextColor = Color.FromRgb(255, 255, 255);
                }
            }
            else
            {
                e?.SetStatusBarColor(System.Drawing.Color.White, true);
                if (nav != null)
                {
                    nav.BarBackgroundColor = Color.FromRgb(255, 255, 255);
                    nav.BarTextColor = Color.FromRgb(0, 0, 0);
                }
            }


        }
    }
}
