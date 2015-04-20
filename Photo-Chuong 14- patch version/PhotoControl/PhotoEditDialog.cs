using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Manning.PhotoAlbum;
using System.Collections.Specialized;
namespace Manning.PhotoControl
{
    public partial class PhotoEditDialog : Manning.PhotoControl.BaseEditDialog
    {
     	private Photograph _photo;
		private Photograph Photo { get { return _photo; } }

		private AlbumManager _manager = null;
		private AlbumManager Manager { get { return _manager; } }

		protected PhotoEditDialog()
		{
			// Call required by Windows Form Designer.
			InitializeComponent();
		}

		private void InitializeDialog(Photograph photo)
		{
			_photo = photo;
			ResetDialog();

		}

		public PhotoEditDialog(Photograph photo)
			: this()
		{
			if (photo == null)
				throw new ArgumentNullException(
					"The photo parameter cannot be null");

			InitializeDialog(photo);
		}

		public PhotoEditDialog(AlbumManager mgr)
			: this()
		{
			if (mgr == null)
				throw new ArgumentNullException(
					"The mgr parameter cannot be null");

			_manager = mgr;
			InitializeDialog(mgr.Current);
		}

		protected override void ResetDialog()
		{
            comboPhotographer.BeginUpdate();
            comboPhotographer.Items.Clear();

            if (Manager != null)
            {
                StringCollection coll = Manager.Photographers;
                foreach (string s in coll)
                    comboPhotographer.Items.Add(s);
            }
            else
                comboPhotographer.Items.Add(Photo.Photographer);

            comboPhotographer.EndUpdate();

            // Initialize form contents
            Photograph photo = Photo;
            if (photo != null)
            {
                txtPhotoFile.Text = photo.FileName;
                txtCaption.Text = photo.Caption;
                dtpDateTaken.Value = photo.DateTaken;
                comboPhotographer.Text = photo.Photographer;
                txtNotes.Text = photo.Notes;
            }
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
				SaveSettings();
		}

		private void SaveSettings()
		{
			Photograph photo = Photo;

			if (photo != null)
			{
                photo.Caption = txtCaption.Text;
                photo.Photographer = comboPhotographer.Text;
				photo.Notes = txtNotes.Text;
                photo.DateTaken = dtpDateTaken.Value;
				// On parse error, do not set date
			
			}
		}

        private void txtCaption_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            e.Handled = !(Char.IsLetterOrDigit(c)
                || Char.IsWhiteSpace(c)
                || Char.IsControl(c));

            if (e.Handled)
                Console.Beep();
        }

        private void mskDateTaken_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput)
			{
				DialogResult result = MessageBox.Show(
					"The Date Taken entry is invalid or "
						+ "in the future and will be ignored."
						+ " Do you wish to correct this?",
					"Photo Properties",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

				e.Cancel = (result == DialogResult.Yes);
			}
        }

        private void txtCaption_TextChanged(object sender, EventArgs e)
        {
            Text = txtCaption + " - Properties";
        }
        private void comboPhotographer_Leave(object sender, EventArgs e)
        {
            string person = comboPhotographer.Text;
            if (!comboPhotographer.Items.Contains(person))
                comboPhotographer.Items.Add(person);
        }
    }
}
