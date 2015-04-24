using System;
using System.IO;
using System.Windows.Forms;

using PhotoAlbum;

namespace MyAlbumExplorer
{
    internal class PhotoNode : TreeNode, IRefreshableNode
    {
        private Photo _photo;
        public Photo Photograph { get { return _photo; } }

        public PhotoNode(Photo photo)
            : base()
        {
            if (photo == null)
                throw new ArgumentNullException("photo");

            _photo = photo;
            Text = photo.Caption;
            ImageKey = "Photo";
            SelectedImageKey = "Photo";
        }

        public void RefreshNode()
        {
            Text = Photograph.Caption;
        }
    }
}