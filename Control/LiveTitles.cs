using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCool
{
    public class LiveTitles
    {
        //设定版本
//         private static Version TargetedVersion = new Version(7, 10, 8858);
// 
//         //判断是否满足版本要求
//         private static bool isTargetedVersion { get { return Environment.OSVersion.Version >= TargetedVersion; } }

//         private static LiveTitles m_Instance = null;

//         private static LiveTitles() 
//         {
//             if (m_Instance != null)
//             {
//                 m_Instance = new LiveTitles();
//             }
// 
//         }

        public static void SetWideBackgroundImage(string WideImage)
        {
            Version TargetedVersion = new Version(7, 10, 8858);

            //判断是否满足版本要求
            bool isTargetedVersion =  Environment.OSVersion.Version >= TargetedVersion;

            ShellTile appTile = ShellTile.ActiveTiles.First();
            if (appTile != null)
            {
                if (isTargetedVersion)
                {
                    // Get the new FlipTileData type.
                    Type flipTileDataType = Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

                    // Get the ShellTile type so we can call the new version of "Update" that takes the new Tile templates.
                    Type shellTileType = Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

                    // Loop through any existing Tiles that are pinned to Start.
                    //var tileToUpdate = ShellTile.ActiveTiles.First();


                    // Get the constructor for the new FlipTileData class and assign it to our variable to hold the Tile properties.
                    var UpdateTileData = flipTileDataType.GetConstructor(new Type[] { }).Invoke(null);

                    // Set the properties. 
//                     SetProperty(UpdateTileData, "Title", "Main Tile Title");
//                     SetProperty(UpdateTileData, "Count", 0);
//                     SetProperty(UpdateTileData, "BackTitle", "Back Tile Title");
//                     SetProperty(UpdateTileData, "BackContent", "Content For back tile.");
//                     SetProperty(UpdateTileData, "SmallBackgroundImage", new Uri("Windows 8 59.png", UriKind.Relative));
//                     SetProperty(UpdateTileData, "BackgroundImage", new Uri("Windows 8 336.png", UriKind.Relative));
//                     SetProperty(UpdateTileData, "BackBackgroundImage", new Uri("", UriKind.Relative));
                    SetProperty(UpdateTileData, "WideBackgroundImage", new Uri(WideImage, UriKind.Relative));
//                     SetProperty(UpdateTileData, "WideBackBackgroundImage", new Uri("", UriKind.Relative));
//                     SetProperty(UpdateTileData, "WideBackContent", "Content for Wide Back Tile. Lots more text here.");

                    // Invoke the new version of ShellTile.Update.
                    shellTileType.GetMethod("Update").Invoke(appTile, new Object[] { UpdateTileData });
                }
//                 else
//                 {
//                     StandardTileData newTile = new StandardTileData
//                     {
//                         Title = "Main Tile Title",
//                         BackgroundImage = new Uri("Windows 8 173.png", UriKind.Relative),
//                         Count = 0,
//                         BackTitle = "Back Tile Title",
//                         BackBackgroundImage = new Uri("", UriKind.Relative),
//                         BackContent = "Content for back tile."
//                     };
//                     appTile.Update(newTile);
//                 }

            }

        }
        private static void SetProperty(object instance, string name, object value)
        {
            var setMethod = instance.GetType().GetProperty(name).GetSetMethod();
            setMethod.Invoke(instance, new object[] { value });
        }

    }
}
