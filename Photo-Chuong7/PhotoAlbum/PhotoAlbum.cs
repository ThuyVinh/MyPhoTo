using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace Manning.PhotoAlbum
{
    public class PhotoAlbum : Collection<Photograph>, IDisposable
    {
        private bool hasChanged = false;
        public bool HasChanged
        {
            get
            {
                if (hasChanged) return true;
                foreach (Photograph p in this)
                    if (p.HasChanged) return true;
                return false;
            }
            set
            {
                hasChanged = value;
                if (value == false)
                    foreach (Photograph p in this)
                        p.HasChanged = false;
            }
        }

        public Photograph Add(string fileName)
        {
            Photograph p = new Photograph(fileName);
            base.Add(p);
            return p;
        }
        protected override void ClearItems()
        {
            if (Count > 0)
            {
                Dispose();
                base.ClearItems();
                HasChanged = true;
            }
        }
        protected override void InsertItem(int index, Photograph item)
        {
            base.InsertItem(index, item);
            HasChanged = true;
        }
        protected override void RemoveItem(int index)
        {
            Items[index].Dispose();
            base.RemoveItem(index);
            hasChanged = true;
        }
        protected override void SetItem(int index, Photograph item)
        {
            base.SetItem(index, item);
            HasChanged = true;
        }
        public void Dispose()
        {
            foreach (Photograph p in this)
                p.Dispose();
        }
        }
    
    }


