using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PhotoAlbum;
using MyPhotoControls;
namespace MyPhotos
{
    public partial class ParentForm : Form
    {
        public ParentForm()
        {
            InitializeComponent();
        }

        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuFileNew_Click(object sender, EventArgs e)
        {
            CreateMdiChild(new Form1());
        }
        private void CreateMdiChild(
Form child)
        {
            child.MdiParent = this;
            child.Show();
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenAlbum();
        }
        private void OpenAlbum()
        {
            string path = null;
            string pwd = null;
            if (AlbumController.OpenAlbumDialog( ref path, ref pwd))
            {
                try
                {
                    foreach (Form f in MdiChildren)
                    {
                        Form1 mf = f as Form1;
                        if (mf != null && mf.AlbumPath == path)
                        {
                            // Show existing child
                            if (mf.WindowState
                            == FormWindowState.Minimized)
                                mf.WindowState
                                = FormWindowState.Normal;
                            mf.BringToFront();
                            return;
                        }
                    }
                    CreateMdiChild(new Form1(path, pwd));
                }
                catch (AlbumStorageException aex)
                {
                    MessageBox.Show(this,
                    "Unable to open album " + path
                    + "\n [" + aex.Message + "]",
                    "Open Album Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                }
            }
        }

        private void menuFileClose_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
                ActiveMdiChild.Close();
        }

        private void menuFile_DropDownOpening(object sender, EventArgs e)
        {
            menuFileClose.Enabled
= (this.ActiveMdiChild != null);
        }
        protected override void OnLoad(
        EventArgs e)
        {
            ComponentResourceManager resources
                = new ComponentResourceManager(
                typeof(Form1));
            Image newImage = (Image)resources.
            GetObject("menuFileNew.Image");
            Image openImage = (Image)resources.
            GetObject("menuFileOpen.Image");
            menuFileNew.Image = newImage;
            menuFileOpen.Image = openImage;
            tsbNew.Image = newImage;
            tsbOpen.Image = openImage;
            PixelDialog.GlobalMdiParent = this;
            SetTitleBar();
            base.OnLoad(e);
        }

        private void tsbNew_Click(object sender, EventArgs e)
        {
            CreateMdiChild(new Form1());
        }

        private void tsbOpen_Click(object sender, EventArgs e)
        {
            OpenAlbum();
        }
        protected override void OnMdiChildActivate(EventArgs e)
        {
            ToolStripManager.RevertMerge(
        toolStripParent);
            Form1 f = ActiveMdiChild as Form1;
            if (f != null)
            {
                ToolStripManager.Merge(
                f.MainToolStrip,
                toolStripParent.Name);
                toolStripParent.ImageList
                = f.MainToolStrip.ImageList;
            }
            SetTitleBar();
            base.OnMdiChildActivate(e);
        }
        protected void SetTitleBar()
        {
            Version ver = new Version(Application.ProductVersion);

            string titleBarFormat = "{0} - MyPhotos MDI {1:#}.{2:#} TryIt";
            string childName = "Untitled";

            Form1 mf = ActiveMdiChild as Form1;
            if (mf != null && !String.IsNullOrEmpty(mf.AlbumTitle))
                childName = mf.AlbumTitle;
            else if (ActiveMdiChild is PixelDialog)
                childName = "Pixel Data";

            Text = String.Format(titleBarFormat, childName, ver.Major, ver.Minor);
        }

    }
}
