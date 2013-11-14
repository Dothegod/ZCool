using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ZCool
{
    class AssociationUriMapper : UriMapperBase
    {
        private string tempUri;

        public override Uri MapUri(Uri uri)
        {
            tempUri = System.Net.HttpUtility.UrlDecode(uri.ToString());
            if (tempUri.Contains("zcool:"))
            {
                return new Uri("/MainPage.xaml", UriKind.Relative);
            }

            // Otherwise perform normal launch.
            return uri;
        }
    }
}
